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

        private static readonly List<Vector3> cubePositions = new List<Vector3>
        {
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f)
        };

        private static readonly List<Vector3> cubeNormals = new List<Vector3>
        {
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, -1f),
            new Vector3(0f, 0f, -1f),
            new Vector3(0f, 0f, -1f),
            new Vector3(0f, 0f, -1f),
            new Vector3(0f, -1f, 0f),
            new Vector3(0f, -1f, 0f),
            new Vector3(0f, -1f, 0f),
            new Vector3(0f, -1f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3(-1f, 0f, 0f),
            new Vector3(-1f, 0f, 0f),
            new Vector3(-1f, 0f, 0f),
            new Vector3(-1f, 0f, 0f)
        };

        private static readonly List<Vector2> cubeUvs = new List<Vector2>()
        {
            new Vector2(0.375f, 1f),
            new Vector2(0.625f, 1f),
            new Vector2(0.375f, 0.75f),
            new Vector2(0.625f, 0.75f),
            new Vector2(0.375f, 0.75f),
            new Vector2(0.625f, 0.75f),
            new Vector2(0.375f, 0.5f),
            new Vector2(0.625f, 0.5f),
            new Vector2(0.375f, 0.5f),
            new Vector2(0.625f, 0.5f),
            new Vector2(0.375f, 0.25f),
            new Vector2(0.625f, 0.25f),
            new Vector2(0.375f, 0.25f),
            new Vector2(0.625f, 0.25f),
            new Vector2(0.375f, 0f),
            new Vector2(0.625f, 0f),
            new Vector2(0.625f, 1f),
            new Vector2(0.875f, 1f),
            new Vector2(0.625f, 0.75f),
            new Vector2(0.875f, 0.75f),
            new Vector2(0.125f, 1f),
            new Vector2(0.375f, 1f),
            new Vector2(0.125f, 0.75f),
            new Vector2(0.375f, 0.75f)
        };

        private static readonly List<int> cubeIndices = new List<int>
        {
            0, 1, 2, 2, 1, 3, 4, 5, 6, 6, 5, 7, 8, 9, 10, 10, 9, 11, 12, 13, 14, 14, 13, 15, 16, 17, 18, 18, 17, 19, 20, 21, 22, 22, 21, 23
        };

        [TestMethod]
        public void ThreeVerticesNormalized()
        {
            var values3d = new List<Vector3> { new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1) };
            var values2d = new List<Vector2> { new Vector2(1, 0), new Vector2(0, 0), new Vector2(0, 1) };

            TriangleListUtils.CalculateTangentsBitangents(values3d, values3d, values2d,
                new List<int> { 0, 1, 2 }, out Vector3[] tangents, out Vector3[] bitangents);

            // Ensure vectors are normalized.
            for (int i = 0; i < tangents.Length; i++)
            {
                Assert.AreEqual(1.0f, tangents[i].Length, delta);
                Assert.AreEqual(1.0f, bitangents[i].Length, delta);

            }
        }

        [TestMethod]
        public void BasicCubeNormalizedNoWeirdFloats()
        {
            TriangleListUtils.CalculateTangentsBitangents(cubePositions, cubeNormals, cubeUvs, cubeIndices, out Vector3[] tangents, out Vector3[] bitangents);
            for (int i = 0; i < tangents.Length; i++)
            {
                Assert.AreEqual(1.0f, tangents[i].Length, delta);
                Assert.AreEqual(1.0f, bitangents[i].Length, delta);
                Assert.IsFalse(IsBadTangentBitangent(tangents[i], bitangents[i]));
            }
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

        private static bool IsBadTangentBitangent(Vector3 tangent, Vector3 bitangent)
        {
            return IsBadVector(tangent) || IsBadVector(bitangent) || (Vector3.Dot(tangent, bitangent) > delta);
        }

        private static bool IsBadVector(Vector3 v)
        {
            return IsBadFloat(v.X) || IsBadFloat(v.Y) || IsBadFloat(v.Z) || v.Length == 0.0f;
        }

        private static bool IsBadFloat(float f)
        {
            return float.IsNaN(f) || float.IsInfinity(f) || float.IsNegativeInfinity(f);
        }
    }
}
