using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    /// <summary>
    /// The alpha blending state set before drawing a <see cref="GenericMesh{T}"/>.
    /// </summary>
    public struct AlphaBlendSettings
    {
        /// <summary>
        /// The default alpha blend settings.
        /// </summary>
        public static AlphaBlendSettings Default = new AlphaBlendSettings(true, BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha, 
            BlendEquationMode.FuncAdd, BlendEquationMode.FuncAdd);

        /// <summary>
        /// Enables or disables alpha blending.
        /// </summary>
        public bool enabled;

        /// <summary>
        /// The source color is multiplied by <see cref="sourceFactor"/>.
        /// </summary>
        public BlendingFactor sourceFactor;

        /// <summary>
        /// The destination color is multiplied by <see cref="destinationFactor"/>.
        /// </summary>
        public BlendingFactor destinationFactor;

        /// <summary>
        /// The blending operation used for the RGB components.
        /// </summary>
        public BlendEquationMode blendingEquationRgb;

        /// <summary>
        /// The blending operation used for only the alpha component.
        /// </summary>
        public BlendEquationMode blendingEquationAlpha;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="sourceFactor"></param>
        /// <param name="destinationFactor"></param>
        /// <param name="blendingEquationRgb"></param>
        /// <param name="blendingEquationAlpha"></param>
        public AlphaBlendSettings(bool enabled, BlendingFactor sourceFactor, BlendingFactor destinationFactor, 
            BlendEquationMode blendingEquationRgb, BlendEquationMode blendingEquationAlpha)
        {
            this.enabled = enabled;
            this.sourceFactor = sourceFactor;
            this.destinationFactor = destinationFactor;
            this.blendingEquationRgb = blendingEquationRgb;
            this.blendingEquationAlpha = blendingEquationAlpha;
        }

        /// <summary>
        /// Compares <paramref name="lhs"/> and <paramref name="rhs"/>
        /// using <see cref="Equals(object)"/>.
        /// </summary>
        /// <param name="lhs">The left object to compare</param>
        /// <param name="rhs">The right object to compare</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> and <paramref name="rhs"/> are equal</returns>
        public static bool operator ==(AlphaBlendSettings lhs, AlphaBlendSettings rhs)
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
        public static bool operator !=(AlphaBlendSettings lhs, AlphaBlendSettings rhs)
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
            if (!(obj is AlphaBlendSettings))
            {
                return false;
            }

            var settings = (AlphaBlendSettings)obj;
            return enabled == settings.enabled &&
                   sourceFactor == settings.sourceFactor &&
                   destinationFactor == settings.destinationFactor &&
                   blendingEquationRgb == settings.blendingEquationRgb &&
                   blendingEquationAlpha == settings.blendingEquationAlpha;
        }

        /// <summary>
        /// Returns a hash code based on the object's fields.
        /// </summary>
        /// <returns>A hash code for the current object</returns>
        public override int GetHashCode()
        {
            var hashCode = 996220672;
            hashCode = hashCode * -1521134295 + enabled.GetHashCode();
            hashCode = hashCode * -1521134295 + sourceFactor.GetHashCode();
            hashCode = hashCode * -1521134295 + destinationFactor.GetHashCode();
            hashCode = hashCode * -1521134295 + blendingEquationRgb.GetHashCode();
            hashCode = hashCode * -1521134295 + blendingEquationAlpha.GetHashCode();
            return hashCode;
        }
    }
}
