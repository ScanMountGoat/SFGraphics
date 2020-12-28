using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using SFGenericModel.VertexAttributes;
using SFGraphics.ShaderGen;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.ShaderGen.Test.ShaderGeneratorTests
{
    [TestClass]
    public class CreateAttributeShader
    {
        private struct VertexStruct
        {
            [VertexFloat("pos", ValueCount.Three, VertexAttribPointerType.Float, false, AttributeUsage.Position, true, true)]
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
            var shader = CreateShader(new List<VertexAttribute>());
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleVec3Attribute()
        {
            var attributes = new List<VertexAttribute>()
            {
                new VertexFloatAttribute("test", ValueCount.Three, VertexAttribPointerType.Float, false, AttributeUsage.Position, false, false)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleVec3AttributeFromStruct()
        {
            var shader = CreateShader<VertexStruct>();
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleIntVec3Attribute()
        {
            var attributes = new List<VertexAttribute>()
            {
                new VertexIntAttribute("test", ValueCount.Three, VertexAttribIntegerType.Int, AttributeUsage.Default, false, false)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleUnsignedIntVec3Attribute()
        {
            var attributes = new List<VertexAttribute>()
            {
                new VertexIntAttribute("test", ValueCount.Three, VertexAttribIntegerType.UnsignedInt, AttributeUsage.Default, false, false)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleByteAttribute()
        {
            var attributes = new List<VertexAttribute>()
            {
                new VertexIntAttribute("test", ValueCount.One, VertexAttribIntegerType.Byte, AttributeUsage.Position, false, false)
            };
            var e = Assert.ThrowsException<System.NotImplementedException>(() =>
                CreateShader(attributes));
        }

        [TestMethod]
        public void SingleVec2Attribute()
        {
            var attributes = new List<VertexAttribute>()
            {
                new VertexFloatAttribute("test", ValueCount.Two, VertexAttribPointerType.Float, false, AttributeUsage.Default, false, false)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleVec4Attribute()
        {
            var attributes = new List<VertexAttribute>()
            {
                new VertexFloatAttribute("test", ValueCount.Four, VertexAttribPointerType.Float, false, AttributeUsage.Position, false, false)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleFloatAttribute()
        {
            var attributes = new List<VertexAttribute>()
            {
                new VertexFloatAttribute("test", ValueCount.One, VertexAttribPointerType.Float, false, AttributeUsage.Position, false, false)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleIntAttribute()
        {
            var attributes = new List<VertexAttribute>()
            {
                new VertexIntAttribute("test", ValueCount.One, VertexAttribIntegerType.Int, AttributeUsage.Default, false, false)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void SingleUnsignedIntAttribute()
        {
            var attributes = new List<VertexAttribute>()
            {
                new VertexIntAttribute("test", ValueCount.One, VertexAttribIntegerType.UnsignedInt, AttributeUsage.Default, false, false)
            };

            var shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void MixedVectorAttributes()
        {
            var attributes = new List<VertexAttribute>()
            {
                new VertexFloatAttribute("test2", ValueCount.Two, VertexAttribPointerType.Float, false, AttributeUsage.Default, false, false),
                new VertexFloatAttribute("test3", ValueCount.Three, VertexAttribPointerType.Float, false, AttributeUsage.Default, false, false),
                new VertexFloatAttribute("test4", ValueCount.Four, VertexAttribPointerType.Float, false, AttributeUsage.Default, false, false)
            };

            Shader shader = CreateShader(attributes);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        private static Shader CreateShader(List<VertexAttribute> attributes)
        {
            var generator = new VertexAttributeShaderGenerator();
            generator.CreateShader(attributes, out string vertSource, out string fragSource);
            Shader shader = new Shader();
            shader.LoadShaders(vertSource, fragSource);
            return shader;
        }

        private static Shader CreateShader<T>() where T : struct
        {
            var generator = new VertexAttributeShaderGenerator();
            generator.CreateShader<T>(out string vertSource, out string fragSource);

            var shader = new Shader();
            shader.LoadShaders(vertSource, fragSource);
            return shader;
        }
    }
}
