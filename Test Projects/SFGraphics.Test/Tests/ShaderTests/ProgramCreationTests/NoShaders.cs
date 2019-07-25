using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using Tests;

namespace SFGraphics.Test.ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class NoShaders : ContextTest
    {
        [TestMethod]
        public void NoAttachedShaders()
        {
            Shader shader = new Shader();
            Assert.IsFalse(shader.LinkStatusIsOk);
        }
    }
}
