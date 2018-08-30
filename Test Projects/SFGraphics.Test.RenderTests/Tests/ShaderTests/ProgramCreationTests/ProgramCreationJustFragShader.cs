using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphics.Test.RenderTests.ShaderTests.ProgramCreationTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class ProgramCreationJustFragShader
        {
            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.BindDummyContext();
            }

            [TestMethod]
            public void ValidFragShader()
            {
                // Load the shader file from the embedded resources.
                Shader shader = new Shader();
                string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphics.Test.RenderTests.Shaders.validFrag.frag");
                shader.LoadShader(shaderSource, ShaderType.FragmentShader);

                Assert.IsTrue(shader.ProgramCreatedSuccessfully);
            }

            [TestMethod]
            public void InvalidFragShader()
            {
                // Load the shader file from the embedded resources.
                Shader shader = new Shader();
                string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphics.Test.RenderTests.Shaders.invalidFrag.frag");
                shader.LoadShader(shaderSource, ShaderType.FragmentShader);

                Assert.IsFalse(shader.ProgramCreatedSuccessfully);
            }
        }
    }
}
