using OpenTK.Graphics.OpenGL;

namespace SFGenericModel
{           
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
        public readonly int valueCount;

        /// <summary>
        /// The data type of the attribute value.
        /// </summary>
        public readonly VertexAttribPointerType vertexAttribPointerType;

        /// <summary>
        /// The total size of the attribute in bytes.
        /// This is usually <c>sizeof(float) * valueCount</c>.
        /// </summary>
        public readonly int sizeInBytes;

        /// <summary>
        /// Creates a new vertex attribute.
        /// </summary>
        /// <param name="name">The name of the attribute in the shader</param>
        /// <param name="valueCount">The number of components for the value</param>
        /// <param name="vertexAttribPointerType">The data type of the value</param>
        /// <param name="sizeInBytes">The total size in bytes of the value</param>
        public VertexAttributeInfo(string name, int valueCount, VertexAttribPointerType vertexAttribPointerType, int sizeInBytes)
        {
            this.name = name;
            this.valueCount = valueCount;
            this.vertexAttribPointerType = vertexAttribPointerType;
            this.sizeInBytes = sizeInBytes;
        }
    }
}
