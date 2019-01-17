using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.ShaderTests.SetterTests
{
    [TestClass]
    public class SetUintArray : ShaderTest
    {
        uint[] values = new uint[] { 1, 2, 3 };

        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetUint("uintArray1", values);
            Assert.IsTrue(IsValidSet("uintArray1", ActiveUniformType.UnsignedInt));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetUint("memes", values);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.UnsignedInt));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetUint("floatArray1", values);
            Assert.IsFalse(IsValidSet("floatArray1", ActiveUniformType.UnsignedInt));
        }
    }
}
