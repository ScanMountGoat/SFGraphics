﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Utils;

namespace ColorUtilsTests
{
    [TestClass]
    public class HsvToRgbFloatTest
    {
        [TestMethod]
        public void HsvToRgbBlack()
        {
            float r;
            float g;
            float b;
            ColorUtils.HsvToRgb(0, 0, 0, out r, out g, out b);

            Assert.AreEqual(0, r);
            Assert.AreEqual(0, g);
            Assert.AreEqual(0, b);
        }

        [TestMethod]
        public void HsvToRgbWhite()
        {
            float r;
            float g;
            float b;
            ColorUtils.HsvToRgb(0, 0, 1, out r, out g, out b);

            Assert.AreEqual(1, r);
            Assert.AreEqual(1, g);
            Assert.AreEqual(1, b);
        }

        [TestMethod]
        public void HsvToRgbRed()
        {
            float r;
            float g;
            float b;
            ColorUtils.HsvToRgb(0, 1, 1, out r, out g, out b);

            Assert.AreEqual(1, r);
            Assert.AreEqual(0, g);
            Assert.AreEqual(0, b);
        }
    }
}
