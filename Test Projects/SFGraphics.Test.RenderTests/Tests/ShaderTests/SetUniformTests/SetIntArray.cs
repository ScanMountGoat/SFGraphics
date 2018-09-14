using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetIntArray : SetBase
    {
        private int[] values = new int[] { -1, 0, 1 };

        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetInt("intArray1", values);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("intArray1", ActiveUniformType.Int);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, eventArgs.Count);
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetInt("memes", values);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.Int);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, eventArgs.Count);
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetInt("float1", values);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.Int);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, eventArgs.Count);
        }
    }
}
