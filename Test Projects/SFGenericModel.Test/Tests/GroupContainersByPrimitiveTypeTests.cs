using System.Collections.Generic;
using SFGenericModel.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.Test.Tests
{
    [TestClass]
    public class GroupContainersByPrimitiveTypeTests
    {
        [TestMethod]
        public void AllTriangles()
        {
            List<VertexContainer<float>> vertexContainers = CreateContainersAllTriangles();
            List<VertexContainer<float>> optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(vertexContainers);

            Assert.AreEqual(1, optimizedContainers.Count);
        }

        [TestMethod]
        public void AlternatingTrianglesTriangleStrip()
        {
            List<VertexContainer<float>> vertexContainers = CreateContainersTriangleTriangleStrip();
            List<VertexContainer<float>> optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(vertexContainers);

            Assert.AreEqual(2, optimizedContainers.Count);
        }

        [TestMethod]
        public void EmptyInput()
        {
            List<VertexContainer<float>> vertexContainers = new List<VertexContainer<float>>();
            List<VertexContainer<float>> optimizedContainers = MeshBatchUtils.GroupContainersByPrimitiveType(vertexContainers);

            Assert.AreEqual(0, optimizedContainers.Count);
        }

        private static List<VertexContainer<float>> CreateContainersAllTriangles()
        {
            List<VertexContainer<float>> vertexContainers = new List<VertexContainer<float>>()
            {
                new VertexContainer<float>(new List<float>(), new List<int>(), PrimitiveType.Triangles),
                new VertexContainer<float>(new List<float>(), new List<int>(), PrimitiveType.Triangles),
                new VertexContainer<float>(new List<float>(), new List<int>(), PrimitiveType.Triangles)
            };

            return vertexContainers;
        }

        private static List<VertexContainer<float>> CreateContainersTriangleTriangleStrip()
        {
            List<VertexContainer<float>> vertexContainers = new List<VertexContainer<float>>()
            {
                new VertexContainer<float>(new List<float>(), new List<int>(), PrimitiveType.Triangles),
                new VertexContainer<float>(new List<float>(), new List<int>(), PrimitiveType.TriangleStrip),
                new VertexContainer<float>(new List<float>(), new List<int>(), PrimitiveType.Triangles),
                new VertexContainer<float>(new List<float>(), new List<int>(), PrimitiveType.TriangleStrip)
            };

            return vertexContainers;
        }
    }
}
