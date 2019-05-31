using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures.TextureFormats;
using SFGraphics.GLObjects.Textures;

namespace SFGraphics.Test.TextureTests
{
    [TestClass]
    public class TextureCubeMapLoadImageData : Tests.ContextTest
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
        }

        [TestMethod]
        public void NotCompressedInternalFormat()
        {
            var textureCubeMap = new TextureCubeMap();

            var e = Assert.ThrowsException<ArgumentException>(() =>
                textureCubeMap.LoadImageData(128, InternalFormat.Rgba,
                mipmaps, mipmaps, mipmaps, mipmaps, mipmaps, mipmaps));
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
            texture.LoadImageData(128, InternalFormat.CompressedRgbaS3tcDxt1Ext,
                mipmaps, mipmaps, mipmaps, mipmaps, mipmaps, mipmaps);

            Assert.AreEqual(128, texture.Width);
            Assert.AreEqual(128, texture.Height);
        }

        [TestMethod]
        public void BitmapFaces()
        {
            // Will fail if exception is thrown.
            var texture = new TextureCubeMap();
            texture.LoadImageData(new System.Drawing.Bitmap(8, 8 * 8 * 6), 8);

            Assert.AreEqual(8, texture.Width);
            Assert.AreEqual(8, texture.Height);
        }
    }
}
