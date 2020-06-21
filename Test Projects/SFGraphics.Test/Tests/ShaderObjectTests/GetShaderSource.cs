using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.ShaderObjectTests
{
    [TestClass]
    public class GetShaderSource : GraphicsContextTest
    {
        [TestMethod]
        public void GetEmptySource()
        {
            var shader = new ShaderObject("", ShaderType.FragmentShader);
            Assert.AreEqual("", shader.GetShaderSource());
        }

        [TestMethod]
        public void GetNullSource()
        {
            var shader = new ShaderObject(null, ShaderType.FragmentShader);
            Assert.AreEqual("", shader.GetShaderSource());
        }

        [TestMethod]
        public void GetValidSource()
        {
            var source = RenderTestUtils.ResourceShaders.GetShaderSource("valid.frag");
            var shader = new ShaderObject(source, ShaderType.FragmentShader);
            Assert.AreEqual(source, shader.GetShaderSource());
        }

        [TestMethod]
        public void GetInvalidSource()
        {
            var source = RenderTestUtils.ResourceShaders.GetShaderSource("invalid.frag");
            var shader = new ShaderObject(source, ShaderType.FragmentShader);
            Assert.AreEqual(source, shader.GetShaderSource());
        }
    }
}
