using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGenericModel.ShaderGenerators;
using SFGraphics.Cameras;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Textures;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SFGenericModel.VertexAttributes;

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
        private ObjMesh modelToRender;

        public MainForm()
        {
            // The context isn't current yet, so don't call any OpenTK methods here.
            InitializeComponent();
            glControl1.OnRenderFrame += RenderFrame;
        }

        private void RenderFrame(object sender, EventArgs e)
        {
            // Context creation and resource creation failed, so we can't render anything.
            if (graphicsResources == null)
                return;

            // Draw a test pattern image to the screen.
            if (textureToRender != null)
            {
                graphicsResources.screenTriangle.DrawScreenTexture(textureToRender, graphicsResources.lutTexture,
                    graphicsResources.screenTextureShader, graphicsResources.samplerObject);
            }

            if (modelToRender != null)
            {
                DrawModel();
            }
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
                MessageBox.Show(graphicsResources.screenTextureShader.GetErrorLog(), "Failed Shader Compilation");
            }

            // Trigger the render event.
            glControl1.RenderFrame();
        }

        private void uvTestPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (graphicsResources != null)
            {
                textureToRender = graphicsResources.uvTestPattern;
                glControl1.RenderFrame();
            }
        }

        private void magentaBlackStripesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (graphicsResources != null)
            {
                textureToRender = graphicsResources.floatMagentaBlackStripes;
                glControl1.RenderFrame();
            }
        }

        private void DrawShape(List<Vector3> shapeVertices, SFShapes.Mesh3D shape)
        {
            var pos = new VertexFloatAttribute("position", ValueCount.Three, VertexAttribPointerType.Float, false, AttributeUsage.Position, true, true);

            var generator = new VertexAttributeShaderGenerator();
            generator.CreateShader(new List<VertexAttribute>() { pos }, out string vert, out string frag);

            var shader = new Shader();
            shader.LoadShaders(new List<Tuple<string, ShaderType, string>>() {
                new Tuple<string, ShaderType, string>(vert, ShaderType.VertexShader, ""),
                new Tuple<string, ShaderType, string>(frag, ShaderType.FragmentShader, ""),
            });
            System.Diagnostics.Debug.WriteLine(shader.GetErrorLog());

            shader.UseProgram();

            shader.SetTexture("uvTestPattern", graphicsResources.uvTestPattern, 0);


            var camera = new Camera()
            {
                RenderWidth = glControl1.Width,
                RenderHeight = glControl1.Height
            };

            var boundingSphere = SFGraphics.Utils.BoundingSphereGenerator.GenerateBoundingSphere(shapeVertices);
            camera.FrameBoundingSphere(boundingSphere.Xyz, boundingSphere.W, 0);

            camera.NearClipPlane = 0.01f;
            camera.FarClipPlane = 100;
            camera.Zoom(-0.5f);
            camera.RotationXDegrees = -50;
            camera.RotationYDegrees = -180;

            var matrix = camera.MvpMatrix;
            shader.SetMatrix4x4("mvpMatrix", ref matrix);

            glControl1.MakeCurrent();
            GL.ClearColor(1, 1, 1, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shape.Draw(shader);

            glControl1.SwapBuffers();
        }

        private void drawCubeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cubeVertices = SFShapes.ShapeGenerator.GetCubePositions(Vector3.Zero, 1);
            var cube = new SFShapes.Mesh3D(cubeVertices);
            DrawShape(cubeVertices.Item1, cube);
        }

        private void drawSphereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sphereVertices = SFShapes.ShapeGenerator.GetSpherePositions(Vector3.Zero, 1, 32);
            var sphere = new SFShapes.Mesh3D(sphereVertices);
            DrawShape(sphereVertices.Item1, sphere);
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            glControl1.RenderFrame();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var result = FileFormatWavefront.FileFormatObj.Load(dialog.FileName, false);

                    var vertices = GetVertices(result);
                    modelToRender = new ObjMesh(vertices);
                    RenderFrame(null, null);
                }
            }
        }

        private void DrawModel()
        {
            var camera = new Camera()
            {
                RenderWidth = glControl1.Width,
                RenderHeight = glControl1.Height,
                NearClipPlane = 0.0001f,
                FarClipPlane = 100.0f
            };
            camera.FrameBoundingSphere(modelToRender.BoundingSphere.Xyz, modelToRender.BoundingSphere.W, 0);

            var generator = new VertexAttributeShaderGenerator();
            generator.CreateShader<ObjVertex>(out string vertexSource, out string fragmentSource);
            var shader = new Shader();
            shader.LoadShaders(vertexSource, fragmentSource);

            shader.UseProgram();
            shader.SetMatrix4x4(generator.MvpMatrixName, camera.MvpMatrix);

            GL.Clear(ClearBufferMask.DepthBufferBit);

            SFGenericModel.RenderState.GLRenderSettings.SetFaceCulling(new SFGenericModel.RenderState.FaceCullingSettings(true, CullFaceMode.Back));
            SFGenericModel.RenderState.GLRenderSettings.SetDepthTesting(new SFGenericModel.RenderState.DepthTestSettings(true, true, DepthFunction.Lequal));

            modelToRender.Draw(shader);

            glControl1.SwapBuffers();
        }

        private static List<ObjVertex> GetVertices(FileFormatWavefront.FileLoadResult<FileFormatWavefront.Model.Scene> result)
        {
            // TODO: Estimate vertex count.
            // TODO: Groups?
            var vertices = new List<ObjVertex>();

            foreach (var face in result.Model.UngroupedFaces)
            {
                foreach (var index in face.Indices)
                {
                    // TODO: Don't assume all attributes are present.
                    var position = new Vector3(result.Model.Vertices[index.vertex].x, result.Model.Vertices[index.vertex].y, result.Model.Vertices[index.vertex].z);

                    vertices.Add(new ObjVertex(position, Vector3.Zero, Vector2.Zero));
                }
            }

            return vertices;
        }
    }
}
