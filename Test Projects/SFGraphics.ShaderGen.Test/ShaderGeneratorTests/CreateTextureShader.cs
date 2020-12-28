using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGenericModel.VertexAttributes;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;

namespace SFGraphics.ShaderGen.Test.ShaderGeneratorTests
{
    [TestClass]
    public class CreateTextureShader
    {
        private struct VertexStruct
        {
            [VertexFloat("pos", ValueCount.Three, VertexAttribPointerType.Float, false, AttributeUsage.Position, false, false)]
            public Vector3 Position { get; }

            [VertexFloat("nrm", ValueCount.Three, VertexAttribPointerType.Float, false, AttributeUsage.Normal, false, false)]
            public Vector3 Normal { get; }

            [VertexFloat("uv0", ValueCount.Two, VertexAttribPointerType.Float, false, AttributeUsage.TexCoord0, false, false)]
            public Vector2 Uv0 { get; }
        }

        private static readonly List<VertexAttribute> correctAttributes = new List<VertexAttribute>()
        {
            new VertexFloatAttribute("pos", ValueCount.Three, VertexAttribPointerType.Float, false, AttributeUsage.Position, false, false),
            new VertexFloatAttribute("nrm", ValueCount.Three, VertexAttribPointerType.Float, false, AttributeUsage.Normal, false, false),
            new VertexFloatAttribute("uv0", ValueCount.Two, VertexAttribPointerType.Float, false, AttributeUsage.TexCoord0, false, false)
        };

        private static readonly List<VertexAttribute> attributesScalarNormal = new List<VertexAttribute>()
        {
            new VertexFloatAttribute("pos", ValueCount.Three, VertexAttribPointerType.Float, false, AttributeUsage.Position, false, false),
            new VertexFloatAttribute("nrm", ValueCount.One, VertexAttribPointerType.Float, false, AttributeUsage.Normal, false, false),
            new VertexFloatAttribute("uv0", ValueCount.Two, VertexAttribPointerType.Float, false, AttributeUsage.TexCoord0, false, false)
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
            var attributes = new List<VertexAttribute>()
            {
                new VertexFloatAttribute("pos", ValueCount.Three, VertexAttribPointerType.Float, false, AttributeUsage.Position, false, false),
            };

            // The specific exception doesn't matter at this point.
            Assert.ThrowsException<System.ArgumentException>(() => CreateShader(new List<TextureRenderInfo>(), attributes));
        }

        [TestMethod]
        public void NoTexturesNoAttributes()
        {
            var e = Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => CreateShader(new List<TextureRenderInfo>(), new List<VertexAttribute>()));

            Assert.IsTrue(e.Message.Contains("attributes must be non empty to generate a valid shader."));
            Assert.AreEqual("attributes", e.ParamName);
        }

        [TestMethod]
        public void NoUvAttribute()
        {
            var attributes = new List<VertexAttribute>()
            {
                new VertexFloatAttribute("pos", ValueCount.Four, VertexAttribPointerType.Float, false, AttributeUsage.Default, false, false),
                new VertexFloatAttribute("nrm", ValueCount.Four, VertexAttribPointerType.Float, false, AttributeUsage.Normal, false, false),
                new VertexFloatAttribute("uv0", ValueCount.Four, VertexAttribPointerType.Float, false, AttributeUsage.Default, false, false)
            };

            var e = Assert.ThrowsException<System.ArgumentException>(() => CreateShader(new List<TextureRenderInfo>(), attributes));

            Assert.IsTrue(e.Message.Contains("No matching texture coordinates attribute was found."));
            Assert.AreEqual("attributes", e.ParamName);
        }

        [TestMethod]
        public void NoNormalAttribute()
        {
            var attributes = new List<VertexAttribute>()
            {
                new VertexFloatAttribute("pos", ValueCount.Four, VertexAttribPointerType.Float, false, AttributeUsage.Default, false, false),
                new VertexFloatAttribute("nrm", ValueCount.Four, VertexAttribPointerType.Float, false, AttributeUsage.Default, false, false),
                new VertexFloatAttribute("uv0", ValueCount.Four, VertexAttribPointerType.Float, false, AttributeUsage.TexCoord0, false, false)
            };

            var e = Assert.ThrowsException<System.ArgumentException>(() => CreateShader(new List<TextureRenderInfo>(), attributes));

            Assert.IsTrue(e.Message.Contains("No matching vertex normal attribute was found."));
            Assert.AreEqual("attributes", e.ParamName);
        }

        [TestMethod]
        public void Vec4Attributes()
        {
            var attributes = new List<VertexAttribute>()
            {
                new VertexFloatAttribute("pos", ValueCount.Four, VertexAttribPointerType.Float, false, AttributeUsage.Position, false, false),
                new VertexFloatAttribute("nrm", ValueCount.Four, VertexAttribPointerType.Float, false, AttributeUsage.Normal, false, false),
                new VertexFloatAttribute("uv0", ValueCount.Four, VertexAttribPointerType.Float, false, AttributeUsage.TexCoord0, false, false)
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
        public void SingleTextureVertexStruct()
        {
            var textures = new List<TextureRenderInfo>()
            {
                new TextureRenderInfo("tex1", UvCoord.TexCoord0, TextureSwizzle.Rgb)
            };

            Shader shader = CreateShader<VertexStruct>(textures);
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
        public void SingleTextureSphereIncorrectNormals()
        {
            var textures = new List<TextureRenderInfo>()
            {
                new TextureRenderInfo("tex1", UvCoord.CamEnvSphere, TextureSwizzle.Rgb)
            };

            Shader shader = CreateShader(textures, attributesScalarNormal);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleTextureCube()
        {
            var textures = new List<TextureRenderInfo>()
            {
                new TextureRenderInfo("tex1", UvCoord.CubeMap, TextureSwizzle.Rgb)
            };

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

        private static Shader CreateShader(List<TextureRenderInfo> textures, List<VertexAttribute> attributes)
        {
            var generator = new TextureShaderGenerator();
            generator.CreateShader(textures, attributes, out string vertexSource, out string fragmentSource);

            Shader shader = new Shader();
            shader.LoadShaders(vertexSource, fragmentSource);
            return shader;
        }

        private static Shader CreateShader<T>(List<TextureRenderInfo> textures) where T : struct
        {
            var generator = new TextureShaderGenerator();
            generator.CreateShader<T>(textures, out string vertexSource, out string fragmentSource);

            Shader shader = new Shader();
            shader.LoadShaders(vertexSource, fragmentSource);
            return shader;
        }
    }
}
