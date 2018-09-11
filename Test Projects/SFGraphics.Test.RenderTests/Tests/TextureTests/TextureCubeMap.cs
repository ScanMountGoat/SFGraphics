using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.RenderTests.TextureTests
{
    [TestClass]
    public class TextureCubeMap
    {
        private static readonly List<byte[]> mipmaps = new List<byte[]>();

        [TestInitialize]
        public void TestSetup()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.BindDummyContext();

            // Binding a pixel unpack buffer affects texture loading methods.
            GL.BindBuffer(BufferTarget.PixelUnpackBuffer, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InvalidMipmapCount()
        {
            List<byte[]> mipmapsBig = new List<byte[]>();
            mipmapsBig.Add(new byte[16]);

            GLObjects.Textures.TextureCubeMap textureCubeMap = new GLObjects.Textures.TextureCubeMap();
            textureCubeMap.LoadImageData(128, InternalFormat.CompressedRgbaS3tcDxt1Ext,
                mipmaps, mipmaps, mipmaps, mipmapsBig, mipmaps, mipmaps);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NotCompressedInternalFormat()
        {
            GLObjects.Textures.TextureCubeMap textureCubeMap = new GLObjects.Textures.TextureCubeMap();
            textureCubeMap.LoadImageData(128, InternalFormat.Rgba,
                mipmaps, mipmaps, mipmaps, mipmaps, mipmaps, mipmaps);
        }

        [TestMethod]
        public void CorrectFormatSameMipmapCount()
        {
            // Will fail if exception is thrown.
            GLObjects.Textures.TextureCubeMap textureCubeMap = new GLObjects.Textures.TextureCubeMap();
            textureCubeMap.LoadImageData(128, InternalFormat.CompressedRgbaS3tcDxt1Ext,
                mipmaps, mipmaps, mipmaps, mipmaps, mipmaps, mipmaps);
        }
    }
}
