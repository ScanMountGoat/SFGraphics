using OpenTK.Graphics.OpenGL;
using SFGraphics.Utils;
using System;

namespace SFGraphics.GLObjects.BufferObjects
{
    /// <summary>
    /// Encapsulates and OpenGL buffer object, which stores unformatted data to be used by the GPU.
    /// </summary>
    public sealed class BufferObject : GLObject
    {
        internal override GLObjectType ObjectType => GLObjectType.BufferObject;

        /// <summary>
        /// The target to which <see cref="GLObject.Id"/> is bound when calling Bind().
        /// </summary>
        public BufferTarget Target { get; }

        /// <summary>
        /// The size in bytes of the initialized data. The actual capacity may be bigger.
        /// </summary>
        public int SizeInBytes => itemCountPreviousWrite * itemSizeInBytesPreviousWrite;

        // Store information from previous write to allow for bounds checking.
        private int itemCountPreviousWrite;
        private int itemSizeInBytesPreviousWrite;

        /// <summary>
        /// Creates a buffer of the specified target with uninitialized data.
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
        /// Binds <see cref="GLObject.Id"/> to <paramref name="target"/>.
        /// </summary>
        /// <param name="target">The target for the bind</param>
        public void Bind(BufferTarget target)
        {
            GL.BindBuffer(target, Id);
        }

        /// <summary>
        /// Binds <see cref="GLObject.Id"/> to an indexed buffer target.
        /// </summary>
        /// <param name="target">The target for the bind</param>
        /// <param name="index">The index of the binding point</param>
        public void BindBase(BufferRangeTarget target, int index)
        {
            if (index != -1)
                GL.BindBufferBase(target, index, Id);
        }

        /// <summary>
        /// Binds a range of this buffers data to an indexed buffer target.
        /// </summary>
        /// <param name="target">The target for the bind</param>
        /// <param name="index">The index of the binding point</param>
        /// <param name="offsetInBytes"></param>
        /// <param name="sizeInBytes"></param>
        public void BindRange(BufferRangeTarget target, int index, int offsetInBytes, int sizeInBytes)
        {
            // TODO: Check for out of bounds data ranges.
            if (index != -1)
                GL.BindBufferRange(target, index, Id, new IntPtr(offsetInBytes), sizeInBytes);
        }

        /// <summary>
        /// Sets the buffers capacity to <paramref name="sizeInBytes"/>, invalidating existing data.
        /// </summary>
        /// <param name="sizeInBytes">The new buffer capacity</param>
        /// <param name="usageHint">A hint on how the data will be used, which allows performance optimizations</param>
        public void SetCapacity(int sizeInBytes, BufferUsageHint usageHint)
        {
            if (sizeInBytes < 0)
                throw new ArgumentOutOfRangeException(nameof(sizeInBytes), "The buffer size must be non negative.");

            // Workaround to ensure bounds checking still works properly.
            itemCountPreviousWrite = 1;
            itemSizeInBytesPreviousWrite = sizeInBytes;

            Bind();
            GL.BufferData(Target, sizeInBytes, IntPtr.Zero, usageHint);
        }

        /// <summary>
        /// Initializes the buffer's data with the specified array.
        /// <paramref name="data"/> should be contiguous in memory, so only 
        /// </summary>
        /// <typeparam name="T">The type of each item</typeparam>
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
        /// </summary>
        /// <typeparam name="T">The type of each item.</typeparam>
        /// <param name="data">The data used to initialize the buffer's data.</param>
        /// <param name="offsetInBytes">The offset where data replacement will begin</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified range includes data 
        /// outside the buffer's current capacity.</exception>        
        public void SetSubData<T>(T[] data, int offsetInBytes) where T : struct
        {
            int itemSizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
            if (!BufferValidation.IsValidAccess(offsetInBytes, itemSizeInBytes, data.Length, SizeInBytes))
                throw new ArgumentOutOfRangeException("", BufferExceptionMessages.outOfRange);


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

            if ((SizeInBytes % itemSizeInBytes) != 0)
                throw new ArgumentOutOfRangeException(nameof(T), BufferExceptionMessages.bufferNotDivisibleByRequestedType);

            int newItemCount = SizeInBytes / itemSizeInBytes;

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
        /// <exception cref="ArgumentOutOfRangeException">The specified range includes data outside the buffer's current capacity.</exception>
        public T[] GetSubData<T>(int offsetInBytes, int itemCount) where T : struct
        {
            int itemSizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
            if (!BufferValidation.IsValidAccess(offsetInBytes, itemSizeInBytes, itemCount, SizeInBytes))
                throw new ArgumentOutOfRangeException("", BufferExceptionMessages.outOfRange);

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
