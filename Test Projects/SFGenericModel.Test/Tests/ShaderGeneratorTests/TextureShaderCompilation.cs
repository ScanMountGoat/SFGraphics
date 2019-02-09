using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using SFGenericModel.VertexAttributes;
using SFGenericModel.ShaderGenerators;
using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.Test.ShaderGeneratorTests
{
    [TestClass]
    public class TextureShaderCompilation
    {
        [TestInitialize]
        public void Initialize()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void NoTextures()
        {
            var pos = new VertexFloatAttribute("position", ValueCount.Three, VertexAttribPointerType.Float);
            var uv0 = new VertexFloatAttribute("uv", ValueCount.Two, VertexAttribPointerType.Float);
            Shader shader = CreateShader(new List<TextureRenderInfo>());
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SamePositionUvAttribute()
        {
            Shader shader = CreateShader(new List<TextureRenderInfo>());
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void Vec4TexCoord()
        {
            Shader shader = CreateShader(new List<TextureRenderInfo>());
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleTexture()
        {
            var textures = new List<TextureRenderInfo>()
            {
                new TextureRenderInfo("tex1", UvCoord.TexCoord0, TextureSwizzle.Rgb)
            };

            Shader shader = CreateShader(textures);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleTextureSphere()
        {
            var textures = new List<TextureRenderInfo>()
            {
                new TextureRenderInfo("tex1", UvCoord.CamEnvSphere, TextureSwizzle.Rgb)
            };

            Shader shader = CreateShader(textures);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleTextureCube()
        {
            var textures = new List<TextureRenderInfo>()
            {
                new TextureRenderInfo("tex1", UvCoord.CubeMap, TextureSwizzle.Rgb)
            };
            var pos = new VertexFloatAttribute("pos", ValueCount.Three, VertexAttribPointerType.Float);
            var uv0 = new VertexFloatAttribute("uv", ValueCount.Two, VertexAttribPointerType.Float);

            Shader shader = CreateShader(textures);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SameTextureDifferentSwizzle()
        {
            var textures = new List<TextureRenderInfo>()
            {
                new TextureRenderInfo("tex1", UvCoord.TexCoord0, TextureSwizzle.Rgb),
                new TextureRenderInfo("tex1", UvCoord.TexCoord0, TextureSwizzle.A)
            };
            var pos = new VertexFloatAttribute("pos", ValueCount.Three, VertexAttribPointerType.Float);
            var uv0 = new VertexFloatAttribute("uv", ValueCount.Two, VertexAttribPointerType.Float);

            Shader shader = CreateShader(textures);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        private static Shader CreateShader(List<TextureRenderInfo> textures)
        {
            // TODO: Test with incorrect attributes.
            var attributes = new List<VertexRenderingAttribute>()
            {
                new VertexRenderingAttribute("pos", ValueCount.Three, VertexAttribPointerType.Float, AttributeUsage.Position, false, false),
                new VertexRenderingAttribute("nrm", ValueCount.Three, VertexAttribPointerType.Float, AttributeUsage.Normal, false, false),
                new VertexRenderingAttribute("uv0", ValueCount.Two, VertexAttribPointerType.Float, AttributeUsage.TexCoord0, false, false)
            };

            TextureShaderGenerator.CreateShader(textures, attributes, out string vertexSource, out string fragmentSource);

            Shader shader = new Shader();
            shader.LoadShaders(vertexSource, fragmentSource);
            return shader;
        }
    }
}
