using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    partial class RenderSettings
    {
        /// <summary>
        /// The alpha blending state set before drawing a <see cref="GenericMesh{T}"/>.
        /// </summary>
        public class DepthTestSettings
        {
            public bool enableDepthTest = true;

            public bool depthMask = true;

            public DepthFunction depthFunction = DepthFunction.Lequal;
        }
    }
}
