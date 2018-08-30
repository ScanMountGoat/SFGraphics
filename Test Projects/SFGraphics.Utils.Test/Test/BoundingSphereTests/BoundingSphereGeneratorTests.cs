using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SFGraphics.Utils;
using OpenTK;

namespace SFGraphicsTests.BoundingSphereGeneratorTests
{
    [TestClass()]
    public class BoundingSphereGeneratorTests
    {
        [TestMethod()]
        public void NoVertices()
        {
            Vector4 boundingSphere = BoundingSphereGenerator.GenerateBoundingSphere(new List<Vector3>());
            Assert.AreEqual(new Vector4(0), boundingSphere);
        }

        [TestMethod()]
        public void UnitCube()
        {
            List<Vector3> vertices = new List<Vector3>();
            vertices.Add(new Vector3(1, -1, -1));
            vertices.Add(new Vector3(1, -1, 1));
            vertices.Add(new Vector3(-1, -1, 1));
            vertices.Add(new Vector3(-1, -1, -1));
            vertices.Add(new Vector3(1, 1, -1));
            vertices.Add(new Vector3(1, 1, 1));
            vertices.Add(new Vector3(-1, 1, 1));
            vertices.Add(new Vector3(-1, 1, -1));

            Vector4 boundingSphere = BoundingSphereGenerator.GenerateBoundingSphere(vertices);
            Assert.AreEqual(new Vector3(0, 0, 0), boundingSphere.Xyz);
            Assert.AreEqual(Math.Sqrt(2), boundingSphere.W, 0.001f);
        }
    }
}