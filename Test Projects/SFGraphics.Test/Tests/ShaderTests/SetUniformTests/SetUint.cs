using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetUint : ShaderTest
    {
        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetUint("uint1", 1);
            Assert.IsTrue(IsValidSet("uint1", ActiveUniformType.UnsignedInt));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetUint("memes", 0);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.UnsignedInt));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetUint("float1", 0);
            Assert.IsFalse(IsValidSet("float1", ActiveUniformType.UnsignedInt));
        }
    }
}
