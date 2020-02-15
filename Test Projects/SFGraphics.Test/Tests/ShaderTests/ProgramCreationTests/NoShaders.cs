using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphics.Test.ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class NoShaders : GraphicsContextTest
    {
        [TestMethod]
        public void NoAttachedShaders()
        {
            Shader shader = new Shader();
            Assert.IsFalse(shader.LinkStatusIsOk);
        }
    }
}
