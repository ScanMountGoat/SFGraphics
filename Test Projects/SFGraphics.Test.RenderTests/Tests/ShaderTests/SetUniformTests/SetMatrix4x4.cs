using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetMatrix4x4 : SetBase
    {
        [TestMethod]
        public void ValidNameValidType()
        {
            Matrix4 matrix4 = Matrix4.Identity;
            shader.SetMatrix4x4("matrix4a", ref matrix4);
            string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("matrix4a", ActiveUniformType.FloatMat4);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, eventArgs.Count);
        }

        [TestMethod]
        public void InvalidName()
        {
            Matrix4 matrix4 = Matrix4.Identity;
            shader.SetMatrix4x4("memes", ref matrix4);
            string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.FloatMat4);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, eventArgs.Count);
        }

        [TestMethod]
        public void InvalidType()
        {
            Matrix4 matrix4 = Matrix4.Identity;
            shader.SetMatrix4x4("float1", ref matrix4);
            string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.FloatMat4);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, eventArgs.Count);
        }
    }
}
