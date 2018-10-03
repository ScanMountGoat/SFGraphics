using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Utils;

namespace ColorUtilsTests
{
    [TestClass]
    public class ClampIntTest
    {
        [TestMethod]
        public void BelowDefaultMin()
        {
            Assert.AreEqual(0, ColorUtils.ClampInt(-5));
        }

        [TestMethod]
        public void AboveDefaultMax()
        {
            Assert.AreEqual(255, ColorUtils.ClampInt(267));
        }

        [TestMethod]
        public void WithinDefaultRange()
        {
            Assert.AreEqual(127, ColorUtils.ClampInt(127));
        }

        [TestMethod]
        public void BelowCustomMin()
        {
            Assert.AreEqual(15, ColorUtils.ClampInt(0, 15));
        }

        [TestMethod]
        public void AboveCustomMax()
        {
            Assert.AreEqual(235, ColorUtils.ClampInt(255, 0, 235));
        }
    }
}
