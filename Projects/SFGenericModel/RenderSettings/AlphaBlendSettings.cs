using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    partial class RenderSettings
    {
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
