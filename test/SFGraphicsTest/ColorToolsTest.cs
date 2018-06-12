using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using SFGraphics;

namespace SFGraphicsTest
{
    [TestClass]
    public class ColorToolsTest
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
}
