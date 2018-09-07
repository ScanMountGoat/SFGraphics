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
            /// <summary>
            /// 
            /// </summary>
            public bool enableFaceCulling = true;

            /// <summary>
            /// 
            /// </summary>
            public CullFaceMode cullFaceMode = CullFaceMode.Back;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                var settings = obj as FaceCullingSettings;
                return settings != null &&
                       enableFaceCulling == settings.enableFaceCulling &&
                       cullFaceMode == settings.cullFaceMode;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                var hashCode = 438860308;
                hashCode = hashCode * -1521134295 + enableFaceCulling.GetHashCode();
                hashCode = hashCode * -1521134295 + cullFaceMode.GetHashCode();
                return hashCode;
            }
        }
    }
}
