using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace SFGenericModel.Test.GenericMeshTests
{
    [TestClass]
    public class GenericMeshConstructors
    {
        private class TestMesh : GenericMesh<float>
        {
            public TestMesh(IndexedVertexData<float> vertexData) : base(vertexData)
            {

            }

            public TestMesh(List<float> vertices, PrimitiveType type) : base(vertices, type)
            {

            }

            public TestMesh(List<float> vertices, List<int> indices, PrimitiveType type) : base(vertices, indices, type)
            {

            }

            public TestMesh(List<float> vertices, List<uint> indices, PrimitiveType type) : base(vertices, indices, type)
            {

            }
        }

        private readonly List<float> vertices = new List<float>() { 1, 2, 3 };
        private readonly List<int> signedIndices = new List<int>() { 1, 2, 3, 4 };
        private readonly List<uint> indices = new List<uint>() { 1, 2, 3, 4, 5 };

        [TestInitialize]
        public void Initialize()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void IndexedVertexData()
        {
            var mesh = new TestMesh(new IndexedVertexData<float>(vertices, PrimitiveType.Points));

            Assert.AreEqual(vertices.Count, mesh.VertexCount);
            Assert.AreEqual(PrimitiveType.Points, mesh.PrimitiveType);
            Assert.AreEqual(DrawElementsType.UnsignedInt, mesh.DrawElementsType);
        }

        [TestMethod]
        public void VertexList()
        {
            var mesh = new TestMesh(vertices, PrimitiveType.Points);

            Assert.AreEqual(vertices.Count, mesh.VertexCount);
            Assert.AreEqual(PrimitiveType.Points, mesh.PrimitiveType);
            Assert.AreEqual(DrawElementsType.UnsignedInt, mesh.DrawElementsType);
        }

        [TestMethod]
        public void SignedIndices()
        {
            var mesh = new TestMesh(vertices, signedIndices, PrimitiveType.Points);

            Assert.AreEqual(signedIndices.Count, mesh.VertexCount);
            Assert.AreEqual(PrimitiveType.Points, mesh.PrimitiveType);
            Assert.AreEqual(DrawElementsType.UnsignedInt, mesh.DrawElementsType);
        }

        [TestMethod]
        public void UnsignedIndices()
        {
            var mesh = new TestMesh(vertices, indices, PrimitiveType.Points);

            Assert.AreEqual(indices.Count, mesh.VertexCount);
            Assert.AreEqual(PrimitiveType.Points, mesh.PrimitiveType);
            Assert.AreEqual(DrawElementsType.UnsignedInt, mesh.DrawElementsType);
        }
    }
}
