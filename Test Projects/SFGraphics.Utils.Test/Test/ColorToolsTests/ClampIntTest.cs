using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Utils;

namespace SFGraphicsTest.ColorToolsTests
{
    public partial class ColorToolsTest
    {
        [TestClass]
        public class ClampIntTest
        {
            [TestMethod]
            public void BelowDefaultMin()
            {
                Assert.AreEqual(0, ColorTools.ClampInt(-5));
            }

            [TestMethod]
            public void AboveDefaultMax()
            {
                Assert.AreEqual(255, ColorTools.ClampInt(267));
            }

            [TestMethod]
            public void WithinDefaultRange()
            {
                Assert.AreEqual(127, ColorTools.ClampInt(127));
            }

            [TestMethod]
            public void BelowCustomMin()
            {
                Assert.AreEqual(15, ColorTools.ClampInt(0, 15));
            }

            [TestMethod]
            public void AboveCustomMax()
            {
                Assert.AreEqual(235, ColorTools.ClampInt(255, 0, 235));
            }
        }
    }
}
