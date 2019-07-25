using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;
using Tests;

namespace SFGraphics.Test.TextureTests
{
    [TestClass]
    public class LoadImageData : ContextTest
    {
        private readonly List<byte[]> mipmaps = new List<byte[]>();
        private Texture2D texture;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            if (texture == null)
                texture = new Texture2D();
        }

        [TestMethod]
        public void Bitmap()
        {
            using (var bmp = new Bitmap(4, 2))
            {
                texture.LoadImageData(bmp);

                Assert.AreEqual(4, texture.Width);
                Assert.AreEqual(2, texture.Height);
            }
        }

        [TestMethod]
        public void CompressedCorrectFormat()
        {
            // Doesn't throw an exception.
            texture.LoadImageData(128, 64, mipmaps, InternalFormat.CompressedRg11Eac);

            Assert.AreEqual(128, texture.Width);
            Assert.AreEqual(64, texture.Height);
        }

        [TestMethod]
        public void UncompressedCorrectFormat()
        {
            // Doesn't throw an exception.
            texture.LoadImageData(128, 64, new byte[0], new TextureFormatUncompressed(PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.Float));

            Assert.AreEqual(128, texture.Width);
            Assert.AreEqual(64, texture.Height);
        }

        [TestMethod]
        public void CompressedIncorrectFormat()
        {
            var e = Assert.ThrowsException<ArgumentException>(() =>
                texture.LoadImageData(128, 64, mipmaps, InternalFormat.Rgb));

            Assert.AreEqual("The InternalFormat is not a compressed image format.", e.Message);
        }

        [TestMethod]
        public void CompressedGenericFormat()
        {
            var e = Assert.ThrowsException<NotSupportedException>(() =>
                texture.LoadImageData(128, 64, mipmaps, InternalFormat.CompressedRed));

            Assert.AreEqual("Generic compressed formats are not supported.", e.Message);
        }

        [TestMethod]
        public void UncompressedIncorrectFormat()
        {
            var e = Assert.ThrowsException<ArgumentException>(() =>
                texture.LoadImageData(128, 64, new byte[0], 
                    new TextureFormatUncompressed(PixelInternalFormat.CompressedRgbaS3tcDxt1Ext, PixelFormat.Rgba, PixelType.Float)));

            Assert.AreEqual("The PixelInternalFormat is not an uncompressed image format.", e.Message);
        }
    }
}
