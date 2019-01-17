using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using OpenTK;

namespace SFGraphics.Utils.Test.ColorUtilsTests
{
    [TestClass]
    public class Vector3FromColorTest
    {
        private readonly float delta = 0.001f;

        [TestMethod]
        public void Vector3FromBlack()
        {
            Color color = Color.FromArgb(128, 0, 0, 0);
            Vector3 actual = ColorUtils.GetVector3(color);

            Vector3 expected = new Vector3(0, 0, 0);
            Assert.AreEqual(expected.X, actual.X, delta);
            Assert.AreEqual(expected.Y, actual.Y, delta);
            Assert.AreEqual(expected.Z, actual.Z, delta);
        }

        [TestMethod]
        public void Vector3FromRgbChannels()
        {
            Color color = Color.FromArgb(128, 64, 32, 16);
            Vector3 actual = ColorUtils.GetVector3(color);

            Vector3 expected = new Vector3(0.251f, 0.126f, 0.063f);
            Assert.AreEqual(expected.X, actual.X, delta);
            Assert.AreEqual(expected.Y, actual.Y, delta);
            Assert.AreEqual(expected.Z, actual.Z, delta);
        }
    }
}
