using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.ShaderObjectTests
{
    [TestClass]
    public class CreateShader : GraphicsContextTest
    {
        [TestMethod]
        public void FromEmptySource()
        {
            var shader = new ShaderObject("", ShaderType.FragmentShader);
            Assert.IsFalse(shader.WasCompiledSuccessfully);
        }

        [TestMethod]
        public void FromNullSource()
        {
            var shader = new ShaderObject("", ShaderType.FragmentShader);
            Assert.IsFalse(shader.WasCompiledSuccessfully);
        }

        [TestMethod]
        public void FromValidSource()
        {
            var source = RenderTestUtils.ResourceShaders.GetShaderSource("valid.frag");
            var shader = new ShaderObject(source, ShaderType.FragmentShader);
            Assert.IsTrue(shader.WasCompiledSuccessfully);
        }

        [TestMethod]
        public void FromInvalidSource()
        {
            var source = RenderTestUtils.ResourceShaders.GetShaderSource("invalid.frag");
            var shader = new ShaderObject(source, ShaderType.FragmentShader);
            Assert.IsFalse(shader.WasCompiledSuccessfully);
        }

        [TestMethod]
        public void SetShaderTypeFrag()
        {
            var source = RenderTestUtils.ResourceShaders.GetShaderSource("valid.frag");
            var shader = new ShaderObject(source, ShaderType.FragmentShader);
            Assert.AreEqual(ShaderType.FragmentShader, shader.ShaderType);
        }

        [TestMethod]
        public void SetShaderTypeGeom()
        {
            var source = RenderTestUtils.ResourceShaders.GetShaderSource("valid.frag");
            var shader = new ShaderObject(source, ShaderType.GeometryShader);
            Assert.AreEqual(ShaderType.GeometryShader, shader.ShaderType);
        }
    }
}
