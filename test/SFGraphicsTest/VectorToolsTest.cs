using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using OpenTK;
using SFGraphics.Tools;

namespace SFGraphicsTest
{
    [TestClass]
    public class VectorToolsTest
    {
        [TestClass]
        public class GetRadiansTest
        {
            private readonly double delta = 0.00001;

            [TestMethod]
            public void ZeroDegreesToRadians()
            {
                Assert.AreEqual(0, VectorTools.GetRadians(0), delta);
            }

            [TestMethod]
            public void SmallDegreesToRadians()
            {
                Assert.AreEqual(0.6108652382, VectorTools.GetRadians(35), delta);
            }

            [TestMethod]
            public void LargeDegreesToRadians()
            {
                Assert.AreEqual(12.566370614, VectorTools.GetRadians(720), delta);
            }
        }

        [TestClass]
        public class GetDegreesTest
        {
            private readonly double delta = 0.00001;

            [TestMethod]
            public void ZeroRadiansToDegrees()
            {
                Assert.AreEqual(0, VectorTools.GetDegrees(0), delta);
            }

            [TestMethod]
            public void SmallRadiansToDegrees()
            {
                Assert.AreEqual(35, VectorTools.GetDegrees(0.6108652382), delta);
            }

            [TestMethod]
            public void LargeRadiansToDegrees()
            {
                Assert.AreEqual(720, VectorTools.GetDegrees(12.566370614), delta);
            }
        }

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
