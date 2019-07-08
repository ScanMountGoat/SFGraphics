
using System.Collections.Generic;

namespace SFGenericModel.RenderState
{
    /// <summary>
    /// Stores the rendering state used for drawing.
    /// The OpenGL state can be set using <see cref="GLRenderSettings.SetRenderSettings(RenderSettings, RenderSettings)"/>.
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
        /// Compares the values of the object's fields with <paramref name="obj"/>
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        /// <returns><c>true</c> if the specified object is equal to the current object</returns>
        public override bool Equals(object obj)
        {
            return obj is RenderSettings settings &&
                settings.alphaBlendSettings.Equals(alphaBlendSettings) &&
                settings.alphaTestSettings.Equals(alphaTestSettings) &&
                settings.depthTestSettings.Equals(depthTestSettings) &&
                settings.faceCullingSettings.Equals(faceCullingSettings);
        }

        /// <summary>
        /// Returns a hash code based on the object's fields.
        /// </summary>
        /// <returns>A hash code for the current object</returns>
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
