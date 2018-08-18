using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    partial class RenderSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public class FaceCullingSettings
        {
            public bool enableFaceCulling = true;

            public CullFaceMode cullFaceMode = CullFaceMode.Back;
        }
    }
}
