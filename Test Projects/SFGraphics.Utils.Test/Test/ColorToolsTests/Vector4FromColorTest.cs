using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using OpenTK;
using SFGraphics.Utils;

namespace ColorToolsTests
{
    public partial class ColorToolsTest
    {
        [TestClass]
        public class Vector4FromColorTest
        {
            [TestMethod]
            public void Vector4FromBlack()
            {
                Color color = Color.FromArgb(128, 0, 0, 0);
                Vector4 actual = ColorUtils.Vector4FromColor(color);

                float delta = 0.01f; 
                Vector4 expected = new Vector4(0, 0, 0, 0.5f);
                Assert.AreEqual(expected.X, actual.X, delta);
                Assert.AreEqual(expected.Y, actual.Y, delta);
                Assert.AreEqual(expected.Z, actual.Z, delta);
                Assert.AreEqual(expected.W, actual.W, delta);
            }

            [TestMethod]
            public void Vector4FromAllChannels()
            {
                Color color = Color.FromArgb(128, 64, 32, 16);
                Vector4 actual = ColorUtils.Vector4FromColor(color);

                float delta = 0.01f;
                Vector4 expected = new Vector4(0.25f, 0.13f, 0.06f, 0.5f);
                Assert.AreEqual(expected.X, actual.X, delta);
                Assert.AreEqual(expected.Y, actual.Y, delta);
                Assert.AreEqual(expected.Z, actual.Z, delta);
                Assert.AreEqual(expected.W, actual.W, delta);
            }
        }
    }
}
