using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;

namespace SFGraphicsRenderTests.TextureTests
{
    public partial class TextureTest
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
            }

            [TestMethod]
            public void Bitmap()
            {
                Texture2D texture = new Texture2D();
                texture.LoadImageData(new System.Drawing.Bitmap(128, 64));

                Assert.AreEqual(128, texture.Width);
                Assert.AreEqual(64, texture.Height);
            }

            [TestMethod]
            public void CompressedTextureCorrectFormat()
            {
                // Doesn't throw an exception.
                Texture2D texture = new Texture2D();
                texture.LoadImageData(128, 64, mipmaps, InternalFormat.CompressedRg11Eac);

                Assert.AreEqual(128, texture.Width);
                Assert.AreEqual(64, texture.Height);
            }

            [TestMethod]
            public void UncompressedTextureCorrectFormat()
            {
                // Doesn't throw an exception.
                Texture2D texture = new Texture2D();
                texture.LoadImageData(128, 64, new byte[0], 5, 
                    new TextureFormatUncompressed(PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.Float));

                Assert.AreEqual(128, texture.Width);
                Assert.AreEqual(64, texture.Height);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void CompressedTextureIncorrectFormat()
            {
                Texture2D texture = new Texture2D();
                texture.LoadImageData(128, 64, mipmaps, InternalFormat.Rgb);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void UncompressedTextureIncorrectFormat()
            {
                Texture2D texture = new Texture2D();
                texture.LoadImageData(128, 64, new byte[0], 5, 
                    new TextureFormatUncompressed(PixelInternalFormat.CompressedRgbaS3tcDxt1Ext, PixelFormat.Rgba, PixelType.Float));
            }
        }
    }
}
