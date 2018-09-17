using System.Collections.Generic;
using System.Linq;
using SFGenericModel.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace MeshBatchUtilsTests
{
    [TestClass]
    public class GroupByPrimitiveType
    {
        private static readonly List<int> indices = new List<int>() { 0, 1, 2 };

        private static readonly List<float> verticesA = new List<float>() { 1, 2, 3 };
        private static readonly List<float> verticesB = new List<float>() { 4, 5, 6 };
        private static readonly List<float> verticesC = new List<float>() { 7, 8, 9 };

        private static readonly List<float> verticesAb = verticesA.Concat(verticesB).ToList();
        private static readonly List<float> verticesAc = verticesA.Concat(verticesC).ToList();

        private List<VertexContainer<float>> trianglesTriangles = new List<VertexContainer<float>>()
        {
            new VertexContainer<float>(verticesA, indices, PrimitiveType.Triangles),
            new VertexContainer<float>(verticesB, indices, PrimitiveType.Triangles),
        };

        private List<VertexContainer<float>> pointsPoints = new List<VertexContainer<float>>()
        {
            new VertexContainer<float>(verticesA, indices, PrimitiveType.Points),
            new VertexContainer<float>(verticesB, indices, PrimitiveType.Points),
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
            List<VertexContainer<float>> optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(trianglesTriangles);

            // Check that vertex data is combined.
            Assert.AreEqual(1, optimizedContainers.Count);
            CollectionAssert.AreEqual(verticesAb, optimizedContainers[0].vertices);

            // Check that indices are offset.
            List<int> expectedIndices = new List<int>() { 0, 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(expectedIndices, optimizedContainers[0].vertexIndices);
        }

        [TestMethod]
        public void CombinePoints()
        {
            List<VertexContainer<float>> optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(pointsPoints);

            // Check that vertex data is combined.
            Assert.AreEqual(1, optimizedContainers.Count);
            CollectionAssert.AreEqual(verticesAb, optimizedContainers[0].vertices);

            // Check that indices are offset.
            List<int> expectedIndices = new List<int>() { 0, 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(expectedIndices, optimizedContainers[0].vertexIndices);
        }

        [TestMethod]
        public void CombineTriangleStrips()
        {
            List<VertexContainer<float>> optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(triangleStripTriangleStrip);

            Assert.AreEqual(1, optimizedContainers.Count);

            // Check that vertex data is combined.
            CollectionAssert.AreEqual(verticesAb, optimizedContainers[0].vertices);

            // Check that indices are offset.
            List<int> expectedIndices = new List<int>() { 0, 1, 2, 2, 3, 3, 4, 5 };
            CollectionAssert.AreEqual(expectedIndices, optimizedContainers[0].vertexIndices);
        }

        [TestMethod]
        public void AlternatingTrianglesTriangleStrip()
        {
            List<VertexContainer<float>> optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(trianglesTriangleStripTriangles);

            // Check that vertex data is combined.
            Assert.AreEqual(2, optimizedContainers.Count);
            CollectionAssert.AreEqual(verticesAc, optimizedContainers[0].vertices);

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

        [TestMethod]
        public void EmptyContainers()
        {
            List<VertexContainer<float>> vertexContainers = new List<VertexContainer<float>>()
            {
                new VertexContainer<float>(new List<float>(), new List<int>(), PrimitiveType.Triangles),
                new VertexContainer<float>(new List<float>(), new List<int>(), PrimitiveType.Triangles)
            };

            List<VertexContainer<float>> optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(vertexContainers);

            Assert.AreEqual(1, optimizedContainers.Count);
            Assert.AreEqual(0, optimizedContainers[0].vertices.Count);
            Assert.AreEqual(0, optimizedContainers[0].vertexIndices.Count);
        }
    }
}
