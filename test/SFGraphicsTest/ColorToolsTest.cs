using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using OpenTK;
using SFGraphics.Tools;

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
        public class Vector4FromColorTest
        {
            [TestMethod]
            public void Vector4FromBlack()
            {
                Color color = Color.FromArgb(128, 0, 0, 0);
                Vector4 actual = ColorTools.Vector4FromColor(color);

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
                Vector4 actual = ColorTools.Vector4FromColor(color);

                float delta = 0.01f;
                Vector4 expected = new Vector4(0.25f, 0.13f, 0.06f, 0.5f);
                Assert.AreEqual(expected.X, actual.X, delta);
                Assert.AreEqual(expected.Y, actual.Y, delta);
                Assert.AreEqual(expected.Z, actual.Z, delta);
                Assert.AreEqual(expected.W, actual.W, delta);
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
            public void HsvToRgbVec3White()
            {
                Vector3 whiteHsv = new Vector3(0, 0, 1);
                Vector3 actualRgb = ColorTools.HsvToRgb(whiteHsv);

                Vector3 expectedRgb = new Vector3(1);
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
        public class RgbToHsvVec3Test
        {
            [TestMethod]
            public void RgbToHsvVec3Black()
            {
                Vector3 blackRgb = new Vector3(0);
                Vector3 actualHsv = ColorTools.RgbToHsv(blackRgb);

                Vector3 expectedHsv = new Vector3(0);
                Assert.AreEqual(expectedHsv, actualHsv);
            }

            [TestMethod]
            public void RgbToHsvVec3White()
            {
                Vector3 whiteRgb = new Vector3(1);
                Vector3 actualHsv = ColorTools.RgbToHsv(whiteRgb);

                Vector3 expectedHsv = new Vector3(0, 0, 1);
                Assert.AreEqual(expectedHsv, actualHsv);
            }

            [TestMethod]
            public void RgbToHsvVec3Red()
            {
                Vector3 redRgb = new Vector3(1, 0, 0);
                Vector3 actualHsv = ColorTools.RgbToHsv(redRgb);

                Vector3 expectedHsv = new Vector3(0, 1, 1);
                Assert.AreEqual(expectedHsv, actualHsv);
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
            public void HsvToRgbWhite()
            {
                float r;
                float g;
                float b;
                ColorTools.HsvToRgb(0, 0, 1, out r, out g, out b);

                Assert.AreEqual(1, r);
                Assert.AreEqual(1, g);
                Assert.AreEqual(1, b);
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
        public class RgbToHsvFloatTest
        {
            [TestMethod]
            public void RgbToHsvBlack()
            {
                float h;
                float s;
                float v;
                ColorTools.RgbToHsv(0, 0, 0, out h, out s, out v);

                Assert.AreEqual(0, h);
                Assert.AreEqual(0, s);
                Assert.AreEqual(0, v);
            }

            [TestMethod]
            public void RgbToHsvWhite()
            {
                float h;
                float s;
                float v;
                ColorTools.RgbToHsv(1, 1, 1, out h, out s, out v);

                Assert.AreEqual(0, h);
                Assert.AreEqual(0, s);
                Assert.AreEqual(1, v);
            }

            [TestMethod]
            public void RgbToHsvRed()
            {
                float h;
                float s;
                float v;
                ColorTools.RgbToHsv(1, 0, 0, out h, out s, out v);

                Assert.AreEqual(0, h);
                Assert.AreEqual(1, s);
                Assert.AreEqual(1, v);
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

        [TestClass]
        public class ClampIntTest
        {
            [TestMethod]
            public void BelowDefaultMin()
            {
                Assert.AreEqual(0, ColorTools.ClampInt(-5));
            }

            [TestMethod]
            public void AboveDefaultMax()
            {
                Assert.AreEqual(255, ColorTools.ClampInt(267));
            }

            [TestMethod]
            public void WithinDefaultRange()
            {
                Assert.AreEqual(127, ColorTools.ClampInt(127));
            }

            [TestMethod]
            public void BelowCustomMin()
            {
                Assert.AreEqual(15, ColorTools.ClampInt(0, 15));
            }

            [TestMethod]
            public void AboveCustomMax()
            {
                Assert.AreEqual(235, ColorTools.ClampInt(255, 0, 235));
            }
        }

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
