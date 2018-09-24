namespace SFGenericModel.VertexAttributes
{
    /// <summary>
    /// Provides methods for setting vertex attributes.
    /// </summary>
    public interface ISettableVertexAttribute
    {
        /// <summary>
        /// Configures the vertex attribute for the currently bound element array buffer.
        /// </summary>
        /// <param name="index">The index of the attribute variable in the shader</param>
        /// <param name="strideInBytes">The vertex size in bytes</param>
        /// <param name="offsetInBytes">The offset of the attribute in the vertex</param>
        void SetVertexAttribute(int index, int strideInBytes, int offsetInBytes);
    }
}
