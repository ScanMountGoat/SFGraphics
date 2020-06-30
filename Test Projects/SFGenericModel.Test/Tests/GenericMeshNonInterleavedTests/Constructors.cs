using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using RenderTestUtils;
using SFGenericModel.VertexAttributes;
using System;

namespace SFGenericModel.Test.GenericMeshNonInterleavedTests
{
    [TestClass]
    public class Constructors
    {
        private class MeshA : GenericMeshNonInterleaved
        {
            public MeshA(uint[] indices, int vertexCount) : base(indices, PrimitiveType.Triangles, vertexCount) { }
        }

        [TestInitialize]
        public void Initialize()
        {
            OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void SetVertexCountVertexIndexCount()
        {
            var mesh = new MeshA(new uint[8], 7);
            Assert.AreEqual(8, mesh.VertexIndexCount);
            Assert.AreEqual(7, mesh.VertexCount);
        }
    }
}
