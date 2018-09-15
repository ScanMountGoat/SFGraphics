using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetTexture : ShaderTest
    {
        [TestMethod]
        public void ValidNameValidTarget()
        {
            shader.SetTexture("tex2D", new Texture2D(), 0);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("tex2D", ActiveUniformType.Sampler2D);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, invalidUniformSets.Count);
            Assert.AreEqual(0, invalidTextureSets.Count);
        }

        [TestMethod]
        public void MoreThanOneTypePerTextureUnit()
        {
            shader.SetTexture("tex2D", new Texture2D(), 0);
            shader.SetTexture("texCube", new TextureCubeMap(), 0);

            Assert.AreEqual(0, invalidUniformSets.Count);
            Assert.AreEqual(1, invalidTextureSets.Count);
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetTexture("memes", new Texture2D(), 0);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.Sampler2D);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetTexture("float1", new Texture2D(), 0);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.Sampler2D);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }

        [TestMethod]
        public void InvalidTarget()
        {
            shader.SetTexture("texCube", new Texture2D(), 0);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("texCube", ActiveUniformType.Sampler2D);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }
    }
}
