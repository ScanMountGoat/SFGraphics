using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SFGenericModel.Utils.Test.IndexUtilsTests
{
    [TestClass]
    public class GenerateOptimizedVertexData
    {
        [TestMethod]
        public void SingleVertex()
        {
            var vertices = new List<char>() { 'a' };
            IndexUtils.OptimizedVertexData(vertices, out List<char> newVertices, out List<int> newIndices);

            CollectionAssert.AreEqual(vertices, newVertices);
            CollectionAssert.AreEqual(new List<int>() { 0 }, newIndices);
        }
    }
}
