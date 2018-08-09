using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects;
using SFGraphics.Tools;

namespace SFGraphicsGui
{
    /// <summary>
    /// A short example of how to use SFGraphics and OpenTK to render a texture.
    /// This class also shows how to check for common errors to avoid the difficult to debug 
    /// <see cref="System.AccessViolationException"/>.
    /// </summary>
    public partial class MainForm : Form
    {
        private GraphicsResources graphicsResources;

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
            DrawScreenTexture();
            glControl1.SwapBuffers();

            // Clean up any unused resources.
            GLObjectManager.DeleteUnusedGLObjects();
        }

        private void DrawScreenTexture()
        {
            // Always check program creation before using shaders to prevent crashes.
            Shader shader = graphicsResources.screenTextureShader;
            if (!shader.ProgramCreatedSuccessfully)
                return;

            // Render using the shader.
            GL.UseProgram(shader.Id);

            shader.SetTexture("uvTexture", graphicsResources.uvTestPattern.Id, TextureTarget.Texture2D, 0);

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
    }
}
