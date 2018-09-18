using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using SFGenericModel.VertexAttributes;
using SFGenericModel.VertexAttributeShader;
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
    }
}
