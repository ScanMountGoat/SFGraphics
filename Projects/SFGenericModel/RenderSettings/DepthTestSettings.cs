using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    partial class RenderSettings
    {
        public class DepthTestSettings
        {
            public bool enableDepthTest = true;

            public bool depthMask = true;

            public DepthFunction depthFunction = DepthFunction.Lequal;
        }
    }
}
