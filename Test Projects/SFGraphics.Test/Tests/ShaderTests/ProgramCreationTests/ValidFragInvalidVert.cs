using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class ValidFragInvalidVert
    {
        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void ValidFragInvalidVertShader()
        {
            Shader shader = new Shader();

            // Load the shader files from the embedded resources.
            string fragSource = RenderTestUtils.ResourceShaders.GetShaderSource("validFrag.frag");
            shader.LoadShader(fragSource, ShaderType.FragmentShader);
            // Force an update of compilation/link status.
            Assert.IsTrue(shader.LinkStatusIsOk);

            // Make sure the compilation/link status still updates.
            string vertSource = RenderTestUtils.ResourceShaders.GetShaderSource("invalidVert.vert");
            shader.LoadShader(vertSource, ShaderType.VertexShader);
            Assert.IsFalse(shader.LinkStatusIsOk);
        }
    }
}
