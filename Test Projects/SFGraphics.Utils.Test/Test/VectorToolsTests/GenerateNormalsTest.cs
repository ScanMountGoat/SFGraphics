using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SFGraphics.Utils;

namespace SFGraphicsTest.VectorToolsTests
{
    public partial class VectorToolsTest
    {
        [TestClass]
        public class GenerateNormalsTest
        {
            [TestMethod]
            public void PositiveNormal()
            {
                // Vertices facing the camera should be in counter-clockwise order.
                Vector3 v1 = new Vector3(-5, 5, 1);
                Vector3 v2 = new Vector3(-5, 0, 1);
                Vector3 v3 = new Vector3(0, 0, 1);
                Vector3 normal = VectorTools.CalculateNormal(v1, v2, v3);
                Assert.IsTrue(Vector3.Dot(normal, new Vector3(1)) > 0);
            }

            [TestMethod]
            public void NegativeNormal()
            {
                // Vertices facing the camera in clockwise order.
                Vector3 v1 = new Vector3(-5, 5, 1);
                Vector3 v2 = new Vector3(-5, 0, 1);
                Vector3 v3 = new Vector3(0, 0, 1);
                Vector3 normal = VectorTools.CalculateNormal(v3, v2, v1);
                Assert.IsTrue(Vector3.Dot(normal, new Vector3(1)) < 0);
            }
        }
    }
}
