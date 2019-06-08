using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    /// <summary>
    /// Contains methods for updating OpenGL rendering state.
    /// </summary>
    public static class GLRenderSettings
    {
        /// <summary>
        /// Updates the current OpenGL rendering state based on <paramref name="settings"/>.
        /// </summary>
        /// <param name="settings">The settings used to perform the update</param>
        public static void SetRenderSettings(RenderSettings settings)
        {
            SetPolygonModeSettings(settings.polygonModeSettings);
            SetFaceCulling(settings.faceCullingSettings);
            SetAlphaBlending(settings.alphaBlendSettings);
            SetAlphaTesting(settings.alphaTestSettings);
            SetDepthTesting(settings.depthTestSettings);
        }

        /// <summary>
        /// Updates the current OpenGL rendering state based on <paramref name="settings"/>.
        /// </summary>
        /// <param name="settings">The settings used to perform the update</param>
        public static void SetPolygonModeSettings(PolygonModeSettings settings)
        {
            GL.PolygonMode(settings.materialFace, settings.polygonMode);
        }

        /// <summary>
        /// Updates the current OpenGL rendering state based on <paramref name="settings"/>.
        /// </summary>
        /// <param name="settings">The settings used to perform the update</param>
        public static void SetFaceCulling(FaceCullingSettings settings)
        {
            SetGLEnableCap(EnableCap.CullFace, settings.enabled);

            GL.CullFace(settings.cullFaceMode);
        }

        /// <summary>
        /// Updates the current OpenGL rendering state based on <paramref name="settings"/>.
        /// </summary>
        /// <param name="settings">The settings used to perform the update</param>
        public static void SetDepthTesting(DepthTestSettings settings)
        {
            SetGLEnableCap(EnableCap.DepthTest, settings.enabled);

            GL.DepthFunc(settings.depthFunction);
            GL.DepthMask(settings.depthMask);
        }

        /// <summary>
        /// Updates the current OpenGL rendering state based on <paramref name="settings"/>.
        /// </summary>
        /// <param name="settings">The settings used to perform the update</param>
        public static void SetAlphaBlending(AlphaBlendSettings settings)
        {
            SetGLEnableCap(EnableCap.Blend, settings.enabled);

            GL.BlendFunc(settings.sourceFactor, settings.destinationFactor);
            GL.BlendEquationSeparate(settings.blendingEquationRgb, settings.blendingEquationAlpha);
        }

        /// <summary>
        /// Updates the current OpenGL rendering state based on <paramref name="settings"/>.
        /// </summary>
        /// <param name="settings">The settings used to perform the update</param>
        public static void SetAlphaTesting(AlphaTestSettings settings)
        {
            SetGLEnableCap(EnableCap.AlphaTest, settings.enabled);
            GL.AlphaFunc(settings.alphaFunction, settings.referenceAlpha);
        }

        private static void SetGLEnableCap(EnableCap enableCap, bool enabled)
        {
            if (enabled)
                GL.Enable(enableCap);
            else
                GL.Disable(enableCap);
        }
    }
}
