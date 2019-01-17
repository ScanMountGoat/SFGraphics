using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Textures;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.TextureTests
{
    [TestClass]
    public class DepthTetureConstructor : Tests.ContextTest
    {
        private readonly List<byte[]> mipmaps = new List<byte[]>();

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
        }
    }
}
