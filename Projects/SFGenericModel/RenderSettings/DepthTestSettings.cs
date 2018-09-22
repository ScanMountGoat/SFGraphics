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
    }
}
