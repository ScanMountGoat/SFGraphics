using System;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects
{
    /// <summary>
    /// Encapsulates an OpenGL buffer object. Like other GLOBjects, memory is handled by GLObjectManager.
    /// </summary>
    public sealed class BufferObject : GLObject
    {
        /// <summary>
        /// Returns the type of OpenGL object. Used for memory management.
        /// </summary>
        public override GLObjectType ObjectType { get { return GLObjectType.Buffer; } }

        /// <summary>
        /// The target to which <see cref="GLObject.Id"/> is bound when calling Bind().
        /// </summary>
        public BufferTarget BufferTarget { get; }

        private int itemCount = 0;
        private int itemSizeInBytes = 0;

        private int BufferSize
        {
            get { return itemCount * itemSizeInBytes; }
        }

        /// <summary>
        /// Creates a buffer of the specified target with unitialized data.
        /// </summary>
        /// <param name="bufferTarget">The target to which <see cref="GLObject.Id"/> is bound</param>
        public BufferObject(BufferTarget bufferTarget) : base(GL.GenBuffer())
        {
            BufferTarget = bufferTarget;
        }

        /// <summary>
        /// Binds <see cref="GLObject.Id"/> to the buffer target specified at creation.
        /// </summary>
        public void Bind()
        {
            GL.BindBuffer(BufferTarget, Id);
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
        /// non nullable structs containing value types as members will work properly.
        /// </summary>
        /// <typeparam name="T">The type of each item. This includes arithmetic types like <see cref="int"/>.</typeparam>
        /// <param name="data">The data used to initialize the buffer's data</param>
        /// <param name="itemSizeInBytes">The size of <typeparamref name="T"/> in bytes</param>
        /// <param name="bufferUsageHint">A hint on how the data will be used, which allows performance optimizations</param>
        public void BufferData<T>(T[] data, int itemSizeInBytes, BufferUsageHint bufferUsageHint) where T : struct
        {
            Bind();
            itemCount = data.Length;
            this.itemSizeInBytes = itemSizeInBytes;
            GL.BufferData(BufferTarget, itemSizeInBytes * data.Length, data, bufferUsageHint);
        }

        /// <summary>
        /// Initializes a portion of the buffer's data with the specified array.
        /// <paramref name="data"/> should be contiguous in memory, so only 
        /// non nullable structs containing value types as members will work properly.
        /// </summary>
        /// <typeparam name="T">The type of each item. This includes arithmetic types like <see cref="int"/>.</typeparam>
        /// <param name="data">The data used to initialize the buffer's data</param>
        /// <param name="offset">The offset in bytes where data replacement will begin</param>
        /// <param name="itemSizeInBytes">The size of <typeparamref name="T"/> in bytes</param>
        public void BufferSubData<T>(T[] data, int offset, int itemSizeInBytes) where T : struct
        {
            // Throw exception for attempts to write data outside the current range.
            if (offset < 0 || itemSizeInBytes < 0)
                throw new ArgumentOutOfRangeException(BufferObjects.BufferObjectExceptionMessages.offsetAndItemSizeMustBeNonNegative);

            int newBufferSize = offset + (data.Length * itemSizeInBytes);
            if (newBufferSize > BufferSize)
                throw new ArgumentOutOfRangeException(BufferObjects.BufferObjectExceptionMessages.subDataTooLong);

            Bind();
            // Attempts to initialize data outside the buffer's range will generate an error.
            GL.BufferSubData(BufferTarget, new IntPtr(offset), itemSizeInBytes * data.Length, data);
        }

        /// <summary>
        /// Binds the buffer and reads all of the data initialized by <see cref="BufferData{T}(T[], int, BufferUsageHint)"/>
        /// or <see cref="BufferSubData{T}(T[], int, int)"/>. 
        /// <para></para><para></para>
        /// The data returned may not be valid if the buffer's data is manually modified using GL.BufferData() or 
        /// GL.BufferSubData(). In this case, use GL.GetBufferSubData() with the appropriate arguments.
        /// </summary>
        /// <typeparam name="T">The type specified for each item when initializing the buffer's data.</typeparam>
        /// <returns>The buffer's data in the specified type</returns>
        public T[] GetBufferData<T>() where T : struct
        { 
            Bind();

            T[] data = new T[itemCount];
            GL.GetBufferSubData(BufferTarget, IntPtr.Zero, itemCount * itemSizeInBytes, data);

            return data;
        }
    }
}
