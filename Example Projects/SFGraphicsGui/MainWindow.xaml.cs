using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;
using SFGraphicsGui.Source;
using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace SFGraphicsGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GraphicsResources graphicsResources;

        private Texture2D textureToRender;
        private RenderMesh modelToRender;
        private int renderModeIndex;

        public MainWindow()
        {
            // The context isn't current yet, so don't call any OpenTK methods here.
            InitializeComponent();
            glViewport.VSync = false;
            glViewport.FrameRendering += RenderFrame;
            glViewport.HandleCreated += GlViewport_HandleCreated;
        }

        private void GlViewport_HandleCreated(object sender, EventArgs e)
        {
            // The context is created when the handle is created.
            SetUpRendering();
        }

        private void RenderFrame(object sender, EventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Draw a test pattern image to the screen.
            if (textureToRender != null)
            {
                graphicsResources.screenTriangle.DrawScreenTexture(textureToRender,
                    graphicsResources.screenTextureShader, graphicsResources.samplerObject);
            }

            if (modelToRender != null)
            {
                ModelRendering.DrawModel(modelToRender, glViewport.Width, glViewport.Height, graphicsResources, renderModeIndex);
            }

            SFGraphics.GLObjects.GLObjectManagement.GLObjectManager.DeleteUnusedGLObjects();
        }

        private void SetUpRendering()
        {
            // Pause the rendering thread to use the context on the current thread.
            glViewport.PauseRendering();

            graphicsResources = new GraphicsResources();

            // Display compilation warnings.
            if (!graphicsResources.screenTextureShader.LinkStatusIsOk)
            {
                MessageBox.Show(graphicsResources.screenTextureShader.GetErrorLog(),
                    "The texture shader did not link successfully.");
            }

            if (!graphicsResources.objModelShader.LinkStatusIsOk)
            {
                MessageBox.Show(graphicsResources.objModelShader.GetErrorLog(),
                    "The attribute shader did not link successfully.");
            }

            // Start rendering on the dedicated thread.
            glViewport.RestartRendering();
        }

        private void BackgroundNone_Click(object sender, RoutedEventArgs e)
        {
            textureToRender = null;
            glViewport.RenderFrame();
        }

        private void BackgroundUvTestPattern_Click(object sender, RoutedEventArgs e)
        {
            textureToRender = graphicsResources.uvTestPattern;
            glViewport.RenderFrame();
        }

        private void BackgroundMagentaBlackStripes_Click(object sender, RoutedEventArgs e)
        {
            textureToRender = graphicsResources.floatMagentaBlackStripes;
            glViewport.RenderFrame();
        }

        private async void FileOpen_OnClick(object sender, RoutedEventArgs e)
        {
            glViewport.PauseRendering();
            var dialog = new OpenFileDialog {Filter = "Model Formats|*.obj;*.dae"};
            {
                if (dialog.ShowDialog() == true)
                {
                    if (dialog.FileName.ToLower().EndsWith(".dae"))
                    {
                        var vertices = await ColladaToRenderMesh.GetVerticesAsync(dialog.FileName);
                        modelToRender = new RenderMesh(vertices);
                    }
                    else if (dialog.FileName.ToLower().EndsWith(".obj"))
                    {
                        modelToRender = WavefrontToRenderMesh.CreateRenderMesh(dialog.FileName);
                    }
                }
            }
            glViewport.RestartRendering();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Users tend to complain when you leave threads running...
            glViewport.Dispose();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Map the number keys to render modes 1 to 10 inclusive. 
            switch (e.Key)
            {
                case Key.D1:
                    renderModeIndex = 0;
                    break;
                case Key.D2:
                    renderModeIndex = 1;
                    break;
                case Key.D3:
                    renderModeIndex = 2;
                    break;
                case Key.D4:
                    renderModeIndex = 3;
                    break;
                case Key.D5:
                    renderModeIndex = 4;
                    break;
                case Key.D6:
                    renderModeIndex = 5;
                    break;
                case Key.D7:
                    renderModeIndex = 6;
                    break;
                case Key.D8:
                    renderModeIndex = 7;
                    break;
                case Key.D9:
                    renderModeIndex = 8;
                    break;
                case Key.D0:
                    renderModeIndex = 10;
                    break;
            }      
        }
    }
}
