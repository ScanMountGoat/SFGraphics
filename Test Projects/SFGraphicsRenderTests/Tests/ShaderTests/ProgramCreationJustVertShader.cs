using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphicsRenderTests.ShaderTests.ProgramCreationTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class ProgramCreationJustVertShader
        {
            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.BindDummyContext();
            }

            [TestMethod]
            public void ValidVertShader()
            {
                // Load the shader file from the embedded resources.
                Shader shader = new Shader();
                string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphicsRenderTests.Shaders.validVert.vert");
                shader.LoadShader(shaderSource, ShaderType.VertexShader);

                Assert.IsTrue(shader.ProgramCreatedSuccessfully);
            }

            [TestMethod]
            public void InvalidVertShader()
            {
                // Load the shader file from the embedded resources.
                Shader shader = new Shader();
                string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphicsRenderTests.Shaders.invalidVert.vert");
                shader.LoadShader(shaderSource, ShaderType.VertexShader);

                Assert.IsFalse(shader.ProgramCreatedSuccessfully);
            }
        }
    }
}
