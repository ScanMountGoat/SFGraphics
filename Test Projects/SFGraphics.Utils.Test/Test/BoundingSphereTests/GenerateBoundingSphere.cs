﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace SFGraphics.Utils.Test.BoundingSphereTests
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
            var points = new List<Vector3>
            {
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f)
            };

            var boundingSphere = BoundingSphereGenerator.GenerateBoundingSphere(points);
            Assert.IsTrue(SpherePointUtils.SphereContainsPoints(boundingSphere, points));
        }
    }
}