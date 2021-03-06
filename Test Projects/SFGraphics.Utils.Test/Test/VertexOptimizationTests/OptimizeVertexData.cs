﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SFGraphics.Utils.Test.OptimizeVertexDataTests
{
    [TestClass]
    public class OptimizeVertexData
    {
        [TestMethod]
        public void SingleVertex()
        {
            var vertices = new List<char> { 'a' };
            VertexOptimization.OptimizeVertexData(vertices, out List<char> newVertices, out List<int> newIndices);

            CollectionAssert.AreEqual(vertices, newVertices);
            CollectionAssert.AreEqual(new List<int> { 0 }, newIndices);
        }

        [TestMethod]
        public void SingleVertexWithIndices()
        {
            var vertices = new List<char> { 'a' };
            var indices = new List<int> { 0 };
            VertexOptimization.OptimizeVertexData(vertices, indices, out List<char> newVertices, out List<int> newIndices);

            CollectionAssert.AreEqual(vertices, newVertices);
            CollectionAssert.AreEqual(indices, newIndices);
        }

        [TestMethod]
        public void ThreeIdenticalVertices()
        {
            var vertices = new List<char> { 'a', 'a', 'a' };
            VertexOptimization.OptimizeVertexData(vertices, out List<char> newVertices, out List<int> newIndices);

            CollectionAssert.AreEqual(new List<char> { 'a' }, newVertices);
            CollectionAssert.AreEqual(new List<int> { 0, 0, 0 }, newIndices);
        }

        [TestMethod]
        public void ThreeIdenticalVerticesWithIndices()
        {
            var vertices = new List<char> { 'a', 'a', 'a' };
            var indices = new List<int> { 0, 1, 2 };

            VertexOptimization.OptimizeVertexData(vertices, indices, out List<char> newVertices, out List<int> newIndices);

            CollectionAssert.AreEqual(new List<char> { 'a' }, newVertices);
            CollectionAssert.AreEqual(new List<int> { 0, 0, 0 }, newIndices);
        }

        [TestMethod]
        public void ThreeUniqueVertices()
        {
            var vertices = new List<char> { 'a', 'b', 'c' };
            VertexOptimization.OptimizeVertexData(vertices, out List<char> newVertices, out List<int> newIndices);

            CollectionAssert.AreEqual(new List<char> { 'a', 'b', 'c' }, newVertices);
            CollectionAssert.AreEqual(new List<int> { 0, 1, 2 }, newIndices);
        }

        [TestMethod]
        public void ThreeUniqueVerticesWithIndices()
        {
            var vertices = new List<char> { 'a', 'b', 'c' };
            var indices = new List<int> { 0, 1, 2 };

            VertexOptimization.OptimizeVertexData(vertices, indices, out List<char> newVertices, out List<int> newIndices);

            CollectionAssert.AreEqual(new List<char> { 'a', 'b', 'c' }, newVertices);
            CollectionAssert.AreEqual(new List<int> { 0, 1, 2 }, newIndices);
        }

        [TestMethod]
        public void RepeatedAndUniqueVertices()
        {
            var vertices = new List<char> { 'a', 'b', 'd', 'b', 'b', 'c', 'c' };
            VertexOptimization.OptimizeVertexData(vertices, out List<char> newVertices, out List<int> newIndices);

            CollectionAssert.AreEqual(new List<char> { 'a', 'b', 'd', 'c' }, newVertices);
            CollectionAssert.AreEqual(new List<int> { 0, 1, 2, 1, 1, 3, 3 }, newIndices);
        }

        [TestMethod]
        public void RepeatedAndUniqueVerticesWidthIndices()
        {
            var vertices = new List<char> { 'a', 'b', 'd', 'b', 'b', 'c', 'c' };
            VertexOptimization.OptimizeVertexData(vertices, out List<char> newVertices, out List<int> newIndices);

            CollectionAssert.AreEqual(new List<char> { 'a', 'b', 'd', 'c' }, newVertices);
            CollectionAssert.AreEqual(new List<int> { 0, 1, 2, 1, 1, 3, 3 }, newIndices);
        }
    }
}
