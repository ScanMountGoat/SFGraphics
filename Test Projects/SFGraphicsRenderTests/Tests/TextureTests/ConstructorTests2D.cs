using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Textures;
using OpenTK.Graphics.OpenGL;


namespace SFGraphicsRenderTests.TextureTests
{
    public partial class TextureTest
    {
        [TestClass]
        public class ConstructorTests2D
        {
            private static readonly List<byte[]> mipmaps = new List<byte[]>();

            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.BindDummyContext();
            }

            [TestMethod]
            public void GenerateId()
            {
                Texture2D texture = new Texture2D(1, 1);
                Assert.AreNotEqual(0, texture.Id);
            }

            [TestMethod]
            public void CompressedTextureCorrectFormat()
            {
                // Doesn't throw an exception.
                Texture2D texture = new Texture2D(1, 1, mipmaps, InternalFormat.CompressedRg11Eac);
            }

            [TestMethod]
            public void UncompressedTextureCorrectFormat()
            {
                // Doesn't throw an exception.
                Texture2D texture = new Texture2D(1, 1, new byte[0], 5, new TextureFormatUncompressed(PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.Float));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void CompressedTextureIncorrectFormat()
            {
                Texture2D texture = new Texture2D(1, 1, mipmaps, InternalFormat.Rgb);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void UncompressedTextureIncorrectFormat()
            {
                Texture2D texture = new Texture2D(1, 1, new byte[0], 5, new TextureFormatUncompressed(PixelInternalFormat.CompressedRgbaS3tcDxt1Ext, PixelFormat.Rgba, PixelType.Float));
            }
        }
    }
}
