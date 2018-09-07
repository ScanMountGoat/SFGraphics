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
            /// 
            /// </summary>
            public bool enableAlphaTesting = false;

            /// <summary>
            /// 
            /// </summary>
            public AlphaFunction alphaFunction = AlphaFunction.Gequal;

            /// <summary>
            /// 
            /// </summary>
            public int referenceAlpha = 128;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                var settings = obj as AlphaTestSettings;
                return settings != null &&
                       enableAlphaTesting == settings.enableAlphaTesting &&
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
                hashCode = hashCode * -1521134295 + enableAlphaTesting.GetHashCode();
                hashCode = hashCode * -1521134295 + alphaFunction.GetHashCode();
                hashCode = hashCode * -1521134295 + referenceAlpha.GetHashCode();
                return hashCode;
            }
        }
    }
}
