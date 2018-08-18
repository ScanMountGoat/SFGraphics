using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphicsRenderTests.ShaderTests.ProgramCreationTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class ProgramCreationNoShaders
        {
            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.BindDummyContext();
            }

            [TestMethod]
            public void NoShaders()
            {
                Shader shader = new Shader();
                Assert.IsFalse(shader.ProgramCreatedSuccessfully);
            }
        }
    }
}
