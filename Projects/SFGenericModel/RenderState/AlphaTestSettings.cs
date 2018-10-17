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

        /// <summary>
        /// Compares <paramref name="lhs"/> and <paramref name="rhs"/>
        /// using <see cref="Equals(object)"/>.
        /// </summary>
        /// <param name="lhs">The left object to compare</param>
        /// <param name="rhs">The right object to compare</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> and <paramref name="rhs"/> are equal</returns>
        public static bool operator ==(AlphaTestSettings lhs, AlphaTestSettings rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Compares <paramref name="lhs"/> and <paramref name="rhs"/>
        /// using <see cref="Equals(object)"/>.
        /// </summary>
        /// <param name="lhs">The left object to compare</param>
        /// <param name="rhs">The right object to compare</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> and <paramref name="rhs"/> are not equal</returns>
        public static bool operator !=(AlphaTestSettings lhs, AlphaTestSettings rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <summary>
        /// Compares the values of the object's fields with <paramref name="obj"/>
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        /// <returns><c>true</c> if the specified object is equal to the current object</returns>
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

        /// <summary>
        /// Returns a hash code based on the object's fields.
        /// </summary>
        /// <returns>A hash code for the current object</returns>
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
