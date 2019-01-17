using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.ShaderTests.SetterTests
{
    [TestClass]
    public class SetVector4Array : ShaderTest
    {
        [TestMethod]
        public void ValidName()
        {
            shader.SetVector4("vector4Arr", new Vector4[8]);
            Assert.IsTrue(IsValidSet("vector4Arr", ActiveUniformType.FloatVec4));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetVector4("memes", new Vector4[8]);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.FloatVec4));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetVector4("vector2Arr", new Vector4[8]);
            Assert.IsFalse(IsValidSet("vector2Arr", ActiveUniformType.FloatVec4));
        }
    }
}
