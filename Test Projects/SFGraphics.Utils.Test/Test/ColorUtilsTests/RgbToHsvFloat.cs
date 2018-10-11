using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Utils;

namespace ColorUtilsTests
{
    [TestClass]
    public class RgbToHsvFloat
    {
        [TestMethod]
        public void Black()
        {
            ColorUtils.RgbToHsv(0, 0, 0, out float h, out float s, out float v);

            Assert.AreEqual(0, h);
            Assert.AreEqual(0, s);
            Assert.AreEqual(0, v);
        }

        [TestMethod]
        public void NegativeRed()
        {
            ColorUtils.RgbToHsv(-1, 0, 0, out float h, out float s, out float v);

            Assert.AreEqual(0, h);
            Assert.AreEqual(0, s);
            Assert.AreEqual(0, v);
        }

        [TestMethod]
        public void GreyishRed()
        {
            ColorUtils.RgbToHsv(1, 0.5f, 0.5f, out float h, out float s, out float v);

            Assert.AreEqual(0, h);
            Assert.AreEqual(0.5f, s);
            Assert.AreEqual(1, v);
        }

        [TestMethod]
        public void White()
        {
            ColorUtils.RgbToHsv(1, 1, 1, out float h, out float s, out float v);

            Assert.AreEqual(0, h);
            Assert.AreEqual(0, s);
            Assert.AreEqual(1, v);
        }

        [TestMethod]
        public void Red()
        {
            ColorUtils.RgbToHsv(1, 0, 0, out float h, out float s, out float v);

            Assert.AreEqual(0, h);
            Assert.AreEqual(1, s);
            Assert.AreEqual(1, v);
        }

        [TestMethod]
        public void Cyan()
        {
            ColorUtils.RgbToHsv(0, 1, 1, out float h, out float s, out float v);

            Assert.AreEqual(180, h);
            Assert.AreEqual(1, s);
            Assert.AreEqual(1, v);
        }


        [TestMethod]
        public void Magenta()
        {
            ColorUtils.RgbToHsv(1, 0, 1, out float h, out float s, out float v);

            Assert.AreEqual(300, h);
            Assert.AreEqual(1, s);
            Assert.AreEqual(1, v);
        }

        [TestMethod]
        public void Yellow()
        {
            ColorUtils.RgbToHsv(1, 1, 0, out float h, out float s, out float v);

            Assert.AreEqual(60, h);
            Assert.AreEqual(1, s);
            Assert.AreEqual(1, v);
        }

        [TestMethod]
        public void BrightRed()
        {
            ColorUtils.RgbToHsv(10, 0, 0, out float h, out float s, out float v);

            Assert.AreEqual(0, h);
            Assert.AreEqual(1, s);
            Assert.AreEqual(10, v);
        }
    }
}
