using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    /// <summary>
    /// The face culling state set before drawing a <see cref="GenericMesh{T}"/>.
    /// </summary>
    public struct FaceCullingSettings
    {
        /// <summary>
        /// The default face culling settings.
        /// </summary>
        public static FaceCullingSettings Default = new FaceCullingSettings(true, CullFaceMode.Back);

        /// <summary>
        /// Enables or disables face culling.
        /// </summary>
        public readonly bool enabled;

        /// <summary>
        /// Determines whether back and/or front faces will be culled.
        /// </summary>
        public readonly CullFaceMode cullFaceMode;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="cullFaceMode"></param>
        public FaceCullingSettings(bool enabled, CullFaceMode cullFaceMode)
        {
            this.enabled = enabled;
            this.cullFaceMode = cullFaceMode;
        }

        /// <summary>
        /// Compares <paramref name="lhs"/> and <paramref name="rhs"/>
        /// using <see cref="Equals(object)"/>.
        /// </summary>
        /// <param name="lhs">The left object to compare</param>
        /// <param name="rhs">The right object to compare</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> and <paramref name="rhs"/> are equal</returns>
        public static bool operator ==(FaceCullingSettings lhs, FaceCullingSettings rhs)
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
        public static bool operator !=(FaceCullingSettings lhs, FaceCullingSettings rhs)
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
            if (!(obj is FaceCullingSettings))
            {
                return false;
            }

            var settings = (FaceCullingSettings)obj;
            return enabled == settings.enabled &&
                   cullFaceMode == settings.cullFaceMode;
        }

        /// <summary>
        /// Returns a hash code based on the object's fields.
        /// </summary>
        /// <returns>A hash code for the current object</returns>
        public override int GetHashCode()
        {
            var hashCode = 96933389;
            hashCode = hashCode * -1521134295 + enabled.GetHashCode();
            hashCode = hashCode * -1521134295 + cullFaceMode.GetHashCode();
            return hashCode;
        }
    }
}
