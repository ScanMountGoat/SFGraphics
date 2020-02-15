using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;


namespace SFGraphics.Test.TextureTests
{
    [TestClass]
    public class DepthTextureConstructor : GraphicsContextTest
    {
        [TestMethod]
        public void DepthFormat()
        {
            // Doesn't throw an exception.
            DepthTexture texture = new DepthTexture(1, 1, PixelInternalFormat.DepthComponent);
        }

        [TestMethod]
        public void NotDepthFormat()
        {
            var e = Assert.ThrowsException<ArgumentException>(() =>
                new DepthTexture(1, 1, PixelInternalFormat.Rgba));

            Assert.AreEqual("The PixelInternalFormat is not a valid depth component format.", e.Message);
        }
    }
}
