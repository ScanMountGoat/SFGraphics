using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Textures;
using OpenTK.Graphics.OpenGL;

namespace SFGraphicsRenderTests.Tests.TextureTests
{
    [TestClass]
    public class CompressedImageSizeTests
    {
        private static readonly int width = 256;
        private static readonly int height = 128;

        [TestInitialize]
        public void TestSetup()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.CreateDummyContext();
        }

        private static bool CompressedTexImage2DSucceeded(int width, int height, InternalFormat internalFormat)
        {
            GL.GetError(); // Clear errors

            int imageSize = CompressedImageSize.CalculateImageSize(width, height, internalFormat);
            GL.CompressedTexImage2D(TextureTarget.Texture2D, 0, internalFormat, width, height, 0, imageSize, IntPtr.Zero);

            // Invalid image size is the only way to get InvalidValue
            return GL.GetError().ToString() == "NoError";
        }

        [TestMethod]
        public void Dxt1Rgb()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgbS3tcDxt1Ext));
        }

        [TestMethod]
        public void Dxt1Rgba()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgbaS3tcDxt1Ext));
        }

        [TestMethod]
        public void Dxt1Srgb()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSrgbS3tcDxt1Ext));
        }

        [TestMethod]
        public void Dxt1SrgbAlpha()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSrgbAlphaS3tcDxt1Ext));
        }

        [TestMethod]
        public void Dxt3Test()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgbaS3tcDxt3Ext));
        }

        [TestMethod]
        public void Dxt5Test()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgbaS3tcDxt5Ext));
        }
    }
}
