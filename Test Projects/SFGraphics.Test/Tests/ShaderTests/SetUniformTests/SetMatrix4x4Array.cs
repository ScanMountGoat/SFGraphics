using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetMatrix4x4Arrays : ShaderTest
    {
        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetMatrix4x4("matrix4Arr", new Matrix4[8]);
            Assert.IsTrue(IsValidSet("matrix4Arr", ActiveUniformType.FloatMat4));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetMatrix4x4("memes", new Matrix4[8]);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.FloatMat4));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetMatrix4x4("vector2Arr", new Matrix4[8]);
            Assert.IsFalse(IsValidSet("vector2Arr", ActiveUniformType.FloatMat4));
        }
    }
}
