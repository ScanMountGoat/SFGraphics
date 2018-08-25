using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;

namespace SFGraphicsRenderTests.TextureTests
{
    [TestClass]
    public class LoadImageDataBufferTests
    {
        private static readonly List<BufferObject> mipmaps = new List<BufferObject>();

        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.BindDummyContext();

            // Binding a pixel unpack buffer affects texture loading methods.
            GL.BindBuffer(BufferTarget.PixelUnpackBuffer, 0);
        }

        [TestMethod]
        public void UncompressedBaseLevel()
        {
            Texture2D texture = new Texture2D();
            BufferObject pixelBuffer = new BufferObject(BufferTarget.PixelUnpackBuffer);
            pixelBuffer.SetData(new float[] { 1, 1, 1 }, sizeof(float), BufferUsageHint.StaticDraw);
            texture.LoadImageData(1, 1, pixelBuffer, 0,
                new TextureFormatUncompressed(PixelInternalFormat.Rgb, PixelFormat.Rgb, PixelType.Float));
        }

        [TestMethod]
        public void CompressedMipmaps()
        {
            // TODO: Fix this.
            Assert.Fail();
        }
    }
}
