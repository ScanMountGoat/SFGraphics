using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;


namespace SFGraphics.Test.TextureTests.Texture3DTests
{
    [TestClass]
    public class LoadImageData : GraphicsContextTest
    {
        // 2 x 4 x 8 RGBA byte.
        private readonly byte[] imageData = new byte[2 * 4 * 8 * 4];

        [TestMethod]
        public void SetDimensions()
        {
            var texture = new Texture3D();
            texture.LoadImageData(2, 4, 8, imageData, new TextureFormatUncompressed(PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.Byte));

            Assert.AreEqual(TextureWrapMode.ClampToEdge, texture.TextureWrapS);
            Assert.AreEqual(TextureWrapMode.ClampToEdge, texture.TextureWrapT);
            Assert.AreEqual(TextureWrapMode.ClampToEdge, texture.TextureWrapR);

            // Ensure the right wrap mode is used for no mipmaps.
            Assert.AreEqual(TextureMagFilter.Linear, texture.MagFilter);
            Assert.AreEqual(TextureMinFilter.Linear, texture.MinFilter);

            Assert.AreEqual(2, texture.Width);
            Assert.AreEqual(4, texture.Height);
            Assert.AreEqual(8, texture.Depth);
        }
    }
}
