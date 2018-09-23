using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetVector3Array : ShaderTest
    {
        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetVector3("vector3Arr", new Vector3[8]);
            Assert.IsTrue(IsValidSet("vector3Arr", ActiveUniformType.FloatVec3));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetVector3("memes", new Vector3[8]);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.FloatVec3));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetVector3("vector2Arr", new Vector3[8]);
            Assert.IsFalse(IsValidSet("vector2Arr", ActiveUniformType.FloatVec3));
        }
    }
}
