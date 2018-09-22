using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    /// <summary>
    /// The alpha blending state set before drawing a <see cref="GenericMesh{T}"/>.
    /// </summary>
    public struct AlphaBlendSettings
    {
        /// <summary>
        /// The default alpha blend settings.
        /// </summary>
        public static AlphaBlendSettings Default = new AlphaBlendSettings(true, BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha, 
            BlendEquationMode.FuncAdd, BlendEquationMode.FuncAdd);

        /// <summary>
        /// Enables or disables alpha blending.
        /// </summary>
        public bool enabled;

        /// <summary>
        /// The source color is multiplied by <see cref="sourceFactor"/>.
        /// </summary>
        public BlendingFactor sourceFactor;

        /// <summary>
        /// The destination color is multiplied by <see cref="destinationFactor"/>.
        /// </summary>
        public BlendingFactor destinationFactor;

        /// <summary>
        /// The blending operation used for the RGB components.
        /// </summary>
        public BlendEquationMode blendingEquationRgb;

        /// <summary>
        /// The blending operation used for only the alpha component.
        /// </summary>
        public BlendEquationMode blendingEquationAlpha;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="sourceFactor"></param>
        /// <param name="destinationFactor"></param>
        /// <param name="blendingEquationRgb"></param>
        /// <param name="blendingEquationAlpha"></param>
        public AlphaBlendSettings(bool enabled, BlendingFactor sourceFactor, BlendingFactor destinationFactor, 
            BlendEquationMode blendingEquationRgb, BlendEquationMode blendingEquationAlpha)
        {
            this.enabled = enabled;
            this.sourceFactor = sourceFactor;
            this.destinationFactor = destinationFactor;
            this.blendingEquationRgb = blendingEquationRgb;
            this.blendingEquationAlpha = blendingEquationAlpha;
        }
    }
}
