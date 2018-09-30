using SFGenericModel.VertexAttributes;

namespace SFGenericModel.ShaderGenerators
{
    /// <summary>
    /// Stores information on how a vertex attribute will be displayed.
    /// </summary>
    public struct VertexAttributeRenderInfo
    {
        /// <summary>
        /// The name value of <see cref="AttributeInfo"/>
        /// </summary>
        public string Name { get { return AttributeInfo.Name; } }

        /// <summary>
        /// Normalize the vector before rendering.
        /// </summary>
        public bool Normalize { get; }

        /// <summary>
        /// Remap values in range [-1, 1] to range [0, 1]
        /// </summary>
        public bool RemapToVisibleRange { get; }

        /// <summary>
        /// Information about the attribute name and type
        /// </summary>
        public VertexAttribute AttributeInfo { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="normalize"></param>
        /// <param name="remapToVisibleRange"></param>
        /// <param name="attributeInfo"></param>
        public VertexAttributeRenderInfo(bool normalize, bool remapToVisibleRange, VertexAttribute attributeInfo)
        {
            Normalize = normalize;
            RemapToVisibleRange = remapToVisibleRange;
            AttributeInfo = attributeInfo;
        }
    }
}
