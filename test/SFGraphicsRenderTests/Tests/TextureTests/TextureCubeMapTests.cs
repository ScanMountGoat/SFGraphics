using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Textures;
using OpenTK.Graphics.OpenGL;

namespace SFGraphicsRenderTests.Tests.TextureTests
{
    [TestClass]
    public class TextureCubeMapTests
    {
        private static readonly List<byte[]> mipmaps = new List<byte[]>();

        [TestInitialize]
        public void TestSetup()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InvalidMipmapCount()
        {
            List<byte[]> mipmapsBig = new List<byte[]>();
            mipmapsBig.Add(new byte[16]);

            TextureCubeMap textureCubeMap = new TextureCubeMap(128, 128, InternalFormat.CompressedRgbaS3tcDxt1Ext, 
                mipmaps, mipmaps, mipmaps, mipmapsBig, mipmaps, mipmaps);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NotCompressedInternalFormat()
        {
            TextureCubeMap textureCubeMap = new TextureCubeMap(128, 128, InternalFormat.Rgba, 
                mipmaps, mipmaps, mipmaps, mipmaps, mipmaps, mipmaps);
        }

        [TestMethod]
        public void CorrectFormatSameMipmapCount()
        {
            // Will fail if exception is thrown.
            TextureCubeMap textureCubeMap = new TextureCubeMap(128, 128, InternalFormat.CompressedRgbaS3tcDxt1Ext,
                mipmaps, mipmaps, mipmaps, mipmaps, mipmaps, mipmaps);
        }
    }
}
