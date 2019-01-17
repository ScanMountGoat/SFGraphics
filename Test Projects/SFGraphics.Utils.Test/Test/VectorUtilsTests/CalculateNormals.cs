using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace SFGraphics.Utils.Test.VectorUtilsTests
{
    [TestClass]
    public class CalculateNormals
    {
        [TestMethod]
        public void PositiveNormal()
        {
            // Vertices facing the camera should be in counter-clockwise order.
            Vector3 v1 = new Vector3(-5, 5, 1);
            Vector3 v2 = new Vector3(-5, 0, 1);
            Vector3 v3 = new Vector3(0, 0, 1);
            Vector3 normal = VectorUtils.CalculateNormal(v1, v2, v3).Normalized();

            Assert.AreEqual(0, normal.X);
            Assert.AreEqual(0, normal.Y);
            Assert.AreEqual(1, normal.Z);
        }

        [TestMethod]
        public void NegativeNormal()
        {
            // Vertices facing the camera in clockwise order.
            Vector3 v1 = new Vector3(-5, 5, 1);
            Vector3 v2 = new Vector3(-5, 0, 1);
            Vector3 v3 = new Vector3(0, 0, 1);
            Vector3 normal = VectorUtils.CalculateNormal(v3, v2, v1).Normalized();

            Assert.AreEqual(0, normal.X);
            Assert.AreEqual(0, normal.Y);
            Assert.AreEqual(-1, normal.Z);
        }
    }
}
