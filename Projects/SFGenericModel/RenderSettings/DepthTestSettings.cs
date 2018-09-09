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
            /// Enables or disables depth testing.
            /// </summary>
            public bool enabled = true;

            /// <summary>
            /// Enables writes to the depth buffer when true.
            /// </summary>
            public bool depthMask = true;

            /// <summary>
            /// The function used to determine if a fragment passes the depth test.
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
                       enabled == settings.enabled &&
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
                hashCode = hashCode * -1521134295 + enabled.GetHashCode();
                hashCode = hashCode * -1521134295 + depthMask.GetHashCode();
                hashCode = hashCode * -1521134295 + depthFunction.GetHashCode();
                return hashCode;
            }
        }
    }
}
