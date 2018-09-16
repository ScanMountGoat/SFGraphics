using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Utils;

namespace VectorToolsTests
{
    [TestClass]
    public class GetRadians
    {
        private readonly double delta = 0.00001;

        [TestMethod]
        public void ZeroDegreesToRadians()
        {
            Assert.AreEqual(0, VectorUtils.GetRadians(0), delta);
        }

        [TestMethod]
        public void SmallDegreesToRadians()
        {
            Assert.AreEqual(0.6108652382, VectorUtils.GetRadians(35), delta);
        }

        [TestMethod]
        public void LargeDegreesToRadians()
        {
            Assert.AreEqual(12.566370614, VectorUtils.GetRadians(720), delta);
        }
    }
}
