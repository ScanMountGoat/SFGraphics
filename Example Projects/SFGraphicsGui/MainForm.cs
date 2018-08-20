using System;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.GLObjectManagement;

namespace SFGraphicsGui
{
    /// <summary>
    /// A short example of how to use SFGraphics and OpenTK to render a texture.
    /// This class also shows how to check for common errors to avoid the difficult to debug 
    /// <see cref="AccessViolationException"/>.
    /// </summary>
    public partial class MainForm : Form
    {
        private GraphicsResources graphicsResources;
        private Texture2D textureToRender;

        public MainForm()
        {
            // The context isn't current yet, so don't call any OpenTK methods here.
            InitializeComponent();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            // Context creation and resource creation failed, so we can't render anything.
            if (graphicsResources == null)
                return;

            // Set up the viewport.
            glControl1.MakeCurrent();
            GL.Viewport(glControl1.ClientRectangle);

            // Draw a test pattern image to the screen.
            DrawScreenTexture(textureToRender);
            glControl1.SwapBuffers();

            // Clean up any unused resources.
            GLObjectManager.DeleteUnusedGLObjects();
        }

        private void DrawScreenTexture(Texture2D texture)
        {
            if (texture == null)
                return;

            // Always check program creation before using shaders to prevent crashes.
            Shader shader = graphicsResources.screenTextureShader;
            if (!shader.ProgramCreatedSuccessfully)
                return;

            // Render using the shader.
            GL.UseProgram(shader.Id);

            // The sampler's parameters are used instead of the texture's parameters.
            int textureUnit = 0;
            graphicsResources.samplerObject.Bind(textureUnit);
            shader.SetTexture("uvTexture", texture.Id, texture.TextureTarget, textureUnit);

            shader.EnableVertexAttributes();
            graphicsResources.screenTriangleVbo.Bind();

            GL.VertexAttribPointer(shader.GetVertexAttributeUniformLocation("position"), 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            // Forgetting to disable vertex attributes can cause crashes on some drivers.
            shader.DisableVertexAttributes();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            if (OpenTK.Graphics.GraphicsContext.CurrentContext != null)
            {
                SetUpRendering();
            }
            else
            {
                MessageBox.Show("Context Creation Failed");
            }
        }

        private void SetUpRendering()
        {
            graphicsResources = new GraphicsResources();

            // Display compilation warnings.
            if (!graphicsResources.screenTextureShader.ProgramCreatedSuccessfully)
            {
                MessageBox.Show(graphicsResources.screenTextureShader.GetErrorLog(), "Failed Shader Compilation");
            }

            // Trigger the render event.
            glControl1.Invalidate();
        }

        private void uVTestPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (graphicsResources != null)
            {
                textureToRender = graphicsResources.uvTestPattern;
                glControl1.Invalidate();
            }
        }

        private void magentaBlackStripesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (graphicsResources != null)
            {
                textureToRender = graphicsResources.floatMagentaBlackStripes;
                glControl1.Invalidate();
            }
        }
    }
}
