using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.ShaderTests.SetterTests
{
    [TestClass]
    public class SetFloat : ShaderTest
    {
        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetFloat("float1", 1);
            Assert.AreEqual(1, GetFloat("float1"));
            Assert.IsTrue(IsValidSet("float1", ActiveUniformType.Float));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetFloat("int1", 0);
            Assert.IsFalse(IsValidSet("int1", ActiveUniformType.Float));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetFloat("memes", 0.5f);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.Float));
        }
    }
}
