using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Utils;

namespace ColorUtilsTests
{
    [TestClass]
    public class RgbToHsvFloatTest
    {
        [TestMethod]
        public void RgbToHsvBlack()
        {
            ColorUtils.RgbToHsv(0, 0, 0, out float h, out float s, out float v);

            Assert.AreEqual(0, h);
            Assert.AreEqual(0, s);
            Assert.AreEqual(0, v);
        }

        [TestMethod]
        public void RgbToHsvWhite()
        {
            ColorUtils.RgbToHsv(1, 1, 1, out float h, out float s, out float v);

            Assert.AreEqual(0, h);
            Assert.AreEqual(0, s);
            Assert.AreEqual(1, v);
        }

        [TestMethod]
        public void RgbToHsvRed()
        {
            ColorUtils.RgbToHsv(1, 0, 0, out float h, out float s, out float v);

            Assert.AreEqual(0, h);
            Assert.AreEqual(1, s);
            Assert.AreEqual(1, v);
        }
    }
}
