using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetFloatArray : ShaderTest
    {
        private float[] values = new float[] { 1.5f, 2.5f, 3.5f };

        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetFloat("floatArray1", values);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("floatArray1", ActiveUniformType.Float);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, invalidUniformSets.Count);
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetFloat("intArray1", values);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("intArray1", ActiveUniformType.Float);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetFloat("memesArray", values);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("memesArray", ActiveUniformType.Float);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }
    }
}
