namespace SFGenericModel.VertexAttributes
{
    /// <summary>
    /// Contains methods for configuring attributes for element array buffers.
    /// </summary>
    public interface IVertexArrayAttrib
    {
        /// <summary>
        /// Configures the vertex attribute for the currently bound array buffer.
        /// </summary>
        /// <param name="index">The index of the attribute variable in the shader</param>
        /// <param name="strideInBytes">The vertex size in bytes</param>
        /// <param name="offsetInBytes">The offset in bytes of the attribute in the vertex</param>
        void SetVertexAttribute(int index, int strideInBytes, int offsetInBytes);
    }
}
