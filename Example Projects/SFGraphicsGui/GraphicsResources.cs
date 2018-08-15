using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects;
using SFGraphics.Tools;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace SFGraphicsGui
{
    class GraphicsResources
    {
        // A triangle that extends past the screen.
        // Avoids the need for a second triangle to fill a rectangular screen.
        // The positions can also be conveniently converted to UVs.
        private static float[] screenTrianglePositions =
        {
            -1f, -1f, 0.0f,
             3f, -1f, 0.0f,
            -1f,  3f, 0.0f
        };

        // Don't call the constructors until an OpenGL context is current to prevent crashes.
        public Texture2D uvTestPattern;
        public Texture2D floatMagentaBlackStripes;
        public Shader screenTextureShader;
        public BufferObject screenTriangleVbo;

        /// <summary>
        /// Create the <see cref="uvTestPattern"/>, <see cref="screenTextureShader"/>, and <see cref="screenTriangleVbo"/>.
        /// Requires an OpenTK context to be current.
        /// </summary>
        public GraphicsResources()
        {
            // Texture setup from a bitmap.
            uvTestPattern = new Texture2D();
            uvTestPattern.LoadImageData(Properties.Resources.UVPattern);

            floatMagentaBlackStripes = CreateTextureFromFloatValues();

            screenTextureShader = CreateShader();
            screenTriangleVbo = CreateScreenQuadBuffer();
        }

        private static Texture2D CreateTextureFromFloatValues()
        {
            Texture2D floatTexture = new Texture2D();
            floatTexture.MinFilter = TextureMinFilter.Nearest;
            floatTexture.MagFilter = TextureMagFilter.Nearest;

            int width = 32;
            int height = 32;
            int mipmaps = 0;

            Vector3[] pixels = new Vector3[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                // Magenta and black stripes.
                pixels[i] = new Vector3(1, 0, 1) * (i % 2);
            }

            floatTexture.LoadImageData(width, height, pixels, mipmaps,
                new TextureFormatUncompressed(PixelInternalFormat.Rgb, PixelFormat.Rgb, PixelType.Float));
            return floatTexture;
        }

        private Shader CreateShader()
        {
            Shader shader = new Shader();
            string vertShaderSource = ResourceTextFile.GetFileText("SFGraphicsGui.Shaders.screenTexture.vert");
            shader.LoadShader(vertShaderSource, ShaderType.VertexShader, "screenTexture");

            string fragShaderSource = ResourceTextFile.GetFileText("SFGraphicsGui.Shaders.screenTexture.frag");
            shader.LoadShader(fragShaderSource, ShaderType.FragmentShader, "screenTexture");

            // An example of how to use precompiled shaders.
            // The program binary can be saved to a file to avoid compiling shaders
            // every time the application is run.
            if (OpenGLExtensions.IsAvailable("GL_ARB_get_program_binary"))
            {
                BinaryFormat binaryFormat;
                byte[] programBinary = shader.GetProgramBinary(out binaryFormat);

                shader.LoadProgramBinary(programBinary, binaryFormat);
            }

            return shader;
        }

        private BufferObject CreateScreenQuadBuffer()
        {
            // Create buffer for vertex positions. 
            // The data won't change, so only initialize once.
            BufferObject bufferObject = new BufferObject(BufferTarget.ArrayBuffer);
            bufferObject.BufferData(screenTrianglePositions, sizeof(float), BufferUsageHint.StaticDraw);

            return bufferObject;
        }
    }
}
