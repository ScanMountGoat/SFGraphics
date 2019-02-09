using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Samplers;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;
using System;

namespace SFGraphicsGui
{
    class GraphicsResources
    {
        // Don't call the constructors until an OpenGL context is current to prevent crashes.
        public Texture2D uvTestPattern;
        public Texture2D floatMagentaBlackStripes;

        public Shader screenTextureShader;

        public ScreenTriangle screenTriangle;

        public SamplerObject samplerObject;

        public Texture3D lutTexture;

        /// <summary>
        /// Create the <see cref="uvTestPattern"/>, <see cref="screenTextureShader"/>, and <see cref="screenTriangle"/>.
        /// Requires an OpenTK context to be current.
        /// </summary>
        public GraphicsResources()
        {
            // Texture setup from a bitmap.
            uvTestPattern = new Texture2D();
            uvTestPattern.LoadImageData(Properties.Resources.UVPattern);

            floatMagentaBlackStripes = TextureCreation.CreateStripes(true, 64, 64);
            screenTextureShader = CreateShader();
            CreateSamplerObject();
            screenTriangle = new ScreenTriangle();

            lutTexture = new Texture3D();
            lutTexture.LoadImageData(1, 1, 1, new float[] { 1, 1, 1, 1 }, new TextureFormatUncompressed(PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.Float));

            Benchmark();
        }

        private void Benchmark()
        {
            int length = 10000;

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < length; i++)
            {
                SFGenericModel.RenderState.GLRenderSettings.SetRenderSettings(new SFGenericModel.RenderState.RenderSettings());
            }
            System.Diagnostics.Debug.WriteLine($"Operation: { (double)stopwatch.ElapsedMilliseconds / length } ms");
        }

        private void CreateSamplerObject()
        {
            samplerObject = new SamplerObject
            {
                MinFilter = TextureMinFilter.NearestMipmapLinear,
                MagFilter = TextureMagFilter.Nearest
            };
        }

        private Shader CreateShader()
        {
            var shader = new Shader();

            var vertShaderSource = ResourceTextFile.GetFileText("SFGraphicsGui.Shaders.screenTexture.vert");
            var fragShaderSource = ResourceTextFile.GetFileText("SFGraphicsGui.Shaders.screenTexture.frag");

            shader.LoadShaders(new Tuple<string, ShaderType, string>[] {
                new Tuple<string, ShaderType, string>(fragShaderSource, ShaderType.FragmentShader, ""),
                new Tuple<string, ShaderType, string>(vertShaderSource, ShaderType.VertexShader, "")
            });

            return shader;
        }
    }
}
