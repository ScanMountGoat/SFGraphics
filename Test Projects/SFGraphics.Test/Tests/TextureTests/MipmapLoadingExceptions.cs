using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures.Utils;


namespace SFGraphics.Test.TextureTests
{
    [TestClass]
    public class MipmapLoadingExceptions : GraphicsContextTest
    {
        [TestMethod]
        public void LoadImageData2DBitmap()
        {
            // Doesn't throw exception.
            MipmapLoading.LoadBaseLevelGenerateMipmaps(TextureTarget.Texture2D, new Bitmap(128, 64));
        }

        [TestMethod]
        public void LoadImageDataCubeBitmap()
        {
            // Doesn't throw exception.
            // Width and height must be equal for cube maps.
            MipmapLoading.LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapPositiveX, new Bitmap(128, 128));
        }
    }
}
