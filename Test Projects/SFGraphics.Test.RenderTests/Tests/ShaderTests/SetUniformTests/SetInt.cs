using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetInt : SetBase
    {
        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetInt("int1", 0);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("int1", ActiveUniformType.Int);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, eventArgs.Count);
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetInt("memes", 0);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.Int);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, eventArgs.Count);
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetInt("float1", 0);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.Int);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, eventArgs.Count);
        }
    }
}
