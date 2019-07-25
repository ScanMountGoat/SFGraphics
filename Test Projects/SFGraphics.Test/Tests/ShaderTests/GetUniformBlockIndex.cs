using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.ShaderTests
{
    [TestClass]
    public class GetUniformBlockIndex : ShaderTest
    {
        [TestMethod]
        public void ValidName()
        {
            Assert.AreEqual(GL.GetUniformBlockIndex(shader.Id, "UniformBlock"), shader.GetUniformBlockIndex("UniformBlock"));
        }

        [TestMethod]
        public void InvalidName()
        {
            Assert.AreEqual(-1, shader.GetUniformBlockIndex("memes"));
        }


        [TestMethod]
        public void EmptyName()
        {
            Assert.AreEqual(-1, shader.GetUniformBlockIndex(""));
        }

        [TestMethod]
        public void NullName()
        {
            Assert.AreEqual(-1, shader.GetUniformBlockIndex(null));
        }

        [TestMethod]
        public void ShaderNotLinked()
        {
            var shader = new Shader();
            Assert.AreEqual(-1, shader.GetUniformBlockIndex("memes"));
        }
    }
}
