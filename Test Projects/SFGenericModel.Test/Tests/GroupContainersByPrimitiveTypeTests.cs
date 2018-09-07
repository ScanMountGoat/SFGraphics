using System.Collections.Generic;
using System.Linq;
using SFGenericModel.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.Test.Tests
{
    [TestClass]
    public class GroupContainersByPrimitiveTypeTests
    {
        private readonly List<int> indices = new List<int>() { 0, 1, 2 };

        private readonly List<float> verticesA = new List<float>() { 1, 2, 3 };
        private readonly List<float> verticesB = new List<float>() { 4, 5, 6 };
        private readonly List<float> verticesC = new List<float>() { 7, 8, 9 };

        [TestMethod]
        public void AllTriangles()
        {
            List<VertexContainer<float>> vertexContainers = CreateContainersAllTriangles();
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
        public void AlternatingTrianglesTriangleStrip()
        {
            List<VertexContainer<float>> vertexContainers = CreateContainersTriangleTriangleStrip();
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
        public void EmptyInput()
        {
            List<VertexContainer<float>> vertexContainers = new List<VertexContainer<float>>();
            List<VertexContainer<float>> optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(vertexContainers);

            Assert.AreEqual(0, optimizedContainers.Count);
        }

        private List<VertexContainer<float>> CreateContainersAllTriangles()
        {
            List<VertexContainer<float>> vertexContainers = new List<VertexContainer<float>>()
            {
                new VertexContainer<float>(verticesA, indices, PrimitiveType.Triangles),
                new VertexContainer<float>(verticesB, indices, PrimitiveType.Triangles),
            };

            return vertexContainers;
        }

        private List<VertexContainer<float>> CreateContainersTriangleTriangleStrip()
        {
            List<VertexContainer<float>> vertexContainers = new List<VertexContainer<float>>()
            {
                new VertexContainer<float>(verticesA, indices, PrimitiveType.Triangles),
                new VertexContainer<float>(verticesB, indices, PrimitiveType.TriangleStrip),
                new VertexContainer<float>(verticesC, indices, PrimitiveType.Triangles)
            };

            return vertexContainers;
        }
    }
}
