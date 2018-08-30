using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SFGraphics.Utils;

namespace SFGraphicsTest.VectorToolsTests
{
    public partial class VectorToolsTest
    {
        [TestClass]
        public class OrthogonalizeTst
        {
            private readonly double delta = 0.0001;

            [TestMethod]
            public void OrthogonalizeVector()
            {
                // Not orthogonal initally.
                Vector3 a = new Vector3(1, 0.5f, 0);
                Vector3 b = new Vector3(1, 0, 0);
                Assert.AreNotEqual(0, Vector3.Dot(a, b));

                // a and b should now be orthogonal.
                // dot(a, b) == 0 if a and b are orthogonal.
                Vector3 aOrthoToB = VectorTools.Orthogonalize(a, b);
                Assert.AreEqual(0, Vector3.Dot(aOrthoToB, b), delta);
            }

            [TestMethod]
            public void AlreadyOrthogonal()
            {
                // Already orthogonal.
                Vector3 a = new Vector3(0, 1, 0);
                Vector3 b = new Vector3(1, 0, 0);
                Assert.AreEqual(0, Vector3.Dot(a, b), delta);

                // a should remain the same
                Vector3 aOrthoToB = VectorTools.Orthogonalize(a, b);
                Assert.AreEqual(a, aOrthoToB);
            }
        }
    }
}
