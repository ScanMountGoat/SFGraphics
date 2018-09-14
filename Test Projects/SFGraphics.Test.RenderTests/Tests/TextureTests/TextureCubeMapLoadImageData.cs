using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace TextureTests
{
    [TestClass]
    public class TextureCubeMapLoadImageData : Tests.ContextTest
    {
        private static readonly List<byte[]> mipmaps = new List<byte[]>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InvalidMipmapCount()
        {
            List<byte[]> mipmapsBig = new List<byte[]>();
            mipmapsBig.Add(new byte[16]);

            var textureCubeMap = new SFGraphics.GLObjects.Textures.TextureCubeMap();
            textureCubeMap.LoadImageData(128, InternalFormat.CompressedRgbaS3tcDxt1Ext,
                mipmaps, mipmaps, mipmaps, mipmapsBig, mipmaps, mipmaps);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NotCompressedInternalFormat()
        {
            var textureCubeMap = new SFGraphics.GLObjects.Textures.TextureCubeMap();
            textureCubeMap.LoadImageData(128, InternalFormat.Rgba,
                mipmaps, mipmaps, mipmaps, mipmaps, mipmaps, mipmaps);
        }

        [TestMethod]
        public void CorrectFormatSameMipmapCount()
        {
            // Will fail if exception is thrown.
            var textureCubeMap = new SFGraphics.GLObjects.Textures.TextureCubeMap();
            textureCubeMap.LoadImageData(128, InternalFormat.CompressedRgbaS3tcDxt1Ext,
                mipmaps, mipmaps, mipmaps, mipmaps, mipmaps, mipmaps);
        }
    }
}
