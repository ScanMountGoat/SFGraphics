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
            Shader shader = CreateShader(new List<TextureRenderInfo>(), pos, pos, uv0);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SamePositionUvAttribute()
        {
            var pos = new VertexFloatAttribute("position", ValueCount.Three, VertexAttribPointerType.Float);
            Shader shader = CreateShader(new List<TextureRenderInfo>(), pos, pos, pos);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void Vec4TexCoord()
        {
            // The extra components for texture coordinates and position should be ignored.
            var pos = new VertexFloatAttribute("pos", ValueCount.Four, VertexAttribPointerType.Float);
            var uv0 = new VertexFloatAttribute("uv", ValueCount.Four, VertexAttribPointerType.Float);

            Shader shader = CreateShader(new List<TextureRenderInfo>(), pos, pos, uv0);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleTexture()
        {
            var textures = new List<TextureRenderInfo>()
            {
                new TextureRenderInfo("tex1", UvCoord.TexCoord0, TextureSwizzle.Rgb)
            };
            var pos = new VertexFloatAttribute("pos", ValueCount.Three, VertexAttribPointerType.Float);
            var uv0 = new VertexFloatAttribute("uv", ValueCount.Two, VertexAttribPointerType.Float);

            Shader shader = CreateShader(textures, pos, pos, uv0);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleTextureSphere()
        {
            var textures = new List<TextureRenderInfo>()
            {
                new TextureRenderInfo("tex1", UvCoord.CamEnvSphere, TextureSwizzle.Rgb)
            };
            var pos = new VertexFloatAttribute("pos", ValueCount.Three, VertexAttribPointerType.Float);
            var uv0 = new VertexFloatAttribute("uv", ValueCount.Two, VertexAttribPointerType.Float);

            Shader shader = CreateShader(textures, pos, pos, uv0);
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

            Shader shader = CreateShader(textures, pos, pos, uv0);
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

            Shader shader = CreateShader(textures, pos, pos, uv0);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        private static Shader CreateShader(List<TextureRenderInfo> textures, VertexFloatAttribute pos, VertexFloatAttribute nrm, VertexFloatAttribute uv0)
        {
            TextureShaderGenerator.CreateShader(textures, pos, nrm, uv0, out string vertexSource, out string fragmentSource);
            Shader shader = new Shader();
            shader.LoadShaders(vertexSource, fragmentSource);
            return shader;
        }
    }
}
