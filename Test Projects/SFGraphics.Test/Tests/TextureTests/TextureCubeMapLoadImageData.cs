using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;


namespace SFGraphics.Test.TextureTests
{
    [TestClass]
    public class TextureCubeMapLoadImageData : GraphicsContextTest
    {
        private static readonly List<byte[]> mipmaps = new List<byte[]>();

        [TestMethod]
        public void InvalidMipmapCount()
        {
            List<byte[]> mipmapsBig = new List<byte[]>
            {
                new byte[16]
            };

            var textureCubeMap = new TextureCubeMap();

            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                textureCubeMap.LoadImageData(128, InternalFormat.CompressedRgbaS3tcDxt1Ext,
                mipmaps, mipmaps, mipmaps, mipmapsBig, mipmaps, mipmaps));

            Assert.IsTrue(e.Message.Contains("Mipmap count is not equal for all faces."));
            Assert.AreEqual("mips", e.ParamName);
        }

        [TestMethod]
        public void NotCompressedInternalFormat()
        {
            var textureCubeMap = new TextureCubeMap();

            var e = Assert.ThrowsException<ArgumentException>(() =>
                textureCubeMap.LoadImageData(128, InternalFormat.Rgba,
                mipmaps, mipmaps, mipmaps, mipmaps, mipmaps, mipmaps));

            Assert.AreEqual("The InternalFormat is not a compressed image format.", e.Message);
        }

        [TestMethod]
        public void CorrectFormatSameMipmapCount()
        {
            // Will fail if exception is thrown.
            var texture = new TextureCubeMap();
            texture.LoadImageData(128, InternalFormat.CompressedRgbaS3tcDxt1Ext,
                mipmaps, mipmaps, mipmaps, mipmaps, mipmaps, mipmaps);

            Assert.AreEqual(128, texture.Width);
            Assert.AreEqual(128, texture.Height);
        }

        [TestMethod]
        public void UncompressedMipmaps()
        {
            var texture = new TextureCubeMap();
            var format = new TextureFormatUncompressed(PixelInternalFormat.Rgba32f, PixelFormat.Rgba, PixelType.Float);
            texture.LoadImageData(128, format,
                mipmaps, mipmaps, mipmaps, mipmaps, mipmaps, mipmaps);

            Assert.AreEqual(128, texture.Width);
            Assert.AreEqual(128, texture.Height);
        }

        [TestMethod]
        public void BitmapFaces()
        {
            // Will fail if exception is thrown.
            var texture = new TextureCubeMap();
            texture.LoadImageData(new Bitmap(8, 8 * 8 * 6), 8);

            Assert.AreEqual(8, texture.Width);
            Assert.AreEqual(8, texture.Height);
        }
    }
}
