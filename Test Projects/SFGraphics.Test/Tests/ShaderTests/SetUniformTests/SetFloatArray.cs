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
            Assert.IsTrue(IsValidSet("floatArray1", ActiveUniformType.Float));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetFloat("intArray1", values);
            Assert.IsFalse(IsValidSet("intArray1", ActiveUniformType.Float));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetFloat("memesArray", values);
            Assert.IsFalse(IsValidSet("memesArray", ActiveUniformType.Float));
        }
    }
}
