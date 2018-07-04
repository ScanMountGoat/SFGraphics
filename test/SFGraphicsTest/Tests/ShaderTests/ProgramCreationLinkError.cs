using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphicsTest.ShaderTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class ProgramCreationLinkError
        {
            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.CreateDummyContext();
            }

            [TestMethod]
            [TestCategory("UnsafeRendering")]
            public void LinkError()
            {
                Shader shader = new Shader();

                // The shader declared but does not define a function.
                string fragSource = TestTools.ResourceShaders.GetShader("SFGraphicsTest.Shaders.linkError.frag");
                shader.LoadShader(fragSource, ShaderType.FragmentShader);
                Assert.IsFalse(shader.ProgramCreatedSuccessfully());
            }
        }
    }
}
