using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    partial class RenderSettings
    {
        public class FaceCullingSettings
        {
            public bool enableFaceCulling = true;

            public CullFaceMode cullFaceMode = CullFaceMode.Back;
        }
    }
}
