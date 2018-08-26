using System;
using SFGraphics.GLObjects.BufferObjects;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects
{
    /// <summary>
    /// Data can be read from and written to the buffer using any value type.
    /// <para></para><para></para>
    /// This class does not permanently store the data used for initialization. 
    /// </summary>
    public sealed class BufferObject : GLObject
    {
        /// <summary>
        /// Returns the type of OpenGL object. Used for memory management.
        /// </summary>
        public override GLObjectType ObjectType { get { return GLObjectType.BufferObject; } }

        /// <summary>
        /// The target to which <see cref="GLObject.Id"/> is bound when calling Bind().
        /// </summary>
        public BufferTarget Target { get; }

        // From last time setting the data.
        private int itemCountPreviousWrite = 0;
        private int itemSizeInBytesPreviousWrite = 0;

        // The actual capacity of the buffer may be bigger.
        private int TotalSizeInBytesPreviousWrite
        {
            get { return itemCountPreviousWrite * itemSizeInBytesPreviousWrite; }
        }

        /// <summary>
        /// Creates a buffer of the specified target with unitialized data.
        /// </summary>
        /// <param name="target">The target to which <see cref="GLObject.Id"/> is bound</param>
        public BufferObject(BufferTarget target) : base(GL.GenBuffer())
        {
            Target = target;
        }

        /// <summary>
        /// Binds <see cref="GLObject.Id"/> to the buffer target specified at creation.
        /// </summary>
        public void Bind()
        {
            GL.BindBuffer(Target, Id);
        }

        /// <summary>
        /// Binds <see cref="GLObject.Id"/> to an indexed buffer target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="index"></param>
        public void BindBase(BufferRangeTarget target, int index)
        {
            GL.BindBufferBase(target, index, Id);
        }

        /// <summary>
        /// Initializes the buffer's data with the specified array.
        /// <paramref name="data"/> should be contiguous in memory, so only 
        /// </summary>
        /// <typeparam name="T">The type of each item. This includes arithmetic types like <see cref="int"/>.</typeparam>
        /// <param name="data">The data used to initialize the buffer's data</param>
        /// <param name="usageHint">A hint on how the data will be used, which allows performance optimizations</param>
        public void SetData<T>(T[] data, BufferUsageHint usageHint) where T : struct
        {
            itemCountPreviousWrite = data.Length;
            itemSizeInBytesPreviousWrite = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

            Bind();
            GL.BufferData(Target, itemSizeInBytesPreviousWrite * data.Length, data, usageHint);
        }

        /// <summary>
        /// Initializes a portion of the buffer's data with the specified array.
        /// <paramref name="data"/> should be contiguous in memory, so only 
        /// non nullable structs containing value types as members will work properly.
        /// </summary>
        /// <typeparam name="T">The type of each item. This includes arithmetic types like <see cref="int"/>.</typeparam>
        /// <param name="data">The data used to initialize the buffer's data.</param>
        /// <param name="offsetInBytes">The offset where data replacement will begin</param>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">The specified range includes data 
        /// outside the buffer's current capacity.</exception>        
        public void SetSubData<T>(T[] data, int offsetInBytes) where T : struct
        {
            int itemSizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

            if (offsetInBytes < 0 || itemSizeInBytes < 0)
                throw new ArgumentOutOfRangeException(BufferExceptionMessages.offsetAndItemSizeMustBeNonNegative);

            int newBufferSize = CalculateRequiredSize(offsetInBytes, data.Length, itemSizeInBytes);
            if (newBufferSize > TotalSizeInBytesPreviousWrite)
                throw new ArgumentOutOfRangeException(BufferExceptionMessages.subDataTooLong);

            Bind();
            GL.BufferSubData(Target, new IntPtr(offsetInBytes), itemSizeInBytes * data.Length, data);
        }

        /// <summary>
        /// Reads the buffer's data into structs of type <typeparamref name="T"/>.
        /// <para></para><para></para>
        /// The data returned may not be valid if the buffer's data is modified using <see cref="MapBuffer(BufferAccess)"/>.
        /// </summary>
        /// <typeparam name="T">The type specified for each item when initializing the buffer's data.</typeparam>
        /// <returns>An array of all the buffer's initialized data</returns>
        public T[] GetData<T>() where T : struct
        {
            int itemSizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

            if ((TotalSizeInBytesPreviousWrite % itemSizeInBytes) != 0)
                throw new ArgumentOutOfRangeException(BufferExceptionMessages.bufferNotDivisibleByRequestedType);

            int newItemCount = TotalSizeInBytesPreviousWrite / itemSizeInBytes;

            Bind();
            T[] data = new T[newItemCount];
            GL.GetBufferSubData(Target, IntPtr.Zero, newItemCount * itemSizeInBytes, data);

            return data;
        }

        /// <summary>
        /// Binds the buffer and reads <paramref name="itemCount"/> elements of type <typeparamref name="T"/> 
        /// starting at <paramref name="offsetInBytes"/>.
        /// </summary>
        /// <typeparam name="T">The type of each item. This includes arithmetic types like <see cref="int"/>.</typeparam>
        /// <param name="offsetInBytes">The starting offset for reading</param>
        /// <param name="itemCount">The number of items of type <typeparamref name="T"/> to read.</param>
        /// <returns>An array of size <paramref name="itemCount"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">The specified range includes data 
        /// outside the buffer's current capacity.</exception>
        public T[] GetSubData<T>(int offsetInBytes, int itemCount) where T : struct
        {
            int itemSizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

            // Throw exception for attempts to read data outside the current range.
            if (offsetInBytes < 0 || itemCount < 0 || itemSizeInBytes < 0)
                throw new ArgumentOutOfRangeException(BufferExceptionMessages.offsetAndItemSizeMustBeNonNegative);

            int newBufferSize = CalculateRequiredSize(offsetInBytes, itemCount, itemSizeInBytes);
            if (newBufferSize > TotalSizeInBytesPreviousWrite)
                throw new ArgumentOutOfRangeException(BufferExceptionMessages.subDataTooLong);

            Bind();

            T[] data = new T[itemCount];
            GL.GetBufferSubData(Target, new IntPtr(offsetInBytes), itemCount * itemSizeInBytes, data);

            return data;
        }

        /// <summary>
        /// Maps and gets a pointer to the buffer's data store.
        /// Using the pointer in a manner inconsistent with <paramref name="access"/>
        /// will result in system errors.
        /// </summary>
        /// <param name="access">Specifies read and/or write access for the mapped data</param>
        /// <returns>An IntPtr for the buffer's data</returns>
        public IntPtr MapBuffer(BufferAccess access)
        {
            Bind();
            return GL.MapBuffer(Target, access);
        }

        /// <summary>
        /// Releases the mapping of the buffer's data store.
        /// If false, the buffer's data was corrupted while mapped
        /// and should be reinitialized.
        /// </summary>
        /// <returns><c>true</c> if the data was not corrupted while mapped</returns>
        public bool Unmap()
        {
            Bind();
            return GL.UnmapBuffer(Target);
        }

        private static int CalculateRequiredSize(int offset, int itemCount, int itemSizeInBytes)
        {
            return offset + (itemCount * itemSizeInBytes);
        }
    }
}
