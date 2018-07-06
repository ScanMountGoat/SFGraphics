using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects;
using OpenTK.Graphics.OpenGL;


namespace SFGraphicsGui
{
    class GraphicsResources
    {
        // A triangle that extends past the screen.
        // Avoids the need for a second triangle to fill a rectangular screen.
        private static float[] screenTrianglePositions =
        {
            -1f, -1f, 0.0f,
             3f, -1f, 0.0f,
            -1f,  3f, 0.0f
        };

        // Don't call the constructors until an OpenTK context is current.
        public Texture uvTestPattern;
        public Shader screenTextureShader;
        public BufferObject screenTriangleVbo;

        /// <summary>
        /// Create the <see cref="uvTestPattern"/>, <see cref="screenTextureShader"/>, and <see cref="screenTriangleVbo"/>.
        /// Requires an OpenTK context to be current.
        /// </summary>
        public GraphicsResources()
        {
            // Texture setup from a bitmap.
            uvTestPattern = new Texture2D(Properties.Resources.UVPattern);

            // Shader setup.
            screenTextureShader = new Shader();
            string vertShaderSource = ResourceTextFile.GetFileText("SFGraphicsGui.Shaders.screenTexture.vert");
            screenTextureShader.LoadShader(vertShaderSource, ShaderType.VertexShader);

            string fragShaderSource = ResourceTextFile.GetFileText("SFGraphicsGui.Shaders.screenTexture.frag");
            screenTextureShader.LoadShader(fragShaderSource, ShaderType.FragmentShader);

            // Create a buffer for drawing.
            CreateScreenQuadBuffer();
        }

        private void CreateScreenQuadBuffer()
        {
            // Create buffer for vertex positions. 
            // The data won't change, so only initialize once.
            screenTriangleVbo = new BufferObject(BufferTarget.ArrayBuffer);
            screenTriangleVbo.Bind();
            GL.BufferData(screenTriangleVbo.BufferTarget, (IntPtr)(sizeof(float) * screenTrianglePositions.Length),
                screenTrianglePositions, BufferUsageHint.StaticDraw);
        }
    }
}
