
namespace SFGenericModel.RenderState
{
    /// <summary>
    /// Stores the rendering state set before <see cref="GenericMesh{T}"/> is drawn.
    /// </summary>
    public class RenderSettings
    {
        /// <summary>
        /// Controls blending effects and alpha transparency.
        /// </summary>
        public AlphaBlendSettings alphaBlendSettings = new AlphaBlendSettings();

        /// <summary>
        /// Controls discarding of fragments based on alpha.
        /// </summary>
        public AlphaTestSettings alphaTestSettings = new AlphaTestSettings();

        /// <summary>
        /// Controls discarding of fragments based on depth.
        /// </summary>
        public DepthTestSettings depthTestSettings = new DepthTestSettings();

        /// <summary>
        /// Controls which faces are culled.
        /// </summary>
        public FaceCullingSettings faceCullingSettings = new FaceCullingSettings();

        /// <summary>
        /// 
        /// </summary>
        public RenderSettings()
        {
            alphaBlendSettings = AlphaBlendSettings.Default;
            alphaTestSettings = AlphaTestSettings.Default;
            depthTestSettings = DepthTestSettings.Default;
            faceCullingSettings = FaceCullingSettings.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var settings = obj as RenderSettings;
            return settings != null &&
                settings.alphaBlendSettings.Equals(alphaBlendSettings) &&
                settings.alphaTestSettings.Equals(alphaTestSettings) &&
                settings.depthTestSettings.Equals(depthTestSettings) &&
                settings.faceCullingSettings.Equals(faceCullingSettings);
        }
    }
}
