using System.Collections.Generic;
using System.Linq;
using SFGenericModel.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.Test.MeshBatchUtilsTests
{
    [TestClass]
    public class GroupByPrimitiveType
    {
        private static readonly int[] indices = { 0, 1, 2 };

        private static readonly float[] verticesA = { 1, 2, 3 };
        private static readonly float[] verticesB = { 4, 5, 6 };
        private static readonly float[] verticesC = { 7, 8, 9 };

        private static readonly float[] verticesAb = verticesA.Concat(verticesB).ToArray();
        private static readonly float[] verticesAc = verticesA.Concat(verticesC).ToArray();

        private readonly List<IndexedVertexData<float>> trianglesTriangles = new List<IndexedVertexData<float>>()
        {
            new IndexedVertexData<float>(verticesA, indices, PrimitiveType.Triangles),
            new IndexedVertexData<float>(verticesB, indices, PrimitiveType.Triangles),
        };

        private readonly List<IndexedVertexData<float>> pointsPoints = new List<IndexedVertexData<float>>()
        {
            new IndexedVertexData<float>(verticesA, indices, PrimitiveType.Points),
            new IndexedVertexData<float>(verticesB, indices, PrimitiveType.Points),
        };

        private readonly List<IndexedVertexData<float>> triangleStripTriangleStrip = new List<IndexedVertexData<float>>()
        {
            new IndexedVertexData<float>(verticesA, indices, PrimitiveType.TriangleStrip),
            new IndexedVertexData<float>(verticesB, indices, PrimitiveType.TriangleStrip),
        };

        private readonly List<IndexedVertexData<float>> trianglesTriangleStripTriangles = new List<IndexedVertexData<float>>()
        {
            new IndexedVertexData<float>(verticesA, indices, PrimitiveType.Triangles),
            new IndexedVertexData<float>(verticesB, indices, PrimitiveType.TriangleStrip),
            new IndexedVertexData<float>(verticesC, indices, PrimitiveType.Triangles)
        };

        [TestMethod]
        public void CombineTriangles()
        {
            var optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(trianglesTriangles);

            // Check that vertex data is combined.
            Assert.AreEqual(1, optimizedContainers.Count);
            CollectionAssert.AreEqual(verticesAb, optimizedContainers[0].Vertices);

            // Check that indices are offset.
            var expectedIndices = new List<int>() { 0, 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(expectedIndices, optimizedContainers[0].Indices);
        }

        [TestMethod]
        public void CombinePoints()
        {
            var optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(pointsPoints);

            // Check that vertex data is combined.
            Assert.AreEqual(1, optimizedContainers.Count);
            CollectionAssert.AreEqual(verticesAb, optimizedContainers[0].Vertices);

            // Check that indices are offset.
            var expectedIndices = new List<int>() { 0, 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(expectedIndices, optimizedContainers[0].Indices);
        }

        [TestMethod]
        public void CombineTriangleStrips()
        {
            var optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(triangleStripTriangleStrip);

            Assert.AreEqual(1, optimizedContainers.Count);

            // Check that vertex data is combined.
            CollectionAssert.AreEqual(verticesAb, optimizedContainers[0].Vertices);

            // Check that indices are offset.
            var expectedIndices = new List<int>() { 0, 1, 2, 2, 3, 3, 4, 5 };
            CollectionAssert.AreEqual(expectedIndices, optimizedContainers[0].Indices);
        }

        [TestMethod]
        public void AlternatingTrianglesTriangleStrip()
        {
            var optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(trianglesTriangleStripTriangles);

            // Check that vertex data is combined.
            Assert.AreEqual(2, optimizedContainers.Count);
            CollectionAssert.AreEqual(verticesAc, optimizedContainers[0].Vertices);

            // Check that indices are offset.
            var expectedIndices = new List<int>() { 0, 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(expectedIndices, optimizedContainers[0].Indices);
        }

        [TestMethod]
        public void EmptyList()
        {
            var vertexContainers = new List<IndexedVertexData<float>>();
            var optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(vertexContainers);

            Assert.AreEqual(0, optimizedContainers.Count);
        }

        [TestMethod]
        public void EmptyContainers()
        {
            List<IndexedVertexData<float>> vertexContainers = new List<IndexedVertexData<float>>()
            {
                new IndexedVertexData<float>(new float[0], new int[0], PrimitiveType.Triangles),
                new IndexedVertexData<float>(new float[0], new int[0], PrimitiveType.Triangles)
            };

            var optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(vertexContainers);

            Assert.AreEqual(1, optimizedContainers.Count);
            Assert.AreEqual(0, optimizedContainers[0].Vertices.Length);
            Assert.AreEqual(0, optimizedContainers[0].Indices.Length);
        }
    }
}
