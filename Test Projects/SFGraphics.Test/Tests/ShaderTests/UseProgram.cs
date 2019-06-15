using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.ShaderTests
{
    [TestClass]
    public class UseProgram : ShaderTest
    {
        [TestMethod]
        public void ValidProgram()
        {
            shader.UseProgram();
        }

        [TestMethod]
        public void InvalidProgram()
        {
            // Shouldn't throw graphics exceptions.
            var shader = new Shader();
            Assert.IsFalse(shader.LinkStatusIsOk);
            shader.UseProgram(); 
        }
    }
}
