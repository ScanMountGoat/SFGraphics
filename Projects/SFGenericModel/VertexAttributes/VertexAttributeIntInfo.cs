using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.VertexAttributes
{
    /// <summary>
    /// Stores the information used to configure vertex attributes for <see cref="GenericMesh{T}"/>.
    /// </summary>
    public class VertexAttributeIntInfo : VertexAttribute
    {
        /// <summary>
        /// Creates a new vertex attribute.
        /// </summary>
        /// <param name="name">The name of the attribute in the shader</param>
        /// <param name="valueCount">The number of components for the value</param>
        /// <param name="type">The data type of the value</param>
        /// <exception cref="System.NotSupportedException"><paramref name="type"/> is not 
        /// a supported attribute type.</exception>
        public VertexAttributeIntInfo(string name, ValueCount valueCount, VertexAttribIntegerType type) 
            : base(name, valueCount, (VertexAttribPointerType)type)
        {
            SizeInBytes = (int)valueCount * AttribPointerUtils.GetSizeInBytes(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">The index of the attribute variable in the shader</param>
        /// <param name="strideInBytes">The vertex size in bytes</param>
        /// <param name="offsetInBytes">The offset of the attribute in the vertex</param>
        public override void SetVertexAttribute(int index, int strideInBytes, int offsetInBytes)
        {
            GL.VertexAttribIPointer(index, (int)ValueCount, (VertexAttribIntegerType)Type, strideInBytes, new System.IntPtr(offsetInBytes));
        }
    }
}
