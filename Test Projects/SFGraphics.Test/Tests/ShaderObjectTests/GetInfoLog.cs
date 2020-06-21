using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.ShaderObjectTests
{
    [TestClass]
    public class GetInfoLog : GraphicsContextTest
    {
        [TestMethod]
        public void FromEmptySource()
        {
            var shader = new ShaderObject("", ShaderType.FragmentShader);
            Assert.AreEqual("", shader.GetInfoLog());
        }

        [TestMethod]
        public void FromNullSource()
        {
            var shader = new ShaderObject("", ShaderType.FragmentShader);
            Assert.AreEqual("", shader.GetInfoLog());
        }

        [TestMethod]
        public void FromValidSource()
        {
            var source = RenderTestUtils.ResourceShaders.GetShaderSource("valid.frag");
            var shader = new ShaderObject(source, ShaderType.FragmentShader);
            Assert.AreEqual("", shader.GetInfoLog());
        }

        [TestMethod]
        public void FromInvalidSource()
        {
            // The logs are driver dependent, so just check that it produces some sort of error message.
            var source = RenderTestUtils.ResourceShaders.GetShaderSource("invalid.frag");
            var shader = new ShaderObject(source, ShaderType.FragmentShader);
            Assert.IsTrue(shader.GetInfoLog().ToLower().Contains("error"));
        }
    }
}
