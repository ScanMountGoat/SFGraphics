using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    partial class RenderSettings
    {
        /// <summary>
        /// The alpha testing state set before drawing a <see cref="GenericMesh{T}"/>.
        /// </summary>
        public class AlphaTestSettings
        {
            public bool enableAlphaTesting = false;

            public AlphaFunction alphaFunction = AlphaFunction.Gequal;

            public int referenceAlpha = 128;
        }
    }
}
