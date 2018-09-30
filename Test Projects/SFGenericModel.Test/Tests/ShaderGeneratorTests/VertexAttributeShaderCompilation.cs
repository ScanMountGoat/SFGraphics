using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using SFGenericModel.VertexAttributes;
using SFGenericModel.ShaderGenerators;
using OpenTK.Graphics.OpenGL;

namespace ShaderGeneratorTests
{
    [TestClass]
    public class VertexAttributeShaderCompilation
    {
        [TestInitialize]
        public void Initialize()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void NoAttributes()
        {
            Shader shader = CreateShader(new List<VertexAttributeRenderInfo>());
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleVec3Attribute()
        {
            var attributes = new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(new VertexAttributeInfo("test", ValueCount.Three, VertexAttribPointerType.Float), false, false)
            };
            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleVec2Attribute()
        {
            var attributes = new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(new VertexAttributeInfo("test", ValueCount.Two, VertexAttribPointerType.Float), false, false)
            };
            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleVec4Attribute()
        {
            var attributes = new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(new VertexAttributeInfo("test", ValueCount.Four, VertexAttribPointerType.Float), false, false)
            };
            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleFloatAttribute()
        {
            var attributes = new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(new VertexAttributeInfo("test", ValueCount.One, VertexAttribPointerType.Float), false, false)
            };
            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleIntAttribute()
        {
            var attributes = new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(new VertexAttributeInfo("test", ValueCount.One, VertexAttribPointerType.Int), false, false)
            };
            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleUnsignedIntAttribute()
        {
            var attributes = new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(new VertexAttributeInfo("test", ValueCount.One, VertexAttribPointerType.UnsignedInt), false, false)
            };
            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void MixedVectorAttributes()
        {
            var attributes = new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(new VertexAttributeInfo("test2", ValueCount.Two, VertexAttribPointerType.Float), false, false),
                new VertexAttributeRenderInfo(new VertexAttributeInfo("test3", ValueCount.Three, VertexAttribPointerType.Float), false, false),
                new VertexAttributeRenderInfo(new VertexAttributeInfo("test4", ValueCount.Four, VertexAttribPointerType.Float), false, false)
            };

            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        private static Shader CreateShader(List<VertexAttributeRenderInfo> attributes)
        {
            VertexAttributeShaderGenerator.CreateShader(attributes, out string vertSource, out string fragSource);
            Shader shader = new Shader();
            shader.LoadShaders(vertSource, fragSource);
            return shader;
        }
    }
}
