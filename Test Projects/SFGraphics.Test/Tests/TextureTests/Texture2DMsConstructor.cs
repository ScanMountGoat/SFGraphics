using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;

namespace TextureTests
{
    [TestClass]
    public class Texture2DMsConstructor : Tests.ContextTest
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
            var e = Assert.ThrowsException<System.ArgumentOutOfRangeException>(() =>
                new Texture2DMultisample(64, 16, PixelInternalFormat.Rgb, 0));
        }

        [TestMethod]
        public void NegativeSamples()
        {
            var e = Assert.ThrowsException<System.ArgumentOutOfRangeException>(() =>
                new Texture2DMultisample(64, 16, PixelInternalFormat.Rgb, -1));
        }
    }
}
