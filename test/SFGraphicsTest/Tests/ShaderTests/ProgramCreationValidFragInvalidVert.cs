using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphicsTest.ShaderTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class ProgramCreationValidFragInvalidVert
        {
            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.CreateDummyContext();
            }

            [TestMethod]
            public void ValidFragInvalidVert()
            {
                Shader shader = new Shader();

                // Load the shader files from the embedded resources.
                string fragSource = TestTools.ResourceShaders.GetShader("SFGraphicsTest.Shaders.validFrag.frag");
                shader.LoadShader(fragSource, ShaderType.FragmentShader);
                // Force an update of compilation/link status.
                Assert.IsTrue(shader.ProgramCreatedSuccessfully());

                // Make sure the compilation/link status still updates.
                string vertSource = TestTools.ResourceShaders.GetShader("SFGraphicsTest.Shaders.invalidVert.vert");
                shader.LoadShader(vertSource, ShaderType.VertexShader);
                Assert.IsFalse(shader.ProgramCreatedSuccessfully());
            }
        }
    }
}
