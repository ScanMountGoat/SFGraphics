using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace TextureTests
{
    [TestClass]
    public class GetBitmap : Tests.ContextTest
    {
        private readonly byte[] originalData = new byte[] { 128, 0, 10, 255 };

        [TestMethod]
        public void GetSinglePixel()
        {
            Texture2D texture = new Texture2D();
            texture.LoadImageData(1, 1, originalData,
                new TextureFormatUncompressed(PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.UnsignedByte));

            using (var bmp = texture.GetBitmap(0))
            {
                // The channels will be swapped by OpenGL.
                Assert.AreEqual(Color.FromArgb(255, 128, 0, 10), bmp.GetPixel(0, 0));
            }
        }
    }
}
