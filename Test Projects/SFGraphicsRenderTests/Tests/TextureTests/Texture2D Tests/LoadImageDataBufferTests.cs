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
            pixelBuffer.SetData(new float[] { 1, 1, 1 }, BufferUsageHint.StaticDraw);
            texture.LoadImageData(1, 1, pixelBuffer, new TextureFormatUncompressed(PixelInternalFormat.Rgb, PixelFormat.Rgb, PixelType.Float));
        }

        [TestMethod]
        public void UncompressedMipmaps()
        {
            Assert.Fail("Not implemented");
        }

        [TestMethod]
        public void CompressedMipmaps()
        {
            Texture2D texture = new Texture2D();
            BufferObject pixelBuffer = new BufferObject(BufferTarget.PixelUnpackBuffer);
            pixelBuffer.SetData(new float[] { 1, 1, 1 }, BufferUsageHint.StaticDraw);
            texture.LoadImageData(1, 1, new List<BufferObject>() { pixelBuffer }, InternalFormat.CompressedRgbaS3tcDxt1Ext);
        }

        [TestMethod]
        public void CompressedBaseLevel()
        {
            Texture2D texture = new Texture2D();
            BufferObject pixelBuffer = new BufferObject(BufferTarget.PixelUnpackBuffer);
            pixelBuffer.SetData(new float[] { 1, 1, 1 }, BufferUsageHint.StaticDraw);
            texture.LoadImageData(1, 1, pixelBuffer, InternalFormat.CompressedRgbaS3tcDxt1Ext);
        }
    }
}
