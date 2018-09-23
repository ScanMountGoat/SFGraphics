using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetInt : ShaderTest
    {
        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetInt("int1", 0);
            Assert.IsTrue(IsValidSet("int1", ActiveUniformType.Int));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetInt("memes", 0);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.Int));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetInt("float1", 0);
            Assert.IsFalse(IsValidSet("float1", ActiveUniformType.Int));
        }
    }
}
