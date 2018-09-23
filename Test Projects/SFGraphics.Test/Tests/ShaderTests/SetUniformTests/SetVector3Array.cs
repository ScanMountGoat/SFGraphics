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
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("vector3Arr", ActiveUniformType.FloatVec3);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, invalidUniformSets.Count);
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetVector3("memes", new Vector3[8]);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.FloatVec3);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetVector3("vector2Arr", new Vector3[8]);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("vector2Arr", ActiveUniformType.FloatVec3);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }
    }
}
