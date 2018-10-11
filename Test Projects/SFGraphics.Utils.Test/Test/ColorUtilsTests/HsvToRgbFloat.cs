using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Utils;

namespace ColorUtilsTests
{
    [TestClass]
    public class HsvToRgbFloat
    {
        [TestMethod]
        public void Black()
        {
            ColorUtils.HsvToRgb(0, 0, 0, out float r, out float g, out float b);

            Assert.AreEqual(0, r);
            Assert.AreEqual(0, g);
            Assert.AreEqual(0, b);
        }

        [TestMethod]
        public void NegativeRed()
        {
            ColorUtils.HsvToRgb(0, 1, -1, out float r, out float g, out float b);

            Assert.AreEqual(0, r);
            Assert.AreEqual(0, g);
            Assert.AreEqual(0, b);
        }

        [TestMethod]
        public void White()
        {
            ColorUtils.HsvToRgb(0, 0, 1, out float r, out float g, out float b);

            Assert.AreEqual(1, r);
            Assert.AreEqual(1, g);
            Assert.AreEqual(1, b);
        }

        [TestMethod]
        public void Red()
        {
            ColorUtils.HsvToRgb(0, 1, 1, out float r, out float g, out float b);

            Assert.AreEqual(1, r);
            Assert.AreEqual(0, g);
            Assert.AreEqual(0, b);
        }

        [TestMethod]
        public void Cyan()
        {
            ColorUtils.HsvToRgb(180, 1, 1, out float r, out float g, out float b);

            Assert.AreEqual(0, r);
            Assert.AreEqual(1, g);
            Assert.AreEqual(1, b);
        }

        [TestMethod]
        public void Magenta()
        {
            ColorUtils.HsvToRgb(300, 1, 1, out float r, out float g, out float b);

            Assert.AreEqual(1, r);
            Assert.AreEqual(0, g);
            Assert.AreEqual(1, b);
        }

        [TestMethod]
        public void Yellow()
        {
            ColorUtils.HsvToRgb(60, 1, 1, out float r, out float g, out float b);

            Assert.AreEqual(1, r);
            Assert.AreEqual(1, g);
            Assert.AreEqual(0, b);
        }

        [TestMethod]
        public void GreyishRed()
        {
            ColorUtils.HsvToRgb(0, 0.5f, 1, out float r, out float g, out float b);

            Assert.AreEqual(1, r);
            Assert.AreEqual(0.5f, g);
            Assert.AreEqual(0.5f, b);
        }

        [TestMethod]
        public void BrightRed()
        {
            ColorUtils.HsvToRgb(0, 1, 10, out float r, out float g, out float b);

            Assert.AreEqual(10, r);
            Assert.AreEqual(0, g);
            Assert.AreEqual(0, b);
        }
    }
}
