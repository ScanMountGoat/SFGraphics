using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SFGraphics.Utils;
using System;

namespace SFGraphicsTest.VectorToolsTests
{
    public partial class VectorToolsTest
    {
        [TestClass]
        public class GenerateTangentBitangentTest
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
                Vector3 s;
                Vector3 t;
                VectorUtils.GenerateTangentBitangent(v1, v2, v3, uv1, uv2, uv3, out s, out t);

                // Make sure tangents and bitangents aren't all zero.
                Assert.IsTrue((Math.Abs(s.X) > 0) || (Math.Abs(s.Y) > 0) || (Math.Abs(s.Z) > 0));
                Assert.IsTrue((Math.Abs(t.X) > 0) || (Math.Abs(t.Y) > 0) || (Math.Abs(t.Z) > 0));

                Assert.AreEqual(new Vector3(0, -1, 1), s);
                Assert.AreEqual(new Vector3(-1, 0, 1), t);
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
                Vector3 s;
                Vector3 t;
                VectorUtils.GenerateTangentBitangent(v1, v2, v3, uv1, uv2, uv3, out s, out t);

                // Make sure tangents and bitangents aren't all zero.
                Assert.IsTrue((Math.Abs(s.X) > 0) || (Math.Abs(s.Y) > 0) || (Math.Abs(s.Z) > 0));
                Assert.IsTrue((Math.Abs(t.X) > 0) || (Math.Abs(t.Y) > 0) || (Math.Abs(t.Z) > 0));
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
                Vector3 s;
                Vector3 t;
                VectorUtils.GenerateTangentBitangent(v1, v2, v3, uv1, uv2, uv3, out s, out t);

                // Make sure tangents and bitangents aren't all zero.
                Assert.IsTrue((Math.Abs(s.X) > 0) || (Math.Abs(s.Y) > 0) || (Math.Abs(s.Z) > 0));
                Assert.IsTrue((Math.Abs(t.X) > 0) || (Math.Abs(t.Y) > 0) || (Math.Abs(t.Z) > 0));
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
                Vector3 s;
                Vector3 t;
                VectorUtils.GenerateTangentBitangent(v1, v2, v3, uv1, uv2, uv3, out s, out t);

                // Make sure tangents and bitangents aren't all zero.
                Assert.IsTrue((Math.Abs(s.X) > 0) || (Math.Abs(s.Y) > 0) || (Math.Abs(s.Z) > 0));
                Assert.IsTrue((Math.Abs(t.X) > 0) || (Math.Abs(t.Y) > 0) || (Math.Abs(t.Z) > 0));
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

                Vector3 s;
                Vector3 t;
                VectorUtils.GenerateTangentBitangent(v1, v2, v3, uv1, uv2, uv3, out s, out t);

                // Check for division by 0.
                Assert.IsFalse(float.IsInfinity(s.X) || float.IsNaN(s.X));
                Assert.IsFalse(float.IsInfinity(s.Y) || float.IsNaN(s.Y));
                Assert.IsFalse(float.IsInfinity(s.Z) || float.IsNaN(s.Z));
                Assert.IsFalse(float.IsInfinity(t.X) || float.IsNaN(s.X));
                Assert.IsFalse(float.IsInfinity(t.Y) || float.IsNaN(t.Y));
                Assert.IsFalse(float.IsInfinity(t.Z) || float.IsNaN(t.Z));
            }
        }
    }
}
