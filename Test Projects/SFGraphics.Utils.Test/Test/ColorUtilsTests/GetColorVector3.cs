using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Utils;
using OpenTK;
using System.Drawing;

namespace ColorUtilsTests
{
    [TestClass]
    public class GetColorVector3
    {
        [TestMethod]
        public void WithinRange()
        {
            Assert.AreEqual(Color.FromArgb(255, 127, 0, 255), ColorUtils.ColorFromVector3(new Vector3(0.5f, 0, 1)));
        }

        [TestMethod]
        public void BelowRange()
        {
            Assert.AreEqual(Color.FromArgb(255, 0, 0, 0), ColorUtils.ColorFromVector3(new Vector3(-1, -1, -1)));
        }

        [TestMethod]
        public void AboveRange()
        {
            Assert.AreEqual(Color.FromArgb(255, 255, 255, 255), ColorUtils.ColorFromVector3(new Vector3(1.1f, 1.1f, 1.1f)));
        }
    }
}
