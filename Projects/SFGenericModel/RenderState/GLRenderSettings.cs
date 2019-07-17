using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    /// <summary>
    /// Contains methods for updating OpenGL rendering state.
    /// </summary>
    public static class GLRenderSettings
    {
        /// <summary>
        /// Updates the current OpenGL rendering state. 
        /// To improve performance, use <see cref="SetRenderSettings(RenderSettings, RenderSettings)"/>.
        /// </summary>
        /// <param name="settings">The settings used to perform the update</param>
        public static void SetRenderSettings(RenderSettings settings)
        {
            // Without knowing the previous value, every GL function must be called.
            SetPolygonModeSettings(settings.polygonModeSettings);
            SetFaceCulling(settings.faceCullingSettings);
            SetAlphaBlending(settings.alphaBlendSettings);
            SetAlphaTesting(settings.alphaTestSettings);
            SetDepthTesting(settings.depthTestSettings);
        }

        /// <summary>
        /// Updates the current OpenGL rendering state. 
        /// To improve performance, no OpenGL functions are called for identical values for
        /// <paramref name="settings"/> and <paramref name="previousSettings"/>.
        /// </summary>
        /// <param name="settings">The settings used to perform the update</param>
        /// <param name="previousSettings">The settings used for the previous update</param>
        public static void SetRenderSettings(RenderSettings settings, RenderSettings previousSettings)
        {
            // Comparing render state can avoid redundant calls to costly GL functions.
            SetPolygonModeSettings(settings.polygonModeSettings, previousSettings.polygonModeSettings);
            SetFaceCulling(settings.faceCullingSettings, previousSettings.faceCullingSettings);
            SetAlphaBlending(settings.alphaBlendSettings, previousSettings.alphaBlendSettings);
            SetAlphaTesting(settings.alphaTestSettings, previousSettings.alphaTestSettings);
            SetDepthTesting(settings.depthTestSettings, previousSettings.depthTestSettings);
        }

        /// <summary>
        /// Updates the current OpenGL rendering state based on <paramref name="settings"/>.
        /// </summary>
        /// <param name="settings">The settings used to perform the update</param>
        /// <param name="previousSettings">The settings used for the previous update</param>
        public static void SetPolygonModeSettings(PolygonModeSettings settings, PolygonModeSettings previousSettings)
        {
            if (settings.materialFace != previousSettings.materialFace || settings.polygonMode != previousSettings.polygonMode)
                GL.PolygonMode(settings.materialFace, settings.polygonMode);
        }

        /// <summary>
        /// Updates the current OpenGL rendering state based on <paramref name="settings"/>.
        /// </summary>
        /// <param name="settings">The settings used to perform the update</param>
        /// <param name="previousSettings">The settings used for the previous update</param>
        public static void SetFaceCulling(FaceCullingSettings settings, FaceCullingSettings previousSettings)
        {
            if (settings.enabled != previousSettings.enabled)
                SetGLEnableCap(EnableCap.CullFace, settings.enabled);

            if (settings.cullFaceMode != previousSettings.cullFaceMode)
                GL.CullFace(settings.cullFaceMode);
        }

        /// <summary>
        /// Updates the current OpenGL rendering state based on <paramref name="settings"/>.
        /// </summary>
        /// <param name="settings">The settings used to perform the update</param>
        /// <param name="previousSettings">The settings used for the previous update</param>
        public static void SetDepthTesting(DepthTestSettings settings, DepthTestSettings previousSettings)
        {
            if (settings.enabled != previousSettings.enabled)
                SetGLEnableCap(EnableCap.DepthTest, settings.enabled);

            if (settings.depthFunction != previousSettings.depthFunction)
                GL.DepthFunc(settings.depthFunction);

            if (settings.depthMask != previousSettings.depthMask)
                GL.DepthMask(settings.depthMask);
        }

        /// <summary>
        /// Updates the current OpenGL rendering state based on <paramref name="settings"/>.
        /// </summary>
        /// <param name="settings">The settings used to perform the update</param>
        /// <param name="previousSettings">The settings used for the previous update</param>
        public static void SetAlphaBlending(AlphaBlendSettings settings, AlphaBlendSettings previousSettings)
        {
            if (settings.enabled != previousSettings.enabled)
                SetGLEnableCap(EnableCap.Blend, settings.enabled);

            if (settings.sourceFactor != previousSettings.sourceFactor || settings.destinationFactor != previousSettings.destinationFactor)
                GL.BlendFunc(settings.sourceFactor, settings.destinationFactor);

            if (settings.blendingEquationRgb != previousSettings.blendingEquationRgb || settings.blendingEquationAlpha != previousSettings.blendingEquationAlpha)
                GL.BlendEquationSeparate(settings.blendingEquationRgb, settings.blendingEquationAlpha);
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
        /// <param name="previousSettings">The settings used for the previous update</param>
        public static void SetAlphaTesting(AlphaTestSettings settings, AlphaTestSettings previousSettings)
        {
            SetGLEnableCap(EnableCap.AlphaTest, settings.enabled);
            GL.AlphaFunc(settings.alphaFunction, settings.referenceAlpha);
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
