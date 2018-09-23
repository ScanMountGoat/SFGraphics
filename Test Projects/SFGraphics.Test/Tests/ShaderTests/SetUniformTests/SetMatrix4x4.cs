using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetMatrix4x4 : ShaderTest
    {
        [TestMethod]
        public void ValidNameValidType()
        {
            Matrix4 matrix4 = Matrix4.Identity;
            shader.SetMatrix4x4("matrix4a", ref matrix4);
            Assert.IsTrue(IsValidSet("matrix4a", ActiveUniformType.FloatMat4));
        }

        [TestMethod]
        public void InvalidName()
        {
            Matrix4 matrix4 = Matrix4.Identity;
            shader.SetMatrix4x4("memes", ref matrix4);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.FloatMat4));
        }

        [TestMethod]
        public void InvalidType()
        {
            Matrix4 matrix4 = Matrix4.Identity;
            shader.SetMatrix4x4("float1", ref matrix4);
            Assert.IsFalse(IsValidSet("float1", ActiveUniformType.FloatMat4));
        }
    }
}
