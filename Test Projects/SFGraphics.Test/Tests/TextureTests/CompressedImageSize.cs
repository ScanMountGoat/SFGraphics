﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures.TextureFormats;
using System;

namespace SFGraphics.Test.TextureTests
{
    [TestClass]
    public class CompressedImageSize : GraphicsContextTest
    {
        private static readonly int width = 8;
        private static readonly int height = 2;

        [TestInitialize]
        public void TestSetup()
        {
            // Set up the context for all the tests.
            base.Initialize();

            // Binding a pixel unpack buffer affects texture loading methods.
            GL.BindBuffer(BufferTarget.PixelUnpackBuffer, 0);
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
        public void CompressedR11Eac()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedR11Eac));
        }

        [TestMethod]
        public void CompressedRed()
        {
            var e = Assert.ThrowsException<NotSupportedException>(() =>
                CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRed));

            Assert.AreEqual("Generic compressed formats are not supported.", e.Message);
        }

        [TestMethod]
        public void CompressedRedRgtc1()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRedRgtc1));
        }

        [TestMethod]
        public void CompressedRg()
        {
            var e = Assert.ThrowsException<NotSupportedException>(() =>
                CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRg));

            Assert.AreEqual("Generic compressed formats are not supported.", e.Message);
        }

        [TestMethod]
        public void CompressedRg11Eac()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRg11Eac));
        }

        [TestMethod]
        public void CompressedRgb()
        {
            var e = Assert.ThrowsException<NotSupportedException>(() =>
                CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgb));

            Assert.AreEqual("Generic compressed formats are not supported.", e.Message);
        }

        [TestMethod]
        public void CompressedRgb8Etc2()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgb8Etc2));
        }

        [TestMethod]
        public void CompressedRgb8PunchthroughAlpha1Etc2()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgb8PunchthroughAlpha1Etc2));
        }

        [TestMethod]
        public void CompressedRgba()
        {
            var e = Assert.ThrowsException<NotSupportedException>(() =>
                CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgba));

            Assert.AreEqual("Generic compressed formats are not supported.", e.Message);
        }

        [TestMethod]
        public void CompressedRgba8Etc2Eac()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgba8Etc2Eac));
        }

        [TestMethod]
        public void CompressedRgbaBptcUnorm()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgbaBptcUnorm));
        }

        [TestMethod]
        public void CompressedRgbaS3tcDxt1Ext()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgbaS3tcDxt1Ext));
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
        public void CompressedRgbBptcSignedFloat()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgbBptcSignedFloat));
        }

        [TestMethod]
        public void CompressedRgbBptcUnsignedFloat()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgbBptcUnsignedFloat));
        }

        [TestMethod]
        public void CompressedRgbS3tcDxt1Ext()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgbS3tcDxt1Ext));
        }

        [TestMethod]
        public void CompressedRgRgtc2()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedRgRgtc2));
        }

        [TestMethod]
        public void CompressedSignedR11Eac()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSignedR11Eac));
        }

        [TestMethod]
        public void CompressedSignedRedRgtc1()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSignedRedRgtc1));
        }

        [TestMethod]
        public void CompressedSignedRg11Eac()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSignedRg11Eac));
        }

        [TestMethod]
        public void CompressedSignedRgRgtc2()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSignedRgRgtc2));
        }

        [TestMethod]
        public void CompressedSrgb()
        {
            var e = Assert.ThrowsException<NotSupportedException>(() =>
                CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSrgb));

            Assert.AreEqual("Generic compressed formats are not supported.", e.Message);
        }

        [TestMethod]
        public void CompressedSrgb8Alpha8Etc2Eac()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSrgb8Alpha8Etc2Eac));
        }

        [TestMethod]
        public void CompressedSrgb8Etc2()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSrgb8Etc2));
        }

        [TestMethod]
        public void CompressedSrgb8PunchthroughAlpha1Etc2()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSrgb8PunchthroughAlpha1Etc2));
        }

        [TestMethod]
        public void CompressedSrgbAlpha()
        {
            var e = Assert.ThrowsException<NotSupportedException>(() =>
                CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSrgbAlpha));

            Assert.AreEqual("Generic compressed formats are not supported.", e.Message);
        }

        [TestMethod]
        public void CompressedSrgbAlphaBptcUnorm()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSrgbAlphaBptcUnorm));
        }

        [TestMethod]
        public void CompressedSrgbAlphaS3tcDxt1Ext()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSrgbAlphaS3tcDxt1Ext));
        }

        [TestMethod]
        public void CompressedSrgbAlphaS3tcDxt3Ext()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSrgbAlphaS3tcDxt3Ext));
        }

        [TestMethod]
        public void CompressedSrgbAlphaS3tcDxt5Ext()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSrgbAlphaS3tcDxt5Ext));
        }

        [TestMethod]
        public void CompressedSrgbS3tcDxt1Ext()
        {
            Assert.IsTrue(CompressedTexImage2DSucceeded(width, height, InternalFormat.CompressedSrgbS3tcDxt1Ext));
        }
    }
}
