using SFGenericModel.VertexAttributes;

namespace SFGenericModel.ShaderGenerators
{
    /// <summary>
    /// Stores information on how a vertex attribute will be displayed.
    /// </summary>
    public struct VertexAttributeRenderInfo
    {
        /// <summary>
        /// Normalize the vector before rendering.
        /// </summary>
        public readonly bool normalize;

        /// <summary>
        /// Remap values in range [-1, 1] to range [0, 1]
        /// </summary>
        public readonly bool remapToVisibleRange;

        /// <summary>
        /// Information about the attribute name and type
        /// </summary>
        public readonly VertexAttribute attributeInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="normalize"></param>
        /// <param name="remapToVisibleRange"></param>
        /// <param name="attributeInfo"></param>
        public VertexAttributeRenderInfo(bool normalize, bool remapToVisibleRange, VertexAttribute attributeInfo)
        {
            this.normalize = normalize;
            this.remapToVisibleRange = remapToVisibleRange;
            this.attributeInfo = attributeInfo;
        }
    }
}
