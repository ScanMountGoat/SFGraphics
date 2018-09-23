using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetVector4Array : ShaderTest
    {
        [TestMethod]
        public void ValidName()
        {
            shader.SetVector4("vector4Arr", new Vector4[8]);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("vector4Arr", ActiveUniformType.FloatVec4);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, invalidUniformSets.Count);
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetVector4("memes", new Vector4[8]);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.FloatVec4);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetVector4("vec2Arr", new Vector4[8]);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("vec2Arr", ActiveUniformType.FloatVec4);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }
    }
}
