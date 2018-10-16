using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    /// <summary>
    /// The polygon mode settings set before drawing.
    /// </summary>
    public struct PolygonModeSettings
    {
        /// <summary>
        /// The default polygon mode settings.
        /// </summary>
        public static PolygonModeSettings Default = new PolygonModeSettings(MaterialFace.FrontAndBack, PolygonMode.Fill);

        /// <summary>
        /// Determines what faces are effected.
        /// </summary>
        public readonly MaterialFace materialFace;

        /// <summary>
        /// Determines how primitives are drawn.
        /// </summary>
        public readonly PolygonMode polygonMode;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="materialFace"></param>
        /// <param name="polygonMode"></param>
        public PolygonModeSettings(MaterialFace materialFace, PolygonMode polygonMode)
        {
            this.materialFace = materialFace;
            this.polygonMode = polygonMode;
        }

        /// <summary>
        /// Compares <paramref name="lhs"/> and <paramref name="rhs"/>
        /// using <see cref="Equals(object)"/>.
        /// </summary>
        /// <param name="lhs">The left object to compare</param>
        /// <param name="rhs">The right object to compare</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> and <paramref name="rhs"/> are equal</returns>
        public static bool operator == (PolygonModeSettings lhs, PolygonModeSettings rhs)
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
        public static bool operator != (PolygonModeSettings lhs, PolygonModeSettings rhs)
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
            if (!(obj is PolygonModeSettings))
            {
                return false;
            }

            var settings = (PolygonModeSettings)obj;
            return materialFace == settings.materialFace &&
                   polygonMode == settings.polygonMode;
        }

        /// <summary>
        /// Returns a hash code based on the object's fields.
        /// </summary>
        /// <returns>A hash code for the current object</returns>
        public override int GetHashCode()
        {
            var hashCode = -2139604081;
            hashCode = hashCode * -1521134295 + materialFace.GetHashCode();
            hashCode = hashCode * -1521134295 + polygonMode.GetHashCode();
            return hashCode;
        }
    }
}
