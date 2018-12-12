using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class JustFragShader : Tests.ContextTest
    {
        [TestMethod]
        public void ValidFragShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            string shaderSource = RenderTestUtils.ResourceShaders.GetShaderSource("valid.frag");
            shader.LoadShader(shaderSource, ShaderType.FragmentShader);

            Assert.IsTrue(shader.LinkStatusIsOk);

            Assert.AreEqual(18, shader.ActiveUniformCount);
            Assert.AreEqual(0, shader.ActiveAttributeCount);
        }

        [TestMethod]
        public void InvalidFragShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            string shaderSource = RenderTestUtils.ResourceShaders.GetShaderSource("invalid.frag");
            shader.LoadShader(shaderSource, ShaderType.FragmentShader);

            Assert.IsFalse(shader.LinkStatusIsOk);

            Assert.AreEqual(0, shader.ActiveUniformCount);
            Assert.AreEqual(0, shader.ActiveAttributeCount);
        }

        [TestMethod]
        public void EmptyFragShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            shader.LoadShader("", ShaderType.FragmentShader);

            Assert.IsFalse(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void NullFragShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            shader.LoadShader(null, ShaderType.FragmentShader);

            Assert.IsFalse(shader.LinkStatusIsOk);
        }
    }
}
