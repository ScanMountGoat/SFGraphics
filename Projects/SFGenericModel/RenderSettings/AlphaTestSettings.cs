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
            /// <summary>
            /// Enables or disables alpha testing.
            /// </summary>
            public bool enabled = false;

            /// <summary>
            /// The function used to determine if a fragment passes the alpha test.
            /// </summary>
            public AlphaFunction alphaFunction = AlphaFunction.Gequal;

            /// <summary>
            /// The comparision value used for <see cref="alphaFunction"/>.
            /// <c>1.0</c> is opaque. <c>0.0</c> is transparent.
            /// </summary>
            public float referenceAlpha = 0.5f;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                var settings = obj as AlphaTestSettings;
                return settings != null &&
                       enabled == settings.enabled &&
                       alphaFunction == settings.alphaFunction &&
                       referenceAlpha == settings.referenceAlpha;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                var hashCode = 907559639;
                hashCode = hashCode * -1521134295 + enabled.GetHashCode();
                hashCode = hashCode * -1521134295 + alphaFunction.GetHashCode();
                hashCode = hashCode * -1521134295 + referenceAlpha.GetHashCode();
                return hashCode;
            }
        }
    }
}
