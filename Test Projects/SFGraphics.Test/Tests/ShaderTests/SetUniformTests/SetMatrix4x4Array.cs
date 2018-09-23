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
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("matrix4a", ActiveUniformType.FloatMat4);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, invalidUniformSets.Count);
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetMatrix4x4("memes", new Matrix4[8]);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.FloatMat4);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetMatrix4x4("float1", new Matrix4[8]);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.FloatMat4);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }
    }
}
