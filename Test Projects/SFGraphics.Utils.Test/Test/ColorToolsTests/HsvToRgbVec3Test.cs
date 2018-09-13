using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SFGraphics.Utils;

namespace ColorToolsTests
{
    public partial class ColorToolsTest
    {
        [TestClass]
        public class HsvToRgbVec3Test
        {
            [TestMethod]
            public void HsvToRgbVec3Black()
            {
                Vector3 blackHsv = new Vector3(0);
                Vector3 actualRgb = ColorUtils.HsvToRgb(blackHsv);

                Vector3 expectedRgb = new Vector3(0);
                Assert.AreEqual(expectedRgb, actualRgb);
            }

            [TestMethod]
            public void HsvToRgbVec3White()
            {
                Vector3 whiteHsv = new Vector3(0, 0, 1);
                Vector3 actualRgb = ColorUtils.HsvToRgb(whiteHsv);

                Vector3 expectedRgb = new Vector3(1);
                Assert.AreEqual(expectedRgb, actualRgb);
            }

            [TestMethod]
            public void HsvToRgbVec3Red()
            {
                Vector3 redHsv = new Vector3(0, 1, 1);
                Vector3 actualRgb = ColorUtils.HsvToRgb(redHsv);

                Vector3 expectedRgb = new Vector3(1, 0, 0);
                Assert.AreEqual(expectedRgb, actualRgb);
            }
        }
    }
}
