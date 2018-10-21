using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SFGraphics.Utils;
using OpenTK;

namespace BoundingSphereTests
{
    [TestClass]
    public class GenerateBoundingSphere
    {
        [TestMethod]
        public void NoVertices()
        {
            var boundingSphere = BoundingSphereGenerator.GenerateBoundingSphere(new List<Vector3>());
            Assert.AreEqual(new Vector4(0), boundingSphere);
        }

        [TestMethod]
        public void UnitCube()
        {
            var points = new List<Vector3>();
            points.Add(new Vector3(0.5f, -0.5f, -0.5f));
            points.Add(new Vector3(0.5f, -0.5f, 0.5f));
            points.Add(new Vector3(-0.5f, -0.5f, 0.5f));
            points.Add(new Vector3(-0.5f, -0.5f, -0.5f));
            points.Add(new Vector3(0.5f, 0.5f, -0.5f));
            points.Add(new Vector3(0.5f, 0.5f, 0.5f));
            points.Add(new Vector3(-0.5f, 0.5f, 0.5f));
            points.Add(new Vector3(-0.5f, 0.5f, -0.5f));

            var boundingSphere = BoundingSphereGenerator.GenerateBoundingSphere(points);
            Assert.IsTrue(SpherePointUtils.SphereContainsPoints(boundingSphere, points));
        }
    }
}