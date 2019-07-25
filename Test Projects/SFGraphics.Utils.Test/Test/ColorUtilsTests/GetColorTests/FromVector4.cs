using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace SFGraphics.Utils.Test.ColorUtilsTests.GetColorTests
{
    [TestClass]
    public class FromVector4
    {
        [TestMethod]
        public void WithinRange()
        {
            Assert.AreEqual(Color.FromArgb(255, 127, 0, 255), ColorUtils.GetColor(new Vector4(0.5f, 0, 1, 1)));
        }

        [TestMethod]
        public void BelowRange()
        {
            Assert.AreEqual(Color.FromArgb(0, 0, 0, 0), ColorUtils.GetColor(new Vector4(-1, -1, -1, -1)));
        }

        [TestMethod]
        public void AboveRange()
        {
            Assert.AreEqual(Color.FromArgb(255, 255, 255, 255), ColorUtils.GetColor(new Vector4(1.1f, 1.1f, 1.1f, 1.1f)));
        }
    }
}
