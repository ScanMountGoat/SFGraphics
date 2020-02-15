using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using RenderTestUtils;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class AttachShader : GraphicsContextTest
    {
        [TestMethod]
        public void ValidFragShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            string shaderSource = ResourceShaders.GetShaderSource("valid.frag");
            int id = Shader.CreateGlShader(shaderSource, ShaderType.FragmentShader);
            shader.AttachShader(id, ShaderType.FragmentShader);

            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void InvalidFragShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            string shaderSource = ResourceShaders.GetShaderSource("invalid.frag");
            int id = Shader.CreateGlShader(shaderSource, ShaderType.FragmentShader);
            shader.AttachShader(id, ShaderType.FragmentShader);

            Assert.IsFalse(shader.LinkStatusIsOk);
        }
    }
}
