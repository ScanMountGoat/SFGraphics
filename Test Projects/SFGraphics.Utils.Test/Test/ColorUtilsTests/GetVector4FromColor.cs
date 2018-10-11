using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using OpenTK;
using SFGraphics.Utils;

namespace ColorUtilsTests
{
    [TestClass]
    public class Vector4FromColorTest
    {
        private readonly float delta = 0.001f;

        [TestMethod]
        public void Vector4FromBlack()
        {
            Color color = Color.FromArgb(128, 0, 0, 0);
            Vector4 actual = ColorUtils.GetVector4(color);

            Vector4 expected = new Vector4(0, 0, 0, 0.502f);
            Assert.AreEqual(expected.X, actual.X, delta);
            Assert.AreEqual(expected.Y, actual.Y, delta);
            Assert.AreEqual(expected.Z, actual.Z, delta);
            Assert.AreEqual(expected.W, actual.W, delta);
        }

        [TestMethod]
        public void Vector4FromAllChannels()
        {
            Color color = Color.FromArgb(128, 64, 32, 16);
            Vector4 actual = ColorUtils.GetVector4(color);

            Vector4 expected = new Vector4(0.251f, 0.126f, 0.063f, 0.502f);
            Assert.AreEqual(expected.X, actual.X, delta);
            Assert.AreEqual(expected.Y, actual.Y, delta);
            Assert.AreEqual(expected.Z, actual.Z, delta);
            Assert.AreEqual(expected.W, actual.W, delta);
        }
    }
}
