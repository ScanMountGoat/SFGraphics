using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.VertexAttributes
{           
    /// <summary>
    /// A floating point vertex attribute. Integer types are converted directly to floats.
    /// </summary>
    public sealed class VertexFloatAttribute : VertexAttribute
    {
        /// <summary>
        /// Integer types are converted to floating point when <c>true</c>.
        /// </summary>
        public bool Normalized { get; }

        /// <summary>
        /// Creates a new vertex attribute.
        /// </summary>
        /// <param name="name">The name of the attribute in the shader</param>
        /// <param name="valueCount">The number of components for the value</param>
        /// <param name="type">The data type of the value</param>
        /// <param name="normalized">Indicates whether integer types should be converted to floating point</param>
        /// <param name="attributeUsage">How the attribute should be rendered</param>
        /// <param name="normalizeVector">Normalize the vector before rendering</param>
        /// <param name="remapToVisibleRange">Remap the vector before rendering</param>
        /// <exception cref="System.NotImplementedException"><paramref name="type"/> is not an implemented attribute type.</exception>
        public VertexFloatAttribute(string name, ValueCount valueCount, VertexAttribPointerType type, bool normalized, AttributeUsage attributeUsage, bool normalizeVector, bool remapToVisibleRange) 
            : base(name, valueCount, type, attributeUsage, normalizeVector, remapToVisibleRange)
        {
            Normalized = normalized;
            SizeInBytes = (int)valueCount * AttribPointerUtils.GetSizeInBytes(type);
        }

        /// <summary>
        /// Creates a new vertex attribute.
        /// </summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="valueCount">The number of vector components</param>
        /// <param name="type">The data type</param>
        /// <param name="normalized">Integer types are converted to floating point when <c>true</c></param>
        public VertexFloatAttribute(string name, ValueCount valueCount, VertexAttribPointerType type, bool normalized) : base(name, valueCount, type, AttributeUsage.Default, false, false)
        {
            Normalized = normalized;
            SizeInBytes = (int)valueCount * AttribPointerUtils.GetSizeInBytes(type);
        }

        /// <summary>
        /// Configures the vertex attribute for the currently bound array buffer.
        /// </summary>
        /// <param name="index">The index of the attribute variable in the shader</param>
        /// <param name="strideInBytes">The vertex size in bytes</param>
        /// <param name="offsetInBytes">The offset in bytes of the attribute in the vertex</param>
        public override void SetVertexAttribute(int index, int strideInBytes, int offsetInBytes)
        {
            GL.VertexAttribPointer(index, (int)ValueCount, Type, Normalized, strideInBytes, offsetInBytes);
        }
    }
}
