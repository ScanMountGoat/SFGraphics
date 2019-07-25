using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace SFGraphics.Utils.Test.VectorUtilsTests
{
    [TestClass]
    public class CalculateTangentBitangent
    {
        [TestMethod]
        public void DifferentUVsDifferentPositions()
        {
            Vector3 v1 = new Vector3(1, 0, 0);
            Vector3 v2 = new Vector3(0, 1, 0);
            Vector3 v3 = new Vector3(0, 0, 1);
            Vector2 uv1 = new Vector2(1, 0);
            Vector2 uv2 = new Vector2(0, 1);
            Vector2 uv3 = new Vector2(1, 1);

            VectorUtils.GenerateTangentBitangent(v1, v2, v3, uv1, uv2, uv3, out Vector3 tangent, out Vector3 bitangent);

            Assert.AreEqual(new Vector3(0, -1, 1), tangent);
            Assert.AreEqual(new Vector3(-1, 0, 1), bitangent);
        }

        [TestMethod]
        public void DifferentUVsSamePositions()
        {
            Vector3 v1 = new Vector3(1, 0, 0);
            Vector3 v2 = new Vector3(1, 0, 0);
            Vector3 v3 = new Vector3(1, 0, 0);
            Vector2 uv1 = new Vector2(1, 0);
            Vector2 uv2 = new Vector2(0, 1);
            Vector2 uv3 = new Vector2(1, 1);
            VectorUtils.GenerateTangentBitangent(v1, v2, v3, uv1, uv2, uv3, out Vector3 tangent, out Vector3 bitangent);

            // Make sure tangents and bitangents aren't all zero.
            Assert.AreEqual(VectorUtils.defaultTangent, tangent);
            Assert.AreEqual(VectorUtils.defaultBitangent, bitangent);
        }

        [TestMethod]
        public void SameUVsDifferentPositions()
        {
            Vector3 v1 = new Vector3(1, 0, 0);
            Vector3 v2 = new Vector3(0, 1, 0);
            Vector3 v3 = new Vector3(0, 0, 1);
            Vector2 uv1 = new Vector2(1, 1);
            Vector2 uv2 = new Vector2(1, 1);
            Vector2 uv3 = new Vector2(1, 1);
            VectorUtils.GenerateTangentBitangent(v1, v2, v3, uv1, uv2, uv3, out Vector3 tangent, out Vector3 bitangent);

            // Make sure tangents and bitangents aren't all zero.
            Assert.AreEqual(VectorUtils.defaultTangent, tangent);
            Assert.AreEqual(VectorUtils.defaultBitangent, bitangent);
        }

        [TestMethod]
        public void SameUVsSamePositions()
        {
            Vector3 v1 = new Vector3(1, 0, 0);
            Vector3 v2 = new Vector3(1, 0, 0);
            Vector3 v3 = new Vector3(1, 0, 0);
            Vector2 uv1 = new Vector2(1, 1);
            Vector2 uv2 = new Vector2(1, 1);
            Vector2 uv3 = new Vector2(1, 1);
            VectorUtils.GenerateTangentBitangent(v1, v2, v3, uv1, uv2, uv3, out Vector3 tangent, out Vector3 bitangent);

            // Make sure tangents and bitangents aren't all zero.
            Assert.AreEqual(VectorUtils.defaultTangent, tangent);
            Assert.AreEqual(VectorUtils.defaultBitangent, bitangent);
        }

        [TestMethod]
        public void UvsWouldCauseDivideByZero()
        {
            Vector3 v1 = new Vector3(1, 0, 0);
            Vector3 v2 = new Vector3(0, 1, 0);
            Vector3 v3 = new Vector3(0, 0, 1);

            // Force the divisor to be 0.
            Vector2 uv1 = new Vector2(0.5f, 0);
            Vector2 uv2 = new Vector2(0.5f, 0);
            Vector2 uv3 = new Vector2(1, 1);

            VectorUtils.GenerateTangentBitangent(v1, v2, v3, uv1, uv2, uv3, out Vector3 tangent, out Vector3 bitangent);

            // Check for division by 0.
            Assert.IsFalse(IsInfiniteOrNaN(tangent.X));
            Assert.IsFalse(IsInfiniteOrNaN(tangent.Y));
            Assert.IsFalse(IsInfiniteOrNaN(tangent.Z));
            Assert.IsFalse(IsInfiniteOrNaN(bitangent.X));
            Assert.IsFalse(IsInfiniteOrNaN(bitangent.Y));
            Assert.IsFalse(IsInfiniteOrNaN(bitangent.Z));
        }

        private static bool IsInfiniteOrNaN(float f)
        {
            return float.IsInfinity(f) || float.IsNaN(f);
        }
    }
}
