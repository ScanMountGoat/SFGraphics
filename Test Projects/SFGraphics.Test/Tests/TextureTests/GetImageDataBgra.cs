using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;


namespace SFGraphics.Test.TextureTests
{
    [TestClass]
    public class GetImageDataBgra : GraphicsContextTest
    {
        private readonly byte[] originalData = { 128, 0, 10, 255 };

        [TestMethod]
        public void GetSinglePixel()
        {
            Texture2D texture = new Texture2D();
            texture.LoadImageData(1, 1, originalData,
                new TextureFormatUncompressed(PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.UnsignedByte));

            // The channels will be swapped by OpenGL.
            byte[] imageData = texture.GetImageDataBgra(0);
            CollectionAssert.AreEqual(new byte[] { 10, 0, 128, 255 }, imageData);
        }
    }
}
