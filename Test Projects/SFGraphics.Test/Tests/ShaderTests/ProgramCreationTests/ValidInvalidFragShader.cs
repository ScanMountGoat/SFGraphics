using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class ValidInvalidFragShader
    {
        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void ValidAndInvalidFragShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            string shaderSource = RenderTestUtils.ResourceShaders.GetShaderSource("valid.frag");
            shader.LoadShader(shaderSource, ShaderType.FragmentShader);

            string shaderSource2 = RenderTestUtils.ResourceShaders.GetShaderSource("invalid.frag");
            shader.LoadShader(shaderSource2, ShaderType.FragmentShader);

            Assert.IsFalse(shader.LinkStatusIsOk);
        }
    }
}
