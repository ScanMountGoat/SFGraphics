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
        /// The comparison value used for <see cref="alphaFunction"/>.
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

        public override bool Equals(object obj)
        {
            if (!(obj is AlphaTestSettings))
            {
                return false;
            }

            var settings = (AlphaTestSettings)obj;
            return enabled == settings.enabled &&
                   alphaFunction == settings.alphaFunction &&
                   referenceAlpha == settings.referenceAlpha;
        }

        public override int GetHashCode()
        {
            var hashCode = 1022993667;
            hashCode = hashCode * -1521134295 + enabled.GetHashCode();
            hashCode = hashCode * -1521134295 + alphaFunction.GetHashCode();
            hashCode = hashCode * -1521134295 + referenceAlpha.GetHashCode();
            return hashCode;
        }
    }
}
