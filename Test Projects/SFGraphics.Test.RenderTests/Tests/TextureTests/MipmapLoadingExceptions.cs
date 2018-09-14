using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.Utils;

namespace TextureTests
{
    [TestClass]
    public class MipmapLoadingExceptions
    {
        private readonly List<byte[]> mipmaps = new List<byte[]>();
        private Texture2D texture;

        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();

            // Binding a pixel unpack buffer affects texture loading methods.
            GL.BindBuffer(BufferTarget.PixelUnpackBuffer, 0);

            texture = new Texture2D();
        }

        [TestMethod]
        public void LoadImageData2DBitmap()
        {
            // Doesn't throw exception.
            MipmapLoading.LoadBaseLevelGenerateMipmaps(TextureTarget.Texture2D, new System.Drawing.Bitmap(128, 64));
        }

        [TestMethod]
        public void LoadImageDataCubeBitmap()
        {
            // Doesn't throw exception.
            // Width and height must be equal for cube maps.
            MipmapLoading.LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapPositiveX, new System.Drawing.Bitmap(128, 128));
        }
    }
}
