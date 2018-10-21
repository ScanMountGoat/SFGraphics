﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SFGraphics.Utils;
using OpenTK;

namespace BoundingSphereTests
{
    [TestClass]
    public class GenerateBoundingSphereFromSpheres
    {
        [TestMethod]
        public void NoSpheres()
        {
            var boundingSphere = BoundingSphereGenerator.GenerateBoundingSphere(new List<Vector4>());
            Assert.AreEqual(new Vector4(0), boundingSphere);
        }

        [TestMethod]
        public void SingleBoundingSphere()
        {
            var spheres = new List<Vector4>() { new Vector4(0, 0, 0, 1) };

            var boundingSphere = BoundingSphereGenerator.GenerateBoundingSphere(spheres);
            Assert.AreEqual(new Vector4(0, 0, 0, 1), boundingSphere);
        }

        [TestMethod]
        public void MaxRadiusSameCenter()
        {
            var spheres = new List<Vector4>() { new Vector4(0, 0, 0, 1), new Vector4(0, 0, 0, 2) };

            var boundingSphere = BoundingSphereGenerator.GenerateBoundingSphere(spheres);
            Assert.AreEqual(new Vector4(0, 0, 0, 2), boundingSphere);
        }

        [TestMethod]
        public void DifferentRadiusDifferentCenter2D()
        {
            var spheres = new List<Vector4>()
            {
                new Vector4(1, 0, 0, 1),
                new Vector4(-1, 0, 0, 1),
                new Vector4(0, 1, 0, 1),
                new Vector4(0, -1, 0, 1),
            };

            var boundingSphere = BoundingSphereGenerator.GenerateBoundingSphere(spheres);
            Assert.AreEqual(new Vector4(0, 0, 0, 2), boundingSphere);
        }
    }
}