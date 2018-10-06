using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;
using OpenTK.Graphics.OpenGL;

namespace TextureTests
{
    [TestClass]
    public class GetImageData : Tests.ContextTest
    {
        private readonly byte[] originalData = new byte[] { 128, 255, 0, 10 };

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
