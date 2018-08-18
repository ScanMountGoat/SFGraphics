
namespace SFGenericModel.RenderState
{
    /// <summary>
    /// Stores the rendering state set before <see cref="GenericMesh{T}"/> is drawn.
    /// </summary>
    public partial class RenderSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public AlphaBlendSettings alphaBlendSettings = new AlphaBlendSettings();

        /// <summary>
        /// 
        /// </summary>
        public AlphaTestSettings alphaTestSettings = new AlphaTestSettings();

        /// <summary>
        /// 
        /// </summary>
        public DepthTestSettings depthTestSettings = new DepthTestSettings();

        /// <summary>
        /// 
        /// </summary>
        public FaceCullingSettings faceCullingSettings = new FaceCullingSettings();
    }
}
