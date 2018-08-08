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

            int imageSize = TextureFormatTools.CalculateImageSize(width, height, internalFormat);
            GL.CompressedTexImage2D(TextureTarget.Texture2D, 0, internalFormat, width, height, 0, imageSize, IntPtr.Zero);

            // Invalid image size is the only way to get InvalidValue
            return GL.GetError().ToString() == "NoError";
        }

        [TestMethod]
        public void CompressedRgbS3tcDxt1Ext()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgbS3tcDxt1Ext));
        }

        [TestMethod]
        public void CompressedRgbaS3tcDxt1Ext()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgbaS3tcDxt1Ext));
        }

        [TestMethod]
        public void CompressedSrgbS3tcDxt1Ext()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSrgbS3tcDxt1Ext));
        }

        [TestMethod]
        public void CompressedSrgbAlphaS3tcDxt1Ext()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSrgbAlphaS3tcDxt1Ext));
        }

        [TestMethod]
        public void CompressedRgbaS3tcDxt3Ext()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgbaS3tcDxt3Ext));
        }

        [TestMethod]
        public void CompressedRgbaS3tcDxt5Ext()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgbaS3tcDxt5Ext));
        }

        [TestMethod]
        public void CompressedRed()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRed));
        }

        [TestMethod]
        public void CompressedRedRgtc1()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRedRgtc1));
        }

        [TestMethod]
        public void CompressedSignedRedRgtc1()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSignedRedRgtc1));
        }
    }
}
