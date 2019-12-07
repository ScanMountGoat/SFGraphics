using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;
using SFGraphicsGui.Source;
using System;
using System.Windows.Forms;
using KeyPressEventArgs = System.Windows.Forms.KeyPressEventArgs;

namespace SFGraphicsGui
{
    public partial class MainForm : Form
    {
        private GraphicsResources graphicsResources;

        private Texture2D textureToRender;
        private RenderMesh modelToRender;
        private int renderModeIndex;

        public MainForm()
        {
            // The context isn't current yet, so don't call any OpenTK methods here.
            InitializeComponent();
            glViewport.VSync = false;
            glViewport.OnRenderFrame += RenderFrame;
        }

        private void RenderFrame(object sender, EventArgs e)
        {
            // Context creation and resource creation failed, so we can't render anything.
            if (graphicsResources == null)
                return;

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

        private void glControl1_Load(object sender, EventArgs e)
        {
            SetUpRendering();
        }

        private void SetUpRendering()
        {
            // Pause the rendering thread to use the context on the current thread.
            glViewport.PauseRendering();

            graphicsResources = new GraphicsResources();

            // Display compilation warnings.
            if (!graphicsResources.screenTextureShader.LinkStatusIsOk)
            {
                MessageBox.Show(graphicsResources.screenTextureShader.GetErrorLog(), "The texture shader did not link successfully.");
            }

            if (!graphicsResources.objModelShader.LinkStatusIsOk)
            {
                MessageBox.Show(graphicsResources.objModelShader.GetErrorLog(), "The attribute shader did not link successfully.");
            }

            // Start rendering on the dedicated thread.
            glViewport.RestartRendering();
        }

        private void uvTestPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textureToRender = graphicsResources.uvTestPattern;
            glViewport.RenderFrame();
        }

        private void magentaBlackStripesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textureToRender = graphicsResources.floatMagentaBlackStripes;
            glViewport.RenderFrame();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {

        }

        private async void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            glViewport.PauseRendering();
            using (var dialog = new OpenFileDialog { Filter = "Model Formats|*.obj;*.dae" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
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

        private void glControl1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                // Convert 1 to 9 to 0 to 8.
                renderModeIndex = Math.Max(int.Parse(e.KeyChar.ToString()) - 1, 0);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Users tend to complain when you leave threads running...
            glViewport.Dispose();
        }
    }
}
