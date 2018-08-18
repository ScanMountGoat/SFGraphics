using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    partial class RenderSettings
    {
        /// <summary>
        /// The face culling state set before drawing a <see cref="GenericMesh{T}"/>.
        /// </summary>
        public class FaceCullingSettings
        {
            public bool enableFaceCulling = true;

            public CullFaceMode cullFaceMode = CullFaceMode.Back;
        }
    }
}
