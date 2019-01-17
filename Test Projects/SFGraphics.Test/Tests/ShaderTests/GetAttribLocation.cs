using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.ShaderTests
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
            Assert.AreEqual(-1, shader.GetAttribLocation("memes"));
        }

        [TestMethod]
        public void EmptyName()
        {
            Assert.AreEqual(-1, shader.GetAttribLocation(""));
        }

        [TestMethod]
        public void NullName()
        {
            Assert.AreEqual(-1, shader.GetAttribLocation(null));
        }

        [TestMethod]
        public void ShaderNotLinked()
        {
            var shader = new SFGraphics.GLObjects.Shaders.Shader();
            Assert.AreEqual(-1, shader.GetAttribLocation("memes"));
        }
    }
}
