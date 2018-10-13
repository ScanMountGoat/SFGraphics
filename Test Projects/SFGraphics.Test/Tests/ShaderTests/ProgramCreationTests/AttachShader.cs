using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class AttachShader
    {
        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void ValidFragShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            string shaderSource = RenderTestUtils.ResourceShaders.GetShaderSource("valid.frag");
            int id = Shader.CreateGlShader(shaderSource, ShaderType.FragmentShader);
            shader.AttachShader(id, ShaderType.FragmentShader);

            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void InvalidFragShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            string shaderSource = RenderTestUtils.ResourceShaders.GetShaderSource("invalid.frag");
            int id = Shader.CreateGlShader(shaderSource, ShaderType.FragmentShader);
            shader.AttachShader(id, ShaderType.FragmentShader);

            Assert.IsFalse(shader.LinkStatusIsOk);
        }
    }
}
