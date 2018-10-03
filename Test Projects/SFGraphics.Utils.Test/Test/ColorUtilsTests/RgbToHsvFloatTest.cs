﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Utils;

namespace ColorUtilsTests
{
    [TestClass]
    public class RgbToHsvFloatTest
    {
        [TestMethod]
        public void RgbToHsvBlack()
        {
            float h;
            float s;
            float v;
            ColorUtils.RgbToHsv(0, 0, 0, out h, out s, out v);

            Assert.AreEqual(0, h);
            Assert.AreEqual(0, s);
            Assert.AreEqual(0, v);
        }

        [TestMethod]
        public void RgbToHsvWhite()
        {
            float h;
            float s;
            float v;
            ColorUtils.RgbToHsv(1, 1, 1, out h, out s, out v);

            Assert.AreEqual(0, h);
            Assert.AreEqual(0, s);
            Assert.AreEqual(1, v);
        }

        [TestMethod]
        public void RgbToHsvRed()
        {
            float h;
            float s;
            float v;
            ColorUtils.RgbToHsv(1, 0, 0, out h, out s, out v);

            Assert.AreEqual(0, h);
            Assert.AreEqual(1, s);
            Assert.AreEqual(1, v);
        }
    }
}
