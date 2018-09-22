using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    /// <summary>
    /// The alpha testing state set before drawing a <see cref="GenericMesh{T}"/>.
    /// </summary>
    public struct AlphaTestSettings
    {
        /// <summary>
        /// The default alpha test settings.
        /// </summary>
        public static AlphaTestSettings Default = new AlphaTestSettings(false, AlphaFunction.Gequal, 0.5f);

        /// <summary>
        /// Enables or disables alpha testing.
        /// </summary>
        public readonly bool enabled;

        /// <summary>
        /// The function used to determine if a fragment passes the alpha test.
        /// </summary>
        public readonly AlphaFunction alphaFunction;

        /// <summary>
        /// The comparision value used for <see cref="alphaFunction"/>.
        /// <c>1.0</c> is opaque. <c>0.0</c> is transparent.
        /// </summary>
        public readonly float referenceAlpha;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="alphaFunction"></param>
        /// <param name="referenceAlpha"></param>
        public AlphaTestSettings(bool enabled, AlphaFunction alphaFunction, float referenceAlpha)
        {
            this.enabled = enabled;
            this.alphaFunction = alphaFunction;
            this.referenceAlpha = referenceAlpha;
        }
    }
}
