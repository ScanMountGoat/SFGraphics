using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SFGraphics.Utils;

namespace ColorToolsTests
{
    public partial class ColorToolsTest
    {
        [TestClass]
        public class RgbToHsvVec3Test
        {
            [TestMethod]
            public void RgbToHsvVec3Black()
            {
                Vector3 blackRgb = new Vector3(0);
                Vector3 actualHsv = ColorUtils.RgbToHsv(blackRgb);

                Vector3 expectedHsv = new Vector3(0);
                Assert.AreEqual(expectedHsv, actualHsv);
            }

            [TestMethod]
            public void RgbToHsvVec3White()
            {
                Vector3 whiteRgb = new Vector3(1);
                Vector3 actualHsv = ColorUtils.RgbToHsv(whiteRgb);

                Vector3 expectedHsv = new Vector3(0, 0, 1);
                Assert.AreEqual(expectedHsv, actualHsv);
            }

            [TestMethod]
            public void RgbToHsvVec3Red()
            {
                Vector3 redRgb = new Vector3(1, 0, 0);
                Vector3 actualHsv = ColorUtils.RgbToHsv(redRgb);

                Vector3 expectedHsv = new Vector3(0, 1, 1);
                Assert.AreEqual(expectedHsv, actualHsv);
            }
        }
    }
}
