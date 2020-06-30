using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using RenderTestUtils;

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

            public TestMesh(float[] vertices, PrimitiveType type) : base(vertices, type)
            {

            }

            public TestMesh(float[] vertices, int[] indices, PrimitiveType type) : base(vertices, indices, type)
            {

            }

            public TestMesh(float[] vertices, uint[] indices, PrimitiveType type) : base(vertices, indices, type)
            {

            }
        }

        private readonly float[] vertices = { 1, 2, 3 };
        private readonly int[] signedIndices = { 1, 2, 3, 4 };
        private readonly uint[] indices = { 1, 2, 3, 4, 5 };

        [TestInitialize]
        public void Initialize()
        {
            OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void IndexedVertexData()
        {
            var mesh = new TestMesh(new IndexedVertexData<float>(vertices, PrimitiveType.Points));

            Assert.AreEqual(vertices.Length, mesh.VertexIndexCount);
            Assert.AreEqual(PrimitiveType.Points, mesh.PrimitiveType);
            Assert.AreEqual(DrawElementsType.UnsignedInt, mesh.DrawElementsType);
        }

        [TestMethod]
        public void VertexList()
        {
            var mesh = new TestMesh(vertices, PrimitiveType.Points);

            Assert.AreEqual(vertices.Length, mesh.VertexIndexCount);
            Assert.AreEqual(PrimitiveType.Points, mesh.PrimitiveType);
            Assert.AreEqual(DrawElementsType.UnsignedInt, mesh.DrawElementsType);
        }

        [TestMethod]
        public void SignedIndices()
        {
            var mesh = new TestMesh(vertices, signedIndices, PrimitiveType.Points);

            Assert.AreEqual(signedIndices.Length, mesh.VertexIndexCount);
            Assert.AreEqual(PrimitiveType.Points, mesh.PrimitiveType);
            Assert.AreEqual(DrawElementsType.UnsignedInt, mesh.DrawElementsType);
        }

        [TestMethod]
        public void UnsignedIndices()
        {
            var mesh = new TestMesh(vertices, indices, PrimitiveType.Points);

            Assert.AreEqual(indices.Length, mesh.VertexIndexCount);
            Assert.AreEqual(PrimitiveType.Points, mesh.PrimitiveType);
            Assert.AreEqual(DrawElementsType.UnsignedInt, mesh.DrawElementsType);
        }
    }
}
