using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.ShaderTests.SetterTests
{
    [TestClass]
    public class SetIntArray : ShaderTest
    {
        private int[] values = new int[] { -1, 0, 1 };

        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetInt("intArray1", values);
            Assert.IsTrue(IsValidSet("intArray1", ActiveUniformType.Int));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetInt("memes", values);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.Int));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetInt("float1", values);
            Assert.IsFalse(IsValidSet("float1", ActiveUniformType.Int));
        }
    }
}
