using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Textures;
using OpenTK.Graphics.OpenGL;

namespace TextureTests
{
    [TestClass]
    public class ConstructorTestsDepth : Tests.ContextTest
    {
        private readonly List<byte[]> mipmaps = new List<byte[]>();

        [TestMethod]
        public void DepthFormat()
        {
            // Doesn't throw an exception.
            DepthTexture texture = new DepthTexture(1, 1, PixelInternalFormat.DepthComponent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NotDepthFormat()
        {
            DepthTexture texture = new DepthTexture(1, 1, PixelInternalFormat.Rgba);
        }
    }
}
