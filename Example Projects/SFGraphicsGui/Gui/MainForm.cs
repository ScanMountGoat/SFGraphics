using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.Cameras;
using SFGraphics.GLObjects.Textures;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ColladaSharp;

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
        private RenderMesh modelToRender;
        private int renderModeIndex = 0;

        public MainForm()
        {
            // The context isn't current yet, so don't call any OpenTK methods here.
            InitializeComponent();
            glViewport.VSync = true;
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
                DrawModel();
            }

            SFGraphics.GLObjects.GLObjectManagement.GLObjectManager.DeleteUnusedGLObjects();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            if (OpenTK.Graphics.GraphicsContext.CurrentContext != null)
                SetUpRendering();
            else
                MessageBox.Show("Context Creation Failed");
        }

        private void SetUpRendering()
        {
            graphicsResources = new GraphicsResources();

            // Display compilation warnings.
            if (!graphicsResources.screenTextureShader.LinkStatusIsOk)
            {
                MessageBox.Show(graphicsResources.screenTextureShader.GetErrorLog(), "The texture shader did not link successfully.");
            }

            // Display compilation warnings.
            if (!graphicsResources.objModelShader.LinkStatusIsOk)
            {
                MessageBox.Show(graphicsResources.objModelShader.GetErrorLog(), "The attribute shader did not link successfully.");
            }

            // Trigger the render event.
            glViewport.RenderFrame();

            glViewport.ResumeRendering();
        }

        private void uvTestPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (graphicsResources != null)
            {
                textureToRender = graphicsResources.uvTestPattern;
                glViewport.RenderFrame();
            }
        }

        private void magentaBlackStripesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (graphicsResources != null)
            {
                textureToRender = graphicsResources.floatMagentaBlackStripes;
                glViewport.RenderFrame();
            }
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            glViewport.RenderFrame();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog() { Filter = "Model Formats|*.obj;*.dae" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    glViewport.PauseRendering();

                    if (dialog.FileName.ToLower().EndsWith(".dae"))
                        InitializeMeshFromCollada(dialog.FileName);
                    else if (dialog.FileName.ToLower().EndsWith(".obj"))
                        InitializeMeshFromObj(dialog);
                    else
                        return;

                    glViewport.RenderFrame();
                    glViewport.ResumeRendering();
                }
            }
        }

        private void InitializeMeshFromObj(OpenFileDialog dialog)
        {
            var vertices = GetVertices(FileFormatWavefront.FileFormatObj.Load(dialog.FileName, false));
            modelToRender = new RenderMesh(vertices);
        }

        private async void InitializeMeshFromCollada(string filename)
        {
            var result = await Collada.ImportAsync(filename, new ColladaImportOptions(),
                new Progress<float>(), CancellationToken.None);

            var vertices = new List<RenderVertex>();
            foreach (var scene in result.Scenes)
            {
                foreach (var subMesh in scene.Model.Children)
                {
                    for (int i = 0; i < subMesh.Primitives.Triangles.Count; i++)
                    {
                        var face = subMesh.Primitives.GetFace(i);
                        vertices.Add(GetVertex(face.Vertex0));
                        vertices.Add(GetVertex(face.Vertex1));
                        vertices.Add(GetVertex(face.Vertex2));
                    }
                }
            }

            modelToRender = new RenderMesh(vertices);
        }

        private static RenderVertex GetVertex(ColladaSharp.Models.Vertex vertex)
        {
            // TODO: Orientation?
            var position = new Vector4(vertex.Position.X, vertex.Position.Y, vertex.Position.Z, 1) * Matrix4.CreateRotationX(90);
            var normal = new Vector4(vertex.Normal.X, vertex.Normal.Y, vertex.Normal.Z, 1) * Matrix4.CreateRotationX(90);
            var texCoord = new Vector2(vertex.TexCoord.X, vertex.TexCoord.Y);
            return new RenderVertex(position.Xyz, normal.Xyz, texCoord);
        }

        private void DrawModel()
        {
            if (!graphicsResources.objModelShader.LinkStatusIsOk)
                return;

            var camera = new Camera()
            {
                RenderWidth = glViewport.Width,
                RenderHeight = glViewport.Height,
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

        private static List<RenderVertex> GetVertices(FileFormatWavefront.FileLoadResult<FileFormatWavefront.Model.Scene> result)
        {
            // TODO: Groups?
            var vertices = new List<RenderVertex>(result.Model.UngroupedFaces.Count * 3);

            foreach (var face in result.Model.UngroupedFaces)
            {
                foreach (var index in face.Indices)
                {
                    AddVertex(result, vertices, index);
                }
            }

            return vertices;
        }

        private static void AddVertex(FileFormatWavefront.FileLoadResult<FileFormatWavefront.Model.Scene> result, List<RenderVertex> vertices, FileFormatWavefront.Model.Index index)
        {
            var position = new Vector3(result.Model.Vertices[index.vertex].x, result.Model.Vertices[index.vertex].y, result.Model.Vertices[index.vertex].z);

            var normal = Vector3.Zero;
            if (index.normal != null)
                normal = new Vector3(result.Model.Normals[(int)index.normal].x, result.Model.Normals[(int)index.normal].y, result.Model.Normals[(int)index.normal].z);

            var texcoord = Vector2.Zero;
            if (index.uv != null)
                texcoord = new Vector2(result.Model.Uvs[(int)index.uv].u, result.Model.Uvs[(int)index.uv].v);

            vertices.Add(new RenderVertex(position, normal, texcoord));
        }

        private void glControl1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                // Convert 1 to 9 to 0 to 8.
                renderModeIndex = Math.Max(int.Parse(e.KeyChar.ToString()) - 1, 0);
                glViewport.RenderFrame();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Users tend to complain when you leave threads running...
            glViewport.Dispose();
        }
    }
}
