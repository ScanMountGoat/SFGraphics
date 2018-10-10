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
        public DepthFunction depthFunction;

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
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <returns></returns>
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
