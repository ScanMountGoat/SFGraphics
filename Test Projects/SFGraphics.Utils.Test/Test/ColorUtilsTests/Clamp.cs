using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Utils;

namespace ColorUtilsTests
{
    [TestClass]
    public class ClampFloatTest
    {
        [TestMethod]
        public void WithinRange()
        {
            Assert.AreEqual(0.5f, ColorUtils.Clamp(0.5f, 0, 1));
        }

        [TestMethod]
        public void BelowMin()
        {
            Assert.AreEqual(0.25f, ColorUtils.Clamp(0, 0.25f, 1));
        }

        [TestMethod]
        public void AboveMax()
        {
            Assert.AreEqual(0.75f, ColorUtils.Clamp(1, 0, 0.75f));
        }
    }
}
