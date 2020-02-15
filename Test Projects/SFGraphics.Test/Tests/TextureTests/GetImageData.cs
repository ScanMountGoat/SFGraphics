using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;


namespace SFGraphics.Test.TextureTests
{
    [TestClass]
    public class GetImageData : GraphicsContextTest
    {
        private readonly byte[] originalData = { 128, 255, 0, 10 };

        [TestMethod]
        public void GetSingleRgbaPixel()
        {
            Texture2D texture = new Texture2D();
            texture.LoadImageData(1, 1, originalData, 
                new TextureFormatUncompressed(PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.UnsignedByte));

            byte[] imageData = texture.GetImageData(0);
            CollectionAssert.AreEqual(originalData, imageData);
        }
    }
}
