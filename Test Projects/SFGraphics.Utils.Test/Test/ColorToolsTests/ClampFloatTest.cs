using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Utils;

namespace SFGraphicsTest.ColorToolsTests
{
    public partial class ColorToolsTest
    {
        [TestClass]
        public class ClampFloatTest
        {
            [TestMethod]
            public void BelowDefaultMin()
            {
                Assert.AreEqual(0, ColorTools.ClampFloat(-1));
            }

            [TestMethod]
            public void AboveDefaultMax()
            {
                Assert.AreEqual(1, ColorTools.ClampFloat(1.1f));
            }

            [TestMethod]
            public void WithinDefaultRange()
            {
                Assert.AreEqual(0.5f, ColorTools.ClampFloat(0.5f));
            }

            [TestMethod]
            public void BelowCustomMin()
            {
                Assert.AreEqual(0.25f, ColorTools.ClampFloat(0, 0.25f));
            }

            [TestMethod]
            public void AboveCustomMax()
            {
                Assert.AreEqual(0.75f, ColorTools.ClampFloat(1, 0, 0.75f));
            }
        }
    }
}
