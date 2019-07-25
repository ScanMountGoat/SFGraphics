using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;
using Tests;

namespace SFGraphics.Test.TextureTests.Texture3DTests
{
    [TestClass]
    public class LoadImageData : ContextTest
    {
        // 2 x 4 x 8 RGBA byte.
        private readonly byte[] imageData = new byte[2 * 4 * 8 * 4];

        private Texture3D texture;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            if (texture == null)
                texture = new Texture3D();
        }
     
        [TestMethod]
        public void SetDimensions()
        {
            texture.LoadImageData(2, 4, 8, imageData, new TextureFormatUncompressed(PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.Byte));

            Assert.AreEqual(2, texture.Width);
            Assert.AreEqual(4, texture.Height);
            Assert.AreEqual(8, texture.Depth);
        }
    }
}
