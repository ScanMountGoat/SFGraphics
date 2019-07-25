using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace SFGraphics.Utils.Test.TriangleListUtilsTests
{
    [TestClass]
    public class CalculateTangentsBitangents
    {
        private static readonly float delta = 0.0001f;

        [TestMethod]
        public void ThreeVertices()
        {
            var values3d = new List<Vector3> { new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1) };
            var values2d = new List<Vector2> { new Vector2(1, 0), new Vector2(0, 0), new Vector2(0, 1) };

            TriangleListUtils.CalculateTangentsBitangents(values3d, values3d, values2d,
                new List<int> { 0, 1, 2 }, out Vector3[] tangents, out Vector3[] bitangents);

            // Ensure vectors are normalized.
            Assert.AreEqual(1.0f, tangents[0].Length, delta);
            Assert.AreEqual(1.0f, tangents[1].Length, delta);
            Assert.AreEqual(1.0f, tangents[2].Length, delta);

            Assert.AreEqual(1.0f, bitangents[0].Length, delta);
            Assert.AreEqual(1.0f, bitangents[1].Length, delta);
            Assert.AreEqual(1.0f, bitangents[2].Length, delta);
        }

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
                TriangleListUtils.CalculateTangentsBitangents(new List<Vector3>(), new List<Vector3> { Vector3.Zero }, new List<Vector2>(),
                    new List<int>(), out Vector3[] tangents, out Vector3[] bitangents));

            Assert.IsTrue(e.Message.Contains("Vector source lengths do not match."));
            Assert.AreEqual("normals", e.ParamName);
        }

        [TestMethod]
        public void IncorrectUvsCount()
        {
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                TriangleListUtils.CalculateTangentsBitangents(new List<Vector3>(), new List<Vector3>(), new List<Vector2> { Vector2.Zero },
                    new List<int>(), out Vector3[] tangents, out Vector3[] bitangents));

            Assert.IsTrue(e.Message.Contains("Vector source lengths do not match."));
            Assert.AreEqual("uvs", e.ParamName);
        }
    }
}
