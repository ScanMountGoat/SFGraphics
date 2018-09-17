using OpenTK.Graphics.OpenGL;
using SFGenericModel.RenderState;

namespace SFGenericModel.Utils
{
    /// <summary>
    /// Contains methods for updating OpenGL rendering state.
    /// </summary>
    public static class GLRenderSettings
    {
        /// <summary>
        /// Updates the current OpenGL rendering state based on render settings
        /// </summary>
        /// <param name="renderSettings">The settings used to perform the update</param>
        public static void SetRenderSettings(RenderSettings renderSettings)
        {
            SetFaceCulling(renderSettings.faceCullingSettings);
            SetAlphaBlending(renderSettings.alphaBlendSettings);
            SetAlphaTesting(renderSettings.alphaTestSettings);
        }

        private static void SetFaceCulling(RenderSettings.FaceCullingSettings settings)
        {
            SetGLEnableCap(EnableCap.CullFace, settings.enabled);

            GL.CullFace(settings.cullFaceMode);
        }

        private static void SetDepthTesting(RenderSettings.DepthTestSettings settings)
        {
            SetGLEnableCap(EnableCap.DepthTest, settings.enabled);

            GL.DepthFunc(settings.depthFunction);
            GL.DepthMask(settings.depthMask);
        }

        private static void SetAlphaBlending(RenderSettings.AlphaBlendSettings settings)
        {
            SetGLEnableCap(EnableCap.Blend, settings.enabled);

            GL.BlendFunc(settings.sourceFactor, settings.destinationFactor);
            GL.BlendEquationSeparate(settings.blendingEquationRgb, settings.blendingEquationAlpha);
        }

        private static void SetAlphaTesting(RenderSettings.AlphaTestSettings settings)
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
