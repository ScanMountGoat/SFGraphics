using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.ShaderGen.GlslShaderUtils;
using System.Collections.Generic;

namespace SFGraphics.ShaderGen.Test.ShaderGeneratorTests
{
    [TestClass]
    public class CreateAttributeShader
    {
        [TestInitialize]
        public void Initialize()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void NoAttributes()
        {
            var shader = CreateShader(new List<ShaderAttribute>());
            Assert.IsTrue(shader.LinkStatusIsOk, shader.GetErrorLog());
        }

        [TestMethod]
        public void SingleVec3Attribute()
        {
            var attributes = new List<ShaderAttribute>()
            {
                new ShaderAttribute("test", AttributeType.Vec3)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk, shader.GetErrorLog());
        }

        [TestMethod]
        public void SingleIntVec2Attribute()
        {
            var attributes = new List<ShaderAttribute>()
            {
                new ShaderAttribute("test", AttributeType.IVec2)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk, shader.GetErrorLog());
        }

        [TestMethod]
        public void SingleIntVec3Attribute()
        {
            var attributes = new List<ShaderAttribute>()
            {
                new ShaderAttribute("test", AttributeType.IVec3)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk, shader.GetErrorLog());
        }

        [TestMethod]
        public void SingleUnsignedIntVec2Attribute()
        {
            var attributes = new List<ShaderAttribute>()
            {
                new ShaderAttribute("test", AttributeType.UVec2)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk, shader.GetErrorLog());
        }

        [TestMethod]
        public void SingleUnsignedIntVec3Attribute()
        {
            var attributes = new List<ShaderAttribute>()
            {
                new ShaderAttribute("test", AttributeType.UVec3)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk, shader.GetErrorLog());
        }

        [TestMethod]
        public void SingleVec2Attribute()
        {
            var attributes = new List<ShaderAttribute>()
            {
                new ShaderAttribute("test", AttributeType.Vec2)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk, shader.GetErrorLog());
        }

        [TestMethod]
        public void SingleVec4Attribute()
        {
            var attributes = new List<ShaderAttribute>()
            {
                new ShaderAttribute("test", AttributeType.Vec4)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk, shader.GetErrorLog());
        }

        [TestMethod]
        public void SingleFloatAttribute()
        {
            var attributes = new List<ShaderAttribute>()
            {
                new ShaderAttribute("test", AttributeType.Float)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk, shader.GetErrorLog());
        }

        [TestMethod]
        public void SingleIntAttribute()
        {
            var attributes = new List<ShaderAttribute>()
            {
                new ShaderAttribute("test", AttributeType.Int)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk, shader.GetErrorLog());
        }

        [TestMethod]
        public void SingleUnsignedIntAttribute()
        {
            var attributes = new List<ShaderAttribute>()
            {
                new ShaderAttribute("test", AttributeType.UnsignedInt)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk, shader.GetErrorLog());
        }

        [TestMethod]
        public void MixedVectorAttributes()
        {
            var attributes = new List<ShaderAttribute>()
            {
                new ShaderAttribute("test2", AttributeType.Vec2),
                new ShaderAttribute("test3", AttributeType.Vec3),
                new ShaderAttribute("test4", AttributeType.Vec4)
            };

            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk, shader.GetErrorLog());
        }

        private static Shader CreateShader(List<ShaderAttribute> attributes)
        {
            var generator = new VertexAttributeShaderGenerator();
            generator.CreateShader(attributes, out string vertSource, out string fragSource);
            Shader shader = new Shader();
            shader.LoadShaders(vertSource, fragSource);
            return shader;
        }
    }
}
