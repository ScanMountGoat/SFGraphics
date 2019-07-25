using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;
using Tests;

namespace SFGraphics.Test.TextureTests
{
    [TestClass]
    public class Texture2DMsConstructor : ContextTest
    {
        [TestMethod]
        public void ValidSampleCount()
        {
            // Doesn't throw an exception.
            var texture = new Texture2DMultisample(64, 16, PixelInternalFormat.Rgb, 1);
        }

        [TestMethod]
        public void ZeroSamples()
        {
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new Texture2DMultisample(64, 16, PixelInternalFormat.Rgb, 0));

            Assert.IsTrue(e.Message.Contains("Sample count must be greater than 0"));
            Assert.AreEqual("samples", e.ParamName);
        }

        [TestMethod]
        public void NegativeSamples()
        {
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new Texture2DMultisample(64, 16, PixelInternalFormat.Rgb, -1));

            Assert.IsTrue(e.Message.Contains("Sample count must be greater than 0"));
            Assert.AreEqual("samples", e.ParamName);
        }
    }
}
