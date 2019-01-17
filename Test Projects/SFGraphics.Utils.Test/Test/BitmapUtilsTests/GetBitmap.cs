using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace SFGraphics.Utils.Test.BitmapUtilsTests
{
    [TestClass]
    public class GetBitmap
    {
        [TestMethod]
        public void CreateSinglePixel()
        {
            byte[] pixels = new byte[] { 1, 2, 3, 4 };
            using (var bmp = BitmapUtils.GetBitmap(1, 1, pixels))
            {
                // Compare ABGR to ARGB.
                Assert.AreEqual(Color.FromArgb(4, 3, 2, 1), bmp.GetPixel(0, 0));
            }
        }
    }
}
