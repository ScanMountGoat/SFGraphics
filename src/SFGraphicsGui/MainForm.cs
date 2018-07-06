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

namespace SFGraphicsGui
{
    public partial class MainForm : Form
    {
        private GraphicsResources graphicsResources;

        public MainForm()
        {
            InitializeComponent();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            glControl1.MakeCurrent();

            GL.Viewport(glControl1.ClientRectangle);

            DrawScreenTexture();

            glControl1.SwapBuffers();
        }

        private void DrawScreenTexture()
        {
            // Always check before using shaders.
            Shader shader = graphicsResources.screenTextureShader;
            if (!shader.ProgramCreatedSuccessfully())
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
            graphicsResources = new GraphicsResources();

            // Display compilation warnings.
            if (!graphicsResources.screenTextureShader.ProgramCreatedSuccessfully())
            {
                MessageBox.Show(graphicsResources.screenTextureShader.GetErrorLog(), "Failed Shader Compilation");
            }

            glControl1.Invalidate();
        }
    }
}
