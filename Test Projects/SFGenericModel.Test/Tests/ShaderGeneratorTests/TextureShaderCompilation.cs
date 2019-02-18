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
        private static readonly List<VertexRenderingAttribute> correctAttributes = new List<VertexRenderingAttribute>()
        {
            new VertexRenderingAttribute("pos", ValueCount.Three, VertexAttribPointerType.Float, AttributeUsage.Position, false, false),
            new VertexRenderingAttribute("nrm", ValueCount.Three, VertexAttribPointerType.Float, AttributeUsage.Normal, false, false),
            new VertexRenderingAttribute("uv0", ValueCount.Two, VertexAttribPointerType.Float, AttributeUsage.TexCoord0, false, false)
        };
        
        [TestInitialize]
        public void Initialize()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void NoTextures()
        {
            Shader shader = CreateShader(new List<TextureRenderInfo>(), correctAttributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void NoNormalNoTexCoord()
        {
            var attributes = new List<VertexRenderingAttribute>()
            {
                new VertexRenderingAttribute("pos", ValueCount.Three, VertexAttribPointerType.Float, AttributeUsage.Position, false, false),
            };

            // The specific exception doesn't matter at this point.
            var e = Assert.ThrowsException<System.ArgumentException>(() => CreateShader(new List<TextureRenderInfo>(), attributes));
        }

        [TestMethod]
        public void NoTexturesNoAttributes()
        {
            var e = Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => CreateShader(new List<TextureRenderInfo>(), new List<VertexRenderingAttribute>()));
            Assert.IsTrue(e.Message.StartsWith("attributes must be non empty to generate a valid shader."));
        }

        [TestMethod]
        public void NoUvAttribute()
        {
            var attributes = new List<VertexRenderingAttribute>()
            {
                new VertexRenderingAttribute("pos", ValueCount.Four, VertexAttribPointerType.Float, AttributeUsage.Default, false, false),
                new VertexRenderingAttribute("nrm", ValueCount.Four, VertexAttribPointerType.Float, AttributeUsage.Normal, false, false),
                new VertexRenderingAttribute("uv0", ValueCount.Four, VertexAttribPointerType.Float, AttributeUsage.Default, false, false)
            };

            var e = Assert.ThrowsException<System.ArgumentException>(() => CreateShader(new List<TextureRenderInfo>(), attributes));
            Assert.IsTrue(e.Message.StartsWith("No matching texture coordinates attribute was found."));
        }

        [TestMethod]
        public void NoNormalAttribute()
        {
            var attributes = new List<VertexRenderingAttribute>()
            {
                new VertexRenderingAttribute("pos", ValueCount.Four, VertexAttribPointerType.Float, AttributeUsage.Default, false, false),
                new VertexRenderingAttribute("nrm", ValueCount.Four, VertexAttribPointerType.Float, AttributeUsage.Default, false, false),
                new VertexRenderingAttribute("uv0", ValueCount.Four, VertexAttribPointerType.Float, AttributeUsage.TexCoord0, false, false)
            };

            var e = Assert.ThrowsException<System.ArgumentException>(() => CreateShader(new List<TextureRenderInfo>(), attributes));
            Assert.IsTrue(e.Message.StartsWith("No matching vertex normal attribute was found."));
        }

        [TestMethod]
        public void Vec4Attributes()
        {
            var attributes = new List<VertexRenderingAttribute>()
            {
                new VertexRenderingAttribute("pos", ValueCount.Four, VertexAttribPointerType.Float, AttributeUsage.Position, false, false),
                new VertexRenderingAttribute("nrm", ValueCount.Four, VertexAttribPointerType.Float, AttributeUsage.Normal, false, false),
                new VertexRenderingAttribute("uv0", ValueCount.Four, VertexAttribPointerType.Float, AttributeUsage.TexCoord0, false, false)
            };

            Shader shader = CreateShader(new List<TextureRenderInfo>(), attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleTexture()
        {
            var textures = new List<TextureRenderInfo>()
            {
                new TextureRenderInfo("tex1", UvCoord.TexCoord0, TextureSwizzle.Rgb)
            };

            Shader shader = CreateShader(textures, correctAttributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleTextureSphere()
        {
            var textures = new List<TextureRenderInfo>()
            {
                new TextureRenderInfo("tex1", UvCoord.CamEnvSphere, TextureSwizzle.Rgb)
            };

            Shader shader = CreateShader(textures, correctAttributes);
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

            Shader shader = CreateShader(textures, correctAttributes);
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

            Shader shader = CreateShader(textures, correctAttributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        private static Shader CreateShader(List<TextureRenderInfo> textures, List<VertexRenderingAttribute> attributes)
        {
            TextureShaderGenerator.CreateShader(textures, attributes, out string vertexSource, out string fragmentSource);

            Shader shader = new Shader();
            shader.LoadShaders(vertexSource, fragmentSource);
            return shader;
        }
    }
}
