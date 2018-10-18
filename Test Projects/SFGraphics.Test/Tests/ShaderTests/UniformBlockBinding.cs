using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests
{
    [TestClass]
    public class UniformBlockBinding : ShaderTest
    {
        [TestMethod]
        public void ValidName()
        {
            shader.UniformBlockBinding("UniformBlock", 3);
            GL.GetActiveUniformBlock(shader.Id,
                shader.GetUniformBlockIndex("UniformBlock"),
                ActiveUniformBlockParameter.UniformBlockBinding, out int binding);
            Assert.AreEqual(3, binding);
        }

        [TestMethod]
        public void InvalidName()
        {
            // Shouldn't throw graphics exceptions.
            shader.UniformBlockBinding("memes", 3);
        }


        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void InvalidBinding()
        {
            // Shouldn't throw graphics exceptions.
            shader.UniformBlockBinding("UniformBlock", -1);
        }
    }
}
