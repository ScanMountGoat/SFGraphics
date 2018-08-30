using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Utils;

namespace SFGraphicsTest.ColorToolsTests
{
    public partial class ColorToolsTest
    {
        [TestClass]
        public class FloatToIntClampTest
        {
            [TestMethod]
            public void BelowDefaultMin()
            {
                Assert.AreEqual(0, ColorTools.FloatToIntClamp(-1));
            }

            [TestMethod]
            public void AboveDefaultMax()
            {
                Assert.AreEqual(255, ColorTools.FloatToIntClamp(1.1f));
            }

            [TestMethod]
            public void WithinDefaultRange()
            {
                Assert.AreEqual(127, ColorTools.FloatToIntClamp(0.5f));
            }

            [TestMethod]
            public void BelowCustomMin()
            {
                Assert.AreEqual(15, ColorTools.FloatToIntClamp(0, 15));
            }

            [TestMethod]
            public void AboveCustomMax()
            {
                Assert.AreEqual(235, ColorTools.FloatToIntClamp(1, 0, 235));
            }
        }
    }
}
