using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetVector2Array : ShaderTest
    {
        [TestMethod]
        public void ValidName()
        {
            shader.SetVector2("vector2Arr", new Vector2[8]);
            Assert.IsTrue(IsValidSet("vector2Arr", ActiveUniformType.FloatVec2));

        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetVector2("memes", new Vector2[8]);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.FloatVec2));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetVector2("vector4Arr", new Vector2[8]);
            Assert.IsFalse(IsValidSet("vector4Arr", ActiveUniformType.FloatVec2));

        }
    }
}
