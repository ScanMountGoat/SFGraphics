using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace SFGraphicsTest.Test.VectorUtilsTests
{
    [TestClass]
    public class CalculateTangentW
    {
        [TestMethod]
        public void ShouldFlip()
        {
            // cross(tangent,bitangent) is in the opposite direction of the normal.
            // This occurs on the side with mirrored UVs.
            var tangent = new Vector3(0, 1, 0);
            var bitangent = new Vector3(1, 0, 0);
            var normal = new Vector3(0, 0, 1);
            var w = SFGraphics.Utils.VectorUtils.CalculateTangentW(normal, tangent, bitangent);
            Assert.AreEqual(-1.0f, w);
        }

        [TestMethod]
        public void ShouldNotFlip()
        {
            // cross(tangent,bitangent) is in the same direction as the normal.
            // This occurs on the side without mirrored UVs.
            var tangent = new Vector3(1, 0, 0);
            var bitangent = new Vector3(0, 1, 0);
            var normal = new Vector3(0, 0, 1);
            var w = SFGraphics.Utils.VectorUtils.CalculateTangentW(normal, tangent, bitangent);
            Assert.AreEqual(1.0f, w);
        }

        [TestMethod]
        public void ShouldNotBeZero()
        {
            // cross(tangent,bitangent) is orthogonal to the normal.
            var tangent = new Vector3(1, 0, 0);
            var bitangent = new Vector3(0, 1, 0);
            var normal = new Vector3(1, 0, 0);
            var w = SFGraphics.Utils.VectorUtils.CalculateTangentW(normal, tangent, bitangent);
            Assert.AreEqual(1.0f, w);
        }
    }
}
