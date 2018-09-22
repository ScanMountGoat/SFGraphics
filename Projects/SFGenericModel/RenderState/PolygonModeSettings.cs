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

        public override int GetHashCode()
        {
            var hashCode = -2139604081;
            hashCode = hashCode * -1521134295 + materialFace.GetHashCode();
            hashCode = hashCode * -1521134295 + polygonMode.GetHashCode();
            return hashCode;
        }
    }
}
