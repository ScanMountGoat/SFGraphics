using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetVector4 : SetBase
    {
        [TestMethod]
        public void ValidName()
        {
            shader.SetVector4("vector4a", new Vector4(1));
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("vector4a", ActiveUniformType.FloatVec4);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, eventArgs.Count);
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetVector4("memes", new Vector4(1));
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.FloatVec4);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, eventArgs.Count);
        }

        [TestMethod]
        public void FloatsValidName()
        {
            shader.SetVector4("vector4a", 1, 1, 1, 1);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("vector4a", ActiveUniformType.FloatVec4);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, eventArgs.Count);
        }

        [TestMethod]
        public void FloatsInvalidName()
        {
            shader.SetVector4("memes2", 1, 1, 1, 1);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("memes2", ActiveUniformType.FloatVec4);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, eventArgs.Count);
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetVector4("float1", 1, 1, 1, 1);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.FloatVec4);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, eventArgs.Count);
        }
    }
}
