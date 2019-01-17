using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SFGraphics.Utils.Test.VectorUtilsTests
{
    [TestClass]
    public class GetDegrees
    {
        private readonly double delta = 0.00001;

        [TestMethod]
        public void ZeroRadiansToDegrees()
        {
            Assert.AreEqual(0, VectorUtils.GetDegrees(0), delta);
        }

        [TestMethod]
        public void SmallRadiansToDegrees()
        {
            Assert.AreEqual(35, VectorUtils.GetDegrees(0.6108652382), delta);
        }

        [TestMethod]
        public void LargeRadiansToDegrees()
        {
            Assert.AreEqual(720, VectorUtils.GetDegrees(12.566370614), delta);
        }
    }
}
