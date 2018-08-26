using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects;
using SFGraphics.GLObjects.Samplers;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;
using SFGraphics.Tools;

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
        public SamplerObject samplerObject;

        /// <summary>
        /// Create the <see cref="uvTestPattern"/>, <see cref="screenTextureShader"/>, and <see cref="screenTriangleVbo"/>.
        /// Requires an OpenTK context to be current.
        /// </summary>
        public GraphicsResources()
        {
            // Texture setup from a bitmap.
            uvTestPattern = new Texture2D();
            uvTestPattern.LoadImageData(Properties.Resources.UVPattern);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            floatMagentaBlackStripes = CreateTextureFromFloatValues(true, 64, 64);

            System.Diagnostics.Debug.WriteLine($"Create texture: { stopwatch.ElapsedMilliseconds } ms");

            screenTextureShader = CreateShader();

            CreateSamplerObject();

            screenTriangleVbo = CreateScreenQuadBuffer();
        }

        private void CreateSamplerObject()
        {
            samplerObject = new SamplerObject();
            samplerObject.MinFilter = TextureMinFilter.NearestMipmapLinear;
            samplerObject.MagFilter = TextureMagFilter.Nearest;
        }

        private static Texture2D CreateTextureFromFloatValues(bool usePbo, int width, int height)
        {
            Texture2D floatTexture = new Texture2D();
            floatTexture.MinFilter = TextureMinFilter.Nearest;
            floatTexture.MagFilter = TextureMagFilter.Nearest;

            int mipmaps = 0;

            Vector3[] pixels = GetImagePixels(width, height);

            if (usePbo)
                LoadFloatTexImageDataPbo(floatTexture, pixels, width, height);
            else
                LoadFloatTexImageData(floatTexture, pixels, width, height, mipmaps);

            return floatTexture;
        }

        private static void LoadFloatTexImageData(Texture2D floatTexture, Vector3[] pixels, int width, int height, int mipmaps)
        {
            floatTexture.LoadImageData(width, height, pixels, mipmaps,
                new TextureFormatUncompressed(PixelInternalFormat.Rgb, PixelFormat.Rgb, PixelType.Float));
        }

        private static void LoadFloatTexImageDataPbo(Texture2D floatTexture, Vector3[] pixels, int width, int height)
        {
            BufferObject pixelBuffer = new BufferObject(BufferTarget.PixelUnpackBuffer);
            pixelBuffer.SetData(pixels, BufferUsageHint.StaticDraw);
            floatTexture.LoadImageData(width, height, pixelBuffer, 0,
                new TextureFormatUncompressed(PixelInternalFormat.Rgb, PixelFormat.Rgb, PixelType.Float));
        }

        private static Vector3[] GetImagePixels(int width, int height)
        {
            Vector3[] pixels = new Vector3[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                // Magenta and black stripes.
                pixels[i] = new Vector3(1, 0, 1) * (i % 2);
            }

            return pixels;
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
            bufferObject.SetData(screenTrianglePositions, BufferUsageHint.StaticDraw);

            return bufferObject;
        }
    }
}
