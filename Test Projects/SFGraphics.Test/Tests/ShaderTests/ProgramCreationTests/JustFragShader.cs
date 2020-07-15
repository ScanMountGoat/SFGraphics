using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using RenderTestUtils;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphics.Test.ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class JustFragShader : GraphicsContextTest
    {
        [TestMethod]
        public void ValidFragShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            string shaderSource = ResourceShaders.GetShaderSource("valid.frag");
            shader.LoadShaders(new ShaderObject(shaderSource, ShaderType.FragmentShader));

            Assert.IsTrue(shader.LinkStatusIsOk);

            Assert.AreEqual(19, shader.ActiveUniformCount);
            Assert.AreEqual(0, shader.ActiveAttributeCount);
            Assert.AreEqual(1, shader.ActiveUniformBlockCount);
        }

        [TestMethod]
        public void InvalidFragShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            string shaderSource = ResourceShaders.GetShaderSource("invalid.frag");
            shader.LoadShaders(new ShaderObject(shaderSource, ShaderType.FragmentShader));

            Assert.IsFalse(shader.LinkStatusIsOk);

            Assert.AreEqual(0, shader.ActiveUniformCount);
            Assert.AreEqual(0, shader.ActiveAttributeCount);
            Assert.AreEqual(0, shader.ActiveUniformBlockCount);
        }

        [TestMethod]
        public void EmptyFragShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            shader.LoadShaders(new ShaderObject("", ShaderType.FragmentShader));

            Assert.IsFalse(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void NullFragShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            shader.LoadShaders(new ShaderObject(null, ShaderType.FragmentShader));

            Assert.IsFalse(shader.LinkStatusIsOk);
        }
    }
}
