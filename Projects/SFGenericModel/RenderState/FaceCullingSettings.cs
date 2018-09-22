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

        public override int GetHashCode()
        {
            var hashCode = 96933389;
            hashCode = hashCode * -1521134295 + enabled.GetHashCode();
            hashCode = hashCode * -1521134295 + cullFaceMode.GetHashCode();
            return hashCode;
        }
    }
}
