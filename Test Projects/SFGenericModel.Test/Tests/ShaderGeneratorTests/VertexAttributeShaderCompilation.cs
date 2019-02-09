using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using SFGenericModel.VertexAttributes;
using SFGenericModel.ShaderGenerators;
using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.Test.ShaderGeneratorTests
{
    [TestClass]
    public class VertexAttributeShaderCompilation
    {
        private struct VertexStruct
        {
            [VertexRendering("pos", ValueCount.Three, VertexAttribPointerType.Float, AttributeUsage.Position, true, true)]
            OpenTK.Vector3 position;
        }

        [TestInitialize]
        public void Initialize()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void NoAttributes()
        {
            Shader shader = CreateShader(new List<VertexRenderingAttribute>());
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleVec3Attribute()
        {
            var attributes = new List<VertexRenderingAttribute>()
            {
                new VertexRenderingAttribute("test", ValueCount.Three, VertexAttribPointerType.Float, AttributeUsage.Position, false, false)
            };
            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleIntVec3Attribute()
        {
            var attributes = new List<VertexRenderingAttribute>()
            {
                new VertexRenderingAttribute("test", ValueCount.Three, VertexAttribIntegerType.Int, false, false)
            };
            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleUnsignedIntVec3Attribute()
        {
            var attributes = new List<VertexRenderingAttribute>()
            {
                new VertexRenderingAttribute("test", ValueCount.Three, VertexAttribIntegerType.UnsignedInt, false, false)
            };
            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleByteAttribute()
        {
            var attributes = new List<VertexRenderingAttribute>()
            {
                new VertexRenderingAttribute("test", ValueCount.One, VertexAttribIntegerType.Byte, false, false)
            };
            var e = Assert.ThrowsException<System.NotImplementedException>(() =>
                CreateShader(attributes));
        }

        [TestMethod]
        public void SingleVec2Attribute()
        {
            var attributes = new List<VertexRenderingAttribute>()
            {
                new VertexRenderingAttribute("test", ValueCount.Two, VertexAttribPointerType.Float, AttributeUsage.Default, false, false)
            };
            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleVec4Attribute()
        {
            var attributes = new List<VertexRenderingAttribute>()
            {
                new VertexRenderingAttribute("test", ValueCount.Four, VertexAttribPointerType.Float, AttributeUsage.Position, false, false)
            };
            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleFloatAttribute()
        {
            var attributes = new List<VertexRenderingAttribute>()
            {
                new VertexRenderingAttribute("test", ValueCount.One, VertexAttribPointerType.Float, AttributeUsage.Position, false, false)
            };
            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleIntAttribute()
        {
            var attributes = new List<VertexRenderingAttribute>()
            {
                new VertexRenderingAttribute("test", ValueCount.One, VertexAttribPointerType.Int, AttributeUsage.Position, false, false)
            };
            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleUnsignedIntAttribute()
        {
            // TODO: This is actually floating point?
            var attributes = new List<VertexRenderingAttribute>()
            {
                new VertexRenderingAttribute("test", ValueCount.One, VertexAttribPointerType.UnsignedInt, AttributeUsage.Position, false, false)
            };
            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void MixedVectorAttributes()
        {
            var attributes = new List<VertexRenderingAttribute>()
            {
                new VertexRenderingAttribute("test2", ValueCount.Two, VertexAttribPointerType.Float, AttributeUsage.Default, false, false),
                new VertexRenderingAttribute("test3", ValueCount.Three, VertexAttribPointerType.Float, AttributeUsage.Default, false, false),
                new VertexRenderingAttribute("test4", ValueCount.Four, VertexAttribPointerType.Float, AttributeUsage.Default, false, false)
            };

            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        private static Shader CreateShader(List<VertexRenderingAttribute> attributes)
        {
            VertexAttributeShaderGenerator.CreateShader(attributes, out string vertSource, out string fragSource);
            Shader shader = new Shader();
            shader.LoadShaders(vertSource, fragSource);
            return shader;
        }
    }
}
