﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.Cameras;
using SFGraphics.GLObjects.Textures;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

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
        private int renderModeIndex = 0;

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
                graphicsResources.screenTriangle.DrawScreenTexture(textureToRender,
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

        private void glControl1_Resize(object sender, EventArgs e)
        {
            glControl1.RenderFrame();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog() { Filter = "Wavefront Obj|*.obj" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var result = FileFormatWavefront.FileFormatObj.Load(dialog.FileName, false);

                    var vertices = GetVertices(result);
                    modelToRender = new ObjMesh(vertices);
                    glControl1.RenderFrame();
                }
            }
        }

        private void DrawModel()
        {
            var camera = new Camera()
            {
                RenderWidth = glControl1.Width,
                RenderHeight = glControl1.Height,
                NearClipPlane = 0.01f,
            };
            camera.FrameBoundingSphere(modelToRender.BoundingSphere.Xyz, modelToRender.BoundingSphere.W, 0);

            graphicsResources.objModelShader.UseProgram();
            graphicsResources.objModelShader.SetMatrix4x4("mvpMatrix", camera.MvpMatrix);
            graphicsResources.objModelShader.SetInt("attributeIndex", renderModeIndex);

            GL.Clear(ClearBufferMask.DepthBufferBit);

            SFGenericModel.RenderState.GLRenderSettings.SetFaceCulling(new SFGenericModel.RenderState.FaceCullingSettings(true, CullFaceMode.Back));
            SFGenericModel.RenderState.GLRenderSettings.SetDepthTesting(new SFGenericModel.RenderState.DepthTestSettings(true, true, DepthFunction.Lequal));

            modelToRender.Draw(graphicsResources.objModelShader);
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

                    var normal = Vector3.Zero;
                    if (index.normal != null)
                        normal = new Vector3(result.Model.Normals[(int)index.normal].x, result.Model.Normals[(int)index.normal].y, result.Model.Normals[(int)index.normal].z);

                    var texcoord = Vector2.Zero;
                    if (index.uv != null)
                        texcoord = new Vector2(result.Model.Uvs[(int)index.uv].u, result.Model.Uvs[(int)index.uv].v);

                    vertices.Add(new ObjVertex(position, normal, texcoord));
                }
            }

            return vertices;
        }

        private void glControl1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                // Convert 1 to 9 to 0 to 8.
                renderModeIndex = Math.Max(int.Parse(e.KeyChar.ToString()) - 1, 0);
                glControl1.RenderFrame();
            }
        }
    }
}
