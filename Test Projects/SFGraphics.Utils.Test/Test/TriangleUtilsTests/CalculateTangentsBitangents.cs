using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System;
using System.Collections.Generic;

namespace SFGraphics.Utils.Test.TriangleUtilsTests
{
    [TestClass]
    public class CalculateTangentsBitangents
    {
        [TestMethod]
        public void NoVertices()
        {
            TriangleListUtils.CalculateTangentsBitangents(new List<Vector3>(), new List<Vector3>(), new List<Vector2>(), 
                new List<int>(), out Vector3[] tangents, out Vector3[] bitangents);

            Assert.AreEqual(0, tangents.Length);
            Assert.AreEqual(0, bitangents.Length);
        }

        [TestMethod]
        public void IncorrectNormalsCount()
        {
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                TriangleListUtils.CalculateTangentsBitangents(new List<Vector3>(), new List<Vector3>() { Vector3.Zero }, new List<Vector2>(),
                    new List<int>(), out Vector3[] tangents, out Vector3[] bitangents));
        }

        [TestMethod]
        public void IncorrectUvsCount()
        {
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                TriangleListUtils.CalculateTangentsBitangents(new List<Vector3>(), new List<Vector3>(), new List<Vector2>() { Vector2.Zero },
                    new List<int>(), out Vector3[] tangents, out Vector3[] bitangents));
        }
    }
}
