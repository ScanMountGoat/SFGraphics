using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using RenderTestUtils;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class ValidFragInvalidVert : GraphicsContextTest
    {
        [TestMethod]
        public void ValidFragInvalidVertShader()
        {
            Shader shader = new Shader();

            // Load the shader files from the embedded resources.
            string fragSource = ResourceShaders.GetShaderSource("valid.frag");
            shader.LoadShader(fragSource, ShaderType.FragmentShader);
            // Force an update of compilation/link status.
            Assert.IsTrue(shader.LinkStatusIsOk);

            // Make sure the compilation/link status still updates.
            string vertSource = ResourceShaders.GetShaderSource("invalid.vert");
            shader.LoadShader(vertSource, ShaderType.VertexShader);
            Assert.IsFalse(shader.LinkStatusIsOk);
        }
    }
}
