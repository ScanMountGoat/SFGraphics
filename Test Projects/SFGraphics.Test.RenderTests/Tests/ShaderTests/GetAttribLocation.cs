using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests
{
    [TestClass]
    public class GetAttribLocation : ShaderTest
    {
        [TestMethod]
        public void ValidName()
        {
            Assert.AreEqual(GL.GetAttribLocation(shader.Id, "position"), shader.GetAttribLocation("position"));
        }

        [TestMethod]
        public void InvalidName()
        {
            Assert.AreEqual(GL.GetAttribLocation(shader.Id, "memes"), shader.GetAttribLocation("memes"));
        }
    }
}
