using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;

namespace SFGraphics.Test.RenderTests.TextureTests
{
    [TestClass]
    public class LoadImageData2DTests
    {
        private static readonly List<byte[]> mipmaps = new List<byte[]>();

        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.BindDummyContext();

            // Binding a pixel unpack buffer affects texture loading methods.
            GL.BindBuffer(BufferTarget.PixelUnpackBuffer, 0);
        }

        [TestMethod]
        public void Bitmap()
        {
            using (var bmp = new System.Drawing.Bitmap(32, 16))
            {
                Texture2D texture = new Texture2D();
                texture.LoadImageData(bmp);

                Assert.AreEqual(32, texture.Width);
                Assert.AreEqual(16, texture.Height);
            }
        }

        [TestMethod]
        public void CompressedCorrectFormat()
        {
            // Doesn't throw an exception.
            Texture2D texture = new Texture2D();
            texture.LoadImageData(128, 64, mipmaps, InternalFormat.CompressedRg11Eac);

            Assert.AreEqual(128, texture.Width);
            Assert.AreEqual(64, texture.Height);
        }

        [TestMethod]
        public void UncompressedCorrectFormat()
        {
            // Doesn't throw an exception.
            Texture2D texture = new Texture2D();
            texture.LoadImageData(128, 64, new byte[0], new TextureFormatUncompressed(PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.Float));

            Assert.AreEqual(128, texture.Width);
            Assert.AreEqual(64, texture.Height);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompressedIncorrectFormat()
        {
            Texture2D texture = new Texture2D();
            texture.LoadImageData(128, 64, mipmaps, InternalFormat.Rgb);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CompressedGenericFormat()
        {
            Texture2D texture = new Texture2D();
            texture.LoadImageData(128, 64, mipmaps, InternalFormat.CompressedRed);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UncompressedIncorrectFormat()
        {
            Texture2D texture = new Texture2D();
            texture.LoadImageData(128, 64, new byte[0], new TextureFormatUncompressed(PixelInternalFormat.CompressedRgbaS3tcDxt1Ext, PixelFormat.Rgba, PixelType.Float));
        }
    }
}
