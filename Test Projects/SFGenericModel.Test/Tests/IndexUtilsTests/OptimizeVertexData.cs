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

        [TestMethod]
        public void ThreeIdenticalVertices()
        {
            var vertices = new List<char>() { 'a', 'a', 'a' };
            IndexUtils.OptimizedVertexData(vertices, out List<char> newVertices, out List<int> newIndices);

            CollectionAssert.AreEqual(new List<char>() { 'a' }, newVertices);
            CollectionAssert.AreEqual(new List<int>() { 0, 0, 0 }, newIndices);
        }

        [TestMethod]
        public void ThreeUniqueVertices()
        {
            var vertices = new List<char>() { 'a', 'b', 'c' };
            IndexUtils.OptimizedVertexData(vertices, out List<char> newVertices, out List<int> newIndices);

            CollectionAssert.AreEqual(new List<char>() { 'a', 'b', 'c' }, newVertices);
            CollectionAssert.AreEqual(new List<int>() { 0, 1, 2 }, newIndices);
        }

        [TestMethod]
        public void RepeatedAndUniqueVertices()
        {
            var vertices = new List<char>() { 'a', 'b', 'd', 'b', 'b', 'c', 'c' };
            IndexUtils.OptimizedVertexData(vertices, out List<char> newVertices, out List<int> newIndices);

            CollectionAssert.AreEqual(new List<char>() { 'a', 'b', 'd', 'c' }, newVertices);
            CollectionAssert.AreEqual(new List<int>() { 0, 1, 2, 1, 1, 3, 3 }, newIndices);
        }
    }
}
