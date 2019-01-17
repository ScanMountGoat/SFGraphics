using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;

namespace SFGraphics.Test.ShaderTests.SetterTests
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
            var cube = new TextureCubeMap();
            shader.SetTexture("texCube", cube, 0);

            Assert.AreEqual(0, invalidUniformSets.Count);

            Assert.AreEqual(1, invalidTextureSets.Count);
            Assert.AreEqual("texCube", invalidTextureSets[0].Name);
            Assert.AreEqual(0, invalidTextureSets[0].TextureUnit);
            Assert.AreEqual(ActiveUniformType.SamplerCube, invalidTextureSets[0].Type);
            Assert.AreEqual(cube, invalidTextureSets[0].Value);
        }

        [TestMethod]
        public void MoreThanOneTypePerTextureUnitAndInvalidSet()
        {
            shader.SetTexture("tex2D", new Texture2D(), 0);
            shader.SetTexture("tex2D", new TextureCubeMap(), 0);

            // Ensure both events can be invoked to aid with debugging.
            Assert.AreEqual(1, invalidUniformSets.Count);
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
