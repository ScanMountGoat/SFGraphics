using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetVector3 : ShaderTest
    {
        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetVector3("vector3a", new Vector3(-1, 0.5f, 1));
            Assert.AreEqual(new Vector3(-1, 0.5f, 1), GetVector3("vector3a"));
            Assert.IsTrue(IsValidSet("vector3", ActiveUniformType.FloatVec3));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetVector3("memes", new Vector3(1));
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.FloatVec3));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetVector3("float1", 1, 1, 1);
            Assert.IsFalse(IsValidSet("float1", ActiveUniformType.FloatVec3));
        }

        [TestMethod]
        public void FloatsValidNameValidType()
        {
            shader.SetVector3("vector3a", 1, 2, -2.5f);
            Assert.AreEqual(new Vector3(1, 2, -2.5f), GetVector3("vector3a"));
            Assert.IsTrue(IsValidSet("vector3a", ActiveUniformType.FloatVec3));
        }

        [TestMethod]
        public void FloatsInvalidName()
        {
            shader.SetVector3("memes2", 1, 1, 1);
            Assert.IsFalse(IsValidSet("memes2", ActiveUniformType.FloatVec3));
        }
    }
}
