using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.BufferObjects;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;


namespace SFGraphics.Test.TextureTests
{
    [TestClass]
    public class LoadImageDataBuffer : GraphicsContextTest
    {
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
            Texture2D texture = new Texture2D();
            BufferObject pixelBuffer = new BufferObject(BufferTarget.PixelUnpackBuffer);
            pixelBuffer.SetData(new float[] { 1, 1, 1 }, BufferUsageHint.StaticDraw);
            texture.LoadImageData(1, 1, new List<BufferObject> { pixelBuffer },
                new TextureFormatUncompressed(PixelInternalFormat.Rgb, PixelFormat.Rgb, PixelType.Float));
        }

        [TestMethod]
        public void CompressedMipmaps()
        {
            Texture2D texture = new Texture2D();
            BufferObject pixelBuffer = new BufferObject(BufferTarget.PixelUnpackBuffer);
            pixelBuffer.SetData(new float[] { 1, 1, 1 }, BufferUsageHint.StaticDraw);
            texture.LoadImageData(1, 1, new List<BufferObject> { pixelBuffer }, InternalFormat.CompressedRgbaS3tcDxt1Ext);
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
