using OpenTK.Graphics.OpenGL;
using SFGraphics.Cameras;

namespace SFGraphicsGui.Source
{
    internal static class ModelRendering
    {
        public static void DrawModel(RenderMesh modelToRender, int width, int height, GraphicsResources graphicsResources, int renderModeIndex)
        {
            if (!graphicsResources.objModelShader.LinkStatusIsOk)
                return;

            var camera = new Camera()
            {
                RenderWidth = width,
                RenderHeight = height,
                NearClipPlane = 0.01f,
            };
            camera.FrameBoundingSphere(modelToRender.BoundingSphere);

            graphicsResources.objModelShader.UseProgram();
            graphicsResources.objModelShader.SetMatrix4x4("mvpMatrix", camera.MvpMatrix);
            graphicsResources.objModelShader.SetInt("attributeIndex", renderModeIndex);

            GL.Clear(ClearBufferMask.DepthBufferBit);

            SFGenericModel.RenderState.GLRenderSettings.SetFaceCulling(new SFGenericModel.RenderState.FaceCullingSettings(true, CullFaceMode.Back));
            SFGenericModel.RenderState.GLRenderSettings.SetDepthTesting(new SFGenericModel.RenderState.DepthTestSettings(true, true, DepthFunction.Lequal));

            modelToRender.Draw(graphicsResources.objModelShader);
        }
    }
}
