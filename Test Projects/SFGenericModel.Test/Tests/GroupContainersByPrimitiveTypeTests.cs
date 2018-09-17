using System.Collections.Generic;
using System.Linq;
using SFGenericModel.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace MeshBatchUtilsTests
{
    [TestClass]
    public class GroupContainersByPrimitiveTypeTests
    {
        private static readonly List<int> indices = new List<int>() { 0, 1, 2 };

        private static readonly List<float> verticesA = new List<float>() { 1, 2, 3 };
        private static readonly List<float> verticesB = new List<float>() { 4, 5, 6 };
        private static readonly List<float> verticesC = new List<float>() { 7, 8, 9 };

        private List<VertexContainer<float>> trianglesTriangles = new List<VertexContainer<float>>()
        {
            new VertexContainer<float>(verticesA, indices, PrimitiveType.Triangles),
            new VertexContainer<float>(verticesB, indices, PrimitiveType.Triangles),
        };

        private List<VertexContainer<float>> triangleStripTriangleStrip = new List<VertexContainer<float>>()
        {
            new VertexContainer<float>(verticesA, indices, PrimitiveType.TriangleStrip),
            new VertexContainer<float>(verticesB, indices, PrimitiveType.TriangleStrip),
        };

        private List<VertexContainer<float>> trianglesTriangleStripTriangles = new List<VertexContainer<float>>()
        {
            new VertexContainer<float>(verticesA, indices, PrimitiveType.Triangles),
            new VertexContainer<float>(verticesB, indices, PrimitiveType.TriangleStrip),
            new VertexContainer<float>(verticesC, indices, PrimitiveType.Triangles)
        };

        [TestMethod]
        public void CombineTriangles()
        {
            List<VertexContainer<float>> vertexContainers = trianglesTriangles;
            List<VertexContainer<float>> optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(vertexContainers);

            Assert.AreEqual(1, optimizedContainers.Count);

            // Check that vertex data is combined.
            List<float> expectedVertices = verticesA.Concat(verticesB).ToList();
            CollectionAssert.AreEqual(expectedVertices, optimizedContainers[0].vertices);

            // Check that indices are offset.
            List<int> expectedIndices = new List<int>() { 0, 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(expectedIndices, optimizedContainers[0].vertexIndices);
        }

        [TestMethod]
        public void CombineTriangleStrips()
        {
            List<VertexContainer<float>> vertexContainers = triangleStripTriangleStrip;
            List<VertexContainer<float>> optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(vertexContainers);

            Assert.AreEqual(1, optimizedContainers.Count);

            // Check that vertex data is combined.
            List<float> expectedVertices = verticesA.Concat(verticesB).ToList();
            CollectionAssert.AreEqual(expectedVertices, optimizedContainers[0].vertices);

            // Check that indices are offset.
            List<int> expectedIndices = new List<int>() { 0, 1, 2, 2, 3, 3, 4, 5 };
            CollectionAssert.AreEqual(expectedIndices, optimizedContainers[0].vertexIndices);
        }

        [TestMethod]
        public void AlternatingTrianglesTriangleStrip()
        {
            List<VertexContainer<float>> vertexContainers = trianglesTriangleStripTriangles;
            List<VertexContainer<float>> optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(vertexContainers);

            Assert.AreEqual(2, optimizedContainers.Count);

            // Check that vertex data is combined.
            List<float> expectedVertices = verticesA.Concat(verticesC).ToList();
            CollectionAssert.AreEqual(expectedVertices, optimizedContainers[0].vertices);

            // Check that indices are offset.
            List<int> expectedIndices = new List<int>() { 0, 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(expectedIndices, optimizedContainers[0].vertexIndices);
        }

        [TestMethod]
        public void EmptyList()
        {
            List<VertexContainer<float>> vertexContainers = new List<VertexContainer<float>>();
            List<VertexContainer<float>> optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(vertexContainers);

            Assert.AreEqual(0, optimizedContainers.Count);
        }
    }
}
