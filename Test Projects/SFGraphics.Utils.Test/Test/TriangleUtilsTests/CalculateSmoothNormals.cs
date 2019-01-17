using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System.Collections.Generic;

namespace SFGraphics.Utils.Test.TriangleUtilsTests
{
    [TestClass]
    public class CalculateSmoothNormals
    {
        [TestMethod]
        public void NoVertices()
        {
            TriangleListUtils.CalculateSmoothNormals(new List<Vector3>(), new List<int>(), out Vector3[] normals);

            Assert.AreEqual(0, normals.Length);
        }
    }
}
