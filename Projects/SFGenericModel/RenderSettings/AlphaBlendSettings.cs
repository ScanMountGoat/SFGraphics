using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    partial class RenderSettings
    {
        /// <summary>
        /// The alpha blending state set before drawing a <see cref="GenericMesh{T}"/>.
        /// </summary>
        public class AlphaBlendSettings
        {
            public bool enableAlphaBlending = true;

            public BlendingFactor sourceFactor = BlendingFactor.SrcAlpha;
            public BlendingFactor destinationFactor = BlendingFactor.OneMinusSrcAlpha;

            public BlendEquationMode blendingEquationRgb = BlendEquationMode.FuncAdd;
            public BlendEquationMode blendingEquationAlpha = BlendEquationMode.FuncAdd;
        }
    }
}
