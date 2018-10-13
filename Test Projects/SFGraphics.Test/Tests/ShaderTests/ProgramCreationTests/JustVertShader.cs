using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class JustVertShader
    {
        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void ValidVertShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            string shaderSource = RenderTestUtils.ResourceShaders.GetShaderSource("valid.vert");
            shader.LoadShader(shaderSource, ShaderType.VertexShader);

            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void InvalidVertShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            string shaderSource = RenderTestUtils.ResourceShaders.GetShaderSource("invalid.vert");
            shader.LoadShader(shaderSource, ShaderType.VertexShader);

            Assert.IsFalse(shader.LinkStatusIsOk);
        }
    }
}
