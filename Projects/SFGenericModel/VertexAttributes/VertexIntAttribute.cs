using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.VertexAttributes
{
    /// <summary>
    /// A vertex attribute that preserves integer values. Only integer types are supported.
    /// </summary>
    public sealed class VertexIntAttribute : VertexAttribute
    {
        /// <summary>
        /// Creates a new vertex attribute.
        /// </summary>
        /// <param name="name">The name of the attribute in the shader</param>
        /// <param name="valueCount">The number of components for the value</param>
        /// <param name="type">The data type of the value</param>
        /// <param name="attributeUsage"></param>
        /// <param name="normalizeVector"></param>
        /// <param name="remapToVisibleRange"></param>
        /// <exception cref="System.NotSupportedException"><paramref name="type"/> is not 
        /// a supported attribute type.</exception>
        public VertexIntAttribute(string name, ValueCount valueCount, VertexAttribIntegerType type, AttributeUsage attributeUsage, bool normalizeVector, bool remapToVisibleRange) 
            : base(name, valueCount, (VertexAttribPointerType)type, attributeUsage, normalizeVector, remapToVisibleRange)
        {
            // The default attribute pointer type enum contains all the integer values.
            SizeInBytes = (int)valueCount * AttribPointerUtils.GetSizeInBytes(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="valueCount">The number of vector components</param>
        /// <param name="type">The data type</param>
        public VertexIntAttribute(string name, ValueCount valueCount, VertexAttribIntegerType type) : base(name, valueCount, (VertexAttribPointerType)type, AttributeUsage.Default, false, false)
        {
            // The default attribute pointer type enum contains all the integer values.
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
            GL.VertexAttribIPointer(index, (int)ValueCount, (VertexAttribIntegerType)Type, strideInBytes, new System.IntPtr(offsetInBytes));
        }
    }
}
