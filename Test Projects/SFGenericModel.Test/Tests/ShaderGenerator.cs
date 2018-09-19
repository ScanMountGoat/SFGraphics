using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using SFGenericModel.VertexAttributes;
using SFGenericModel.ShaderGenerators;
using OpenTK.Graphics.OpenGL;

namespace GenericMeshTests
{
    [TestClass]
    public class ShaderGenerator
    {
        [TestInitialize]
        public void Initialize()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void NoAttributes()
        {
            Shader shader = VertexAttributeShaderGenerator.CreateShader(new List<VertexAttributeRenderInfo>());
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleVec3Attribute()
        {
            var attributes = new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(false, false, new VertexAttributeInfo("test", ValueCount.Three, VertexAttribPointerType.Float))
            };
            Shader shader = VertexAttributeShaderGenerator.CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleVec2Attribute()
        {
            var attributes = new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(false, false, new VertexAttributeInfo("test", ValueCount.Two, VertexAttribPointerType.Float))
            };
            Shader shader = VertexAttributeShaderGenerator.CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleVec4Attribute()
        {
            var attributes = new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(false, false, new VertexAttributeInfo("test", ValueCount.Four, VertexAttribPointerType.Float))
            };
            Shader shader = VertexAttributeShaderGenerator.CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleFloatAttribute()
        {
            var attributes = new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(false, false, new VertexAttributeInfo("test", ValueCount.One, VertexAttribPointerType.Float))
            };
            Shader shader = VertexAttributeShaderGenerator.CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleIntAttribute()
        {
            var attributes = new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(false, false, new VertexAttributeInfo("test", ValueCount.One, VertexAttribPointerType.Int))
            };
            Shader shader = VertexAttributeShaderGenerator.CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleUnsignedIntAttribute()
        {
            var attributes = new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(false, false, new VertexAttributeInfo("test", ValueCount.One, VertexAttribPointerType.UnsignedInt))
            };
            Shader shader = VertexAttributeShaderGenerator.CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void MixedVectorAttributes()
        {
            var attributes = new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(false, false, new VertexAttributeInfo("test2", ValueCount.Two, VertexAttribPointerType.Float)),
                new VertexAttributeRenderInfo(false, false, new VertexAttributeInfo("test3", ValueCount.Three, VertexAttribPointerType.Float)),
                new VertexAttributeRenderInfo(false, false, new VertexAttributeInfo("test4", ValueCount.Four, VertexAttribPointerType.Float))
            };

            Shader shader = VertexAttributeShaderGenerator.CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }
    }
}
