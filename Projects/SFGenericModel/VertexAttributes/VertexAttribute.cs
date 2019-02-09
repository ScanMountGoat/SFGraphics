using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.VertexAttributes
{
    /// <summary>
    /// The number of vector components for a vertex attribute. Scalars should use <see cref="ValueCount.One"/>.
    /// </summary>
    public enum ValueCount
    {
        /// <summary>
        /// A scalar value
        /// </summary>
        One = 1,

        /// <summary>
        /// A two component vector value
        /// </summary>
        Two = 2,

        /// <summary>
        /// A three component vector value
        /// </summary>
        Three = 3,

        /// <summary>
        /// A four component vector value
        /// </summary>
        Four = 4
    }

    /// <summary>
    /// Stores information for a vertex attribute variable.
    /// </summary>
    public abstract class VertexAttribute : System.Attribute
    {
        /// <summary>
        /// The name of the attribute in the shader.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The number of components. Ex: 1 for <see cref="float"/> or
        /// 4 for Vector4.
        /// </summary>
        public ValueCount ValueCount { get; }

        /// <summary>
        /// The total size of the attribute's data in bytes.
        /// </summary>
        public int SizeInBytes { get; protected set; }

        /// <summary>
        /// The data type of the attribute value.
        /// </summary>
        public VertexAttribPointerType Type { get; }

        /// <summary>
        /// Configures the vertex attribute for the currently bound array buffer.
        /// </summary>
        /// <param name="index">The index of the attribute variable in the shader</param>
        /// <param name="strideInBytes">The vertex size in bytes</param>
        /// <param name="offsetInBytes">The offset in bytes of the attribute in the vertex</param>
        public abstract void SetVertexAttribute(int index, int strideInBytes, int offsetInBytes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="valueCount"></param>
        /// <param name="type"></param>
        protected VertexAttribute(string name, ValueCount valueCount, VertexAttribPointerType type)
        {
            Name = name;
            ValueCount = valueCount;
            Type = type;
        }
    }
}
