using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.RenderTests.ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class NoShaders
    {
        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void NoAttachedShaders()
        {
            Shader shader = new Shader();
            Assert.IsFalse(shader.LinkStatusIsOk);
        }
    }
}
