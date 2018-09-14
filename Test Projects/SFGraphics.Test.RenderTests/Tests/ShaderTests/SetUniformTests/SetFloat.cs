using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetFloat : SetBase
    {
        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetFloat("float1", 0);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.Float);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, eventArgs.Count);
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetFloat("int1", 0);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("int1", ActiveUniformType.Float);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, eventArgs.Count);
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetFloat("memes", 0);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.Float);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, eventArgs.Count);
        }
    }
}
