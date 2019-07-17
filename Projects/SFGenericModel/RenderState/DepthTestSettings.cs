using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    /// <summary>
    /// The alpha blending state set before drawing a <see cref="GenericMesh{T}"/>.
    /// </summary>
    public struct DepthTestSettings
    {
        /// <summary>
        /// The default depth test settings.
        /// </summary>
        public static DepthTestSettings Default = new DepthTestSettings(true, true, DepthFunction.Lequal);

        /// <summary>
        /// Enables or disables depth testing.
        /// </summary>
        public readonly bool enabled;

        /// <summary>
        /// Enables writes to the depth buffer when true.
        /// </summary>
        public readonly bool depthMask;

        /// <summary>
        /// The function used to determine if a fragment passes the depth test.
        /// </summary>
        public readonly DepthFunction depthFunction;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="depthMask"></param>
        /// <param name="depthFunction"></param>
        public DepthTestSettings(bool enabled, bool depthMask, DepthFunction depthFunction)
        {
            this.enabled = enabled;
            this.depthMask = depthMask;
            this.depthFunction = depthFunction;
        }

        /// <summary>
        /// Compares <paramref name="lhs"/> and <paramref name="rhs"/>
        /// using <see cref="Equals(object)"/>.
        /// </summary>
        /// <param name="lhs">The left object to compare</param>
        /// <param name="rhs">The right object to compare</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> and <paramref name="rhs"/> are equal</returns>
        public static bool operator ==(DepthTestSettings lhs, DepthTestSettings rhs)
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
        public static bool operator !=(DepthTestSettings lhs, DepthTestSettings rhs)
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
            if (!(obj is DepthTestSettings))
            {
                return false;
            }

            var settings = (DepthTestSettings)obj;
            return enabled == settings.enabled &&
                   depthMask == settings.depthMask &&
                   depthFunction == settings.depthFunction;
        }

        /// <summary>
        /// Returns a hash code based on the object's fields.
        /// </summary>
        /// <returns>A hash code for the current object</returns>
        public override int GetHashCode()
        {
            var hashCode = 1286869374;
            hashCode = hashCode * -1521134295 + enabled.GetHashCode();
            hashCode = hashCode * -1521134295 + depthMask.GetHashCode();
            hashCode = hashCode * -1521134295 + depthFunction.GetHashCode();
            return hashCode;
        }
    }
}
