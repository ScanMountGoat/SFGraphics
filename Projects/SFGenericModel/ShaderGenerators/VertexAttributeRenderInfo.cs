using SFGenericModel.VertexAttributes;

namespace SFGenericModel.ShaderGenerators
{
    /// <summary>
    /// Determines how a <see cref="VertexAttribute"/> should be rendered for generated shaders.
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
        /// <param name="attributeInfo">Information about the attribute name and type</param>
        /// <param name="normalize">Normalize the vector before rendering</param>
        /// <param name="remapToVisibleRange">Remap values in range [-1, 1] to range [0, 1]</param>
        public VertexAttributeRenderInfo(VertexAttribute attributeInfo, bool normalize  = false, bool remapToVisibleRange = false)
        {
            Normalize = normalize;
            RemapToVisibleRange = remapToVisibleRange;
            AttributeInfo = attributeInfo;
        }
    }
}
