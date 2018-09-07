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
            /// <summary>
            /// 
            /// </summary>
            public bool enableDepthTest = true;

            /// <summary>
            /// 
            /// </summary>
            public bool depthMask = true;

            /// <summary>
            /// 
            /// </summary>
            public DepthFunction depthFunction = DepthFunction.Lequal;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                var settings = obj as DepthTestSettings;
                return settings != null &&
                       enableDepthTest == settings.enableDepthTest &&
                       depthMask == settings.depthMask &&
                       depthFunction == settings.depthFunction;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                var hashCode = -79323741;
                hashCode = hashCode * -1521134295 + enableDepthTest.GetHashCode();
                hashCode = hashCode * -1521134295 + depthMask.GetHashCode();
                hashCode = hashCode * -1521134295 + depthFunction.GetHashCode();
                return hashCode;
            }
        }
    }
}
