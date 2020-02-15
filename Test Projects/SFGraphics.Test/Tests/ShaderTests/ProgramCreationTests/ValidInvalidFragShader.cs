using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using RenderTestUtils;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class ValidInvalidFragShader : GraphicsContextTest
    {
        [TestMethod]
        public void ValidAndInvalidFragShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            string shaderSource = ResourceShaders.GetShaderSource("valid.frag");
            shader.LoadShader(shaderSource, ShaderType.FragmentShader);

            string shaderSource2 = ResourceShaders.GetShaderSource("invalid.frag");
            shader.LoadShader(shaderSource2, ShaderType.FragmentShader);

            Assert.IsFalse(shader.LinkStatusIsOk);
        }
    }
}
