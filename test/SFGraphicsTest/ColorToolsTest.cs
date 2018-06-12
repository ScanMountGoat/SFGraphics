using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using OpenTK;
using SFGraphics;

namespace SFGraphicsTest
{
    [TestClass]
    public class ColorToolsTest
    {
        [TestClass]
        public class InvertColorTest
        {
            [TestMethod]
            public void InvertColor()
            {
                Color color = Color.FromArgb(255, 255, 255, 255);
                Color inverted = ColorTools.InvertColor(color);

                Color expected = Color.FromArgb(255, 0, 0, 0);
                Assert.AreEqual(expected, inverted);
            }
        }

        [TestClass]
        public class HsvToRgbVec3Test
        {
            [TestMethod]
            public void HsvToRgbVec3Black()
            {
                Vector3 blackHsv = new Vector3(0);
                Vector3 actualRgb = ColorTools.HsvToRgb(blackHsv);

                Vector3 expectedRgb = new Vector3(0);
                Assert.AreEqual(expectedRgb, actualRgb);
            }

            [TestMethod]
            public void HsvToRgbVec3Red()
            {
                Vector3 redHsv = new Vector3(0, 1, 1);
                Vector3 actualRgb = ColorTools.HsvToRgb(redHsv);

                Vector3 expectedRgb = new Vector3(1, 0, 0);
                Assert.AreEqual(expectedRgb, actualRgb);
            }
        }

        [TestClass]
        public class HsvToRgbFloatTest
        {
            [TestMethod]
            public void HsvToRgbBlack()
            {
                float r;
                float g;
                float b;
                ColorTools.HsvToRgb(0, 0, 0, out r, out g, out b);

                Assert.AreEqual(0, r);
                Assert.AreEqual(0, g);
                Assert.AreEqual(0, b);
            }

            [TestMethod]
            public void HsvToRgbRed()
            {
                float r;
                float g;
                float b;
                ColorTools.HsvToRgb(0, 1, 1, out r, out g, out b);

                Assert.AreEqual(1, r);
                Assert.AreEqual(0, g);
                Assert.AreEqual(0, b);
            }
        }

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
