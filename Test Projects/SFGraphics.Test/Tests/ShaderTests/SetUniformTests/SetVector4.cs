using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetVector4 : ShaderTest
    {
        [TestMethod]
        public void ValidName()
        {
            shader.SetVector4("vector4a", new Vector4(1));
            Assert.IsTrue(IsValidSet("vector4a", ActiveUniformType.FloatVec4));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetVector4("memes", new Vector4(1));
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.FloatVec4));
        }

        [TestMethod]
        public void FloatsValidName()
        {
            shader.SetVector4("vector4a", 1, 1, 1, 1);
            Assert.IsTrue(IsValidSet("vector4a", ActiveUniformType.FloatVec4));
        }

        [TestMethod]
        public void FloatsInvalidName()
        {
            shader.SetVector4("memes2", 1, 1, 1, 1);
            Assert.IsFalse(IsValidSet("memes2", ActiveUniformType.FloatVec4));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetVector4("float1", 1, 1, 1, 1);
            Assert.IsFalse(IsValidSet("float1", ActiveUniformType.FloatVec4));
        }
    }
}
