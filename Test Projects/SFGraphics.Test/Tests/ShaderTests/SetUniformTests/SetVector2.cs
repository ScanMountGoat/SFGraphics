using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.ShaderTests.SetterTests
{
    [TestClass]
    public class SetVector2 : ShaderTest
    {
        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetVector2("vector2a", new Vector2(0.1f, -0.2f));
            Assert.AreEqual(new Vector2(0.1f, -0.2f), GetVector2("vector2a"));
            Assert.IsTrue(IsValidSet("vector2a", ActiveUniformType.FloatVec2));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetVector2("memes", new Vector2(1));
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.FloatVec2));
        }

        [TestMethod]
        public void FloatsValidNameValidType()
        {
            shader.SetVector2("vector2a", 0.5f, -0.75f);
            Assert.AreEqual(new Vector2(0.5f, -0.75f), GetVector2("vector2a"));
            Assert.IsTrue(IsValidSet("vector2a", ActiveUniformType.FloatVec2));
        }

        [TestMethod]
        public void FloatsInvalidName()
        {
            shader.SetVector2("memes2", 1, 1);
            Assert.IsFalse(IsValidSet("memes2", ActiveUniformType.FloatVec2));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetVector2("float1", 1, 1);
            Assert.IsFalse(IsValidSet("float1", ActiveUniformType.FloatVec2));
        }
    }
}
