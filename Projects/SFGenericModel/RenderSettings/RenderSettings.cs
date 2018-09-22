
using System.Collections.Generic;

namespace SFGenericModel.RenderState
{
    /// <summary>
    /// Stores the rendering state set before <see cref="GenericMesh{T}"/> is drawn.
    /// </summary>
    public class RenderSettings
    {
        /// <summary>
        /// Controls the appearance of primitives.
        /// </summary>
        public PolygonModeSettings polygonModeSettings = PolygonModeSettings.Default;

        /// <summary>
        /// Controls blending effects and alpha transparency.
        /// </summary>
        public AlphaBlendSettings alphaBlendSettings = AlphaBlendSettings.Default;

        /// <summary>
        /// Controls discarding of fragments based on alpha.
        /// </summary>
        public AlphaTestSettings alphaTestSettings = AlphaTestSettings.Default;

        /// <summary>
        /// Controls discarding of fragments based on depth.
        /// </summary>
        public DepthTestSettings depthTestSettings = DepthTestSettings.Default;

        /// <summary>
        /// Controls which faces are culled.
        /// </summary>
        public FaceCullingSettings faceCullingSettings = FaceCullingSettings.Default;

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = 2089746424;
            hashCode = hashCode * -1521134295 + EqualityComparer<PolygonModeSettings>.Default.GetHashCode(polygonModeSettings);
            hashCode = hashCode * -1521134295 + EqualityComparer<AlphaBlendSettings>.Default.GetHashCode(alphaBlendSettings);
            hashCode = hashCode * -1521134295 + EqualityComparer<AlphaTestSettings>.Default.GetHashCode(alphaTestSettings);
            hashCode = hashCode * -1521134295 + EqualityComparer<DepthTestSettings>.Default.GetHashCode(depthTestSettings);
            hashCode = hashCode * -1521134295 + EqualityComparer<FaceCullingSettings>.Default.GetHashCode(faceCullingSettings);
            return hashCode;
        }
    }
}
