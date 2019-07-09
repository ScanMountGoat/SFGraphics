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
        public Shader objModelShader;

        public ScreenTriangle screenTriangle;

        public SamplerObject samplerObject;

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

            screenTextureShader = CreateScreenTextureShader();

            CreateObjModelShader();

            CreateSamplerObject();

            screenTriangle = new ScreenTriangle();

            Benchmark();
        }

        private void CreateObjModelShader()
        {
            var generator = new SFGenericModel.ShaderGenerators.VertexAttributeShaderGenerator();
            generator.CreateShader<ObjVertex>(out string vertexSource, out string fragmentSource);
            objModelShader = new Shader();
            objModelShader.LoadShaders(vertexSource, fragmentSource);
        }

        private void Benchmark()
        {
            int iterations = 100000;

            var material = new SFGenericModel.Materials.GenericMaterial();
            for (int i = 0; i < 100; i++)
            {
                material.AddInt(i.ToString(), i);
            }

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                //material.SetShaderUniforms(objModelShader);
            }
            stopwatch.Stop();
            System.Diagnostics.Debug.WriteLine($"Operation: { (double)stopwatch.ElapsedMilliseconds / iterations } ms");
        }

        private void CreateSamplerObject()
        {
            samplerObject = new SamplerObject
            {
                MinFilter = TextureMinFilter.NearestMipmapLinear,
                MagFilter = TextureMagFilter.Nearest
            };
        }

        private Shader CreateScreenTextureShader()
        {
            var shader = new Shader();
            shader.LoadShaders(ResourceTextFile.GetFileText("SFGraphicsGui.Shaders.screenTexture.vert"), ResourceTextFile.GetFileText("SFGraphicsGui.Shaders.screenTexture.frag"));

            return shader;
        }
    }
}
