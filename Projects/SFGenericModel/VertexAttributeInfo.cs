using OpenTK.Graphics.OpenGL;

namespace SFGenericModel
{           
    /// <summary>
    /// The number of components for a vertex attribute.
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
    /// Stores the information used to configure vertex attributes for <see cref="GenericMesh{T}"/>.
    /// </summary>
    public struct VertexAttributeInfo
    {
        /// <summary>
        /// The name of the attribute in the shader.
        /// </summary>
        public readonly string name;

        /// <summary>
        /// The number of components. Ex: 1 for <see cref="float"/> or
        /// 4 for Vector4.
        /// </summary>
        public readonly ValueCount valueCount;

        /// <summary>
        /// The data type of the attribute value.
        /// </summary>
        public readonly VertexAttribPointerType type;

        /// <summary>
        /// The total size of the attribute's data in bytes.
        /// </summary>
        public readonly int sizeInBytes;

        /// <summary>
        /// Creates a new vertex attribute.
        /// </summary>
        /// <param name="name">The name of the attribute in the shader</param>
        /// <param name="valueCount">The number of components for the value</param>
        /// <param name="vertexAttribPointerType">The data type of the value</param>
        /// <exception cref="System.NotSupportedException"><paramref name="vertexAttribPointerType"/> is not 
        /// a supported attribute type.</exception>
        public VertexAttributeInfo(string name, ValueCount valueCount, VertexAttribPointerType vertexAttribPointerType)
        {
            this.name = name;
            this.valueCount = valueCount;
            this.type = vertexAttribPointerType;

            if (!AttribPointerUtils.sizeInBytesByType.ContainsKey(vertexAttribPointerType))
                throw new System.NotSupportedException($"{ vertexAttribPointerType.ToString() } is not a supported type.");

            sizeInBytes = (int)valueCount * AttribPointerUtils.sizeInBytesByType[vertexAttribPointerType];
        }
    }
}
