using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace SFGraphics.Utils.Test.TriangleListUtilsTests
{
    [TestClass]
    public class CalculateSmoothNormals
    {
        private static readonly float delta = 0.0001f;

        [TestMethod]
        public void NoVertices()
        {
            TriangleListUtils.CalculateSmoothNormals(new List<Vector3>(), new List<int>(), out Vector3[] normals);

            Assert.AreEqual(0, normals.Length);
        }

        [TestMethod]
        public void ThreeVertices()
        {
            var values3d = new List<Vector3> { new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1) };

            TriangleListUtils.CalculateSmoothNormals(values3d, new List<int> { 0, 1, 2 }, out Vector3[] normals);

            // Ensure vectors are normalized.
            Assert.AreEqual(1.0f, normals[0].Length, delta);
            Assert.AreEqual(1.0f, normals[1].Length, delta);
            Assert.AreEqual(1.0f, normals[2].Length, delta);
        }
    }
}
