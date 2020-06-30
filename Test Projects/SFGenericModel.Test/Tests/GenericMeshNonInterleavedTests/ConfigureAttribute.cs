using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using RenderTestUtils;
using SFGenericModel.VertexAttributes;
using System;

namespace SFGenericModel.Test.GenericMeshNonInterleavedTests
{
    [TestClass]
    public class ConfigureAttribute
    {
        private static readonly int vertexCount = 8;

        private class MeshA : GenericMeshNonInterleaved
        {
            public MeshA() : base(new uint[16], PrimitiveType.Triangles, vertexCount) { }
        }

        [TestInitialize]
        public void Initialize()
        {
            OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void InvaidAttributeNoBuffers()
        {
            var mesh = new MeshA();
            var e = Assert.ThrowsException<System.ArgumentException>(() =>
                mesh.ConfigureAttribute(new VertexFloatAttribute("attr1", ValueCount.Four, VertexAttribPointerType.Byte, false), "buffer1", 0, sizeof(byte) * 4));
            Assert.AreEqual("bufferName", e.ParamName);
            Assert.IsTrue(e.Message.Contains("The buffer buffer1 has not been added."));
        }

        [TestMethod]
        public void ValidAttributeSingleBuffer()
        {
            var mesh = new MeshA();
            mesh.AddBuffer("buffer1", new byte[vertexCount * 4 * sizeof(byte)]);
            mesh.ConfigureAttribute(new VertexFloatAttribute("attr1", ValueCount.Four, VertexAttribPointerType.Byte, false), "buffer1", 0, sizeof(byte) * 4);
        }

        [TestMethod]
        public void InvalidAttributeNegativeOffset()
        {
            var mesh = new MeshA();
            mesh.AddBuffer("buffer1", new byte[vertexCount * 4 * sizeof(byte)]);
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                mesh.ConfigureAttribute(new VertexFloatAttribute("attr1", ValueCount.Four, VertexAttribPointerType.Byte, false), "buffer1", -5, sizeof(byte) * 4));
            Assert.IsTrue(e.Message.Contains("One or more attribute data accesses will not be within the specified buffer's data storage"));
        }

        [TestMethod]
        public void InvalidAttributeNegativeStride()
        {
            var mesh = new MeshA();
            mesh.AddBuffer("buffer1", new byte[vertexCount * 4 * sizeof(byte)]);
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                mesh.ConfigureAttribute(new VertexFloatAttribute("attr1", ValueCount.Four, VertexAttribPointerType.Byte, false), "buffer1", 0, -1 * sizeof(byte) * 4));
            Assert.AreEqual("strideInBytes", e.ParamName);
            Assert.IsTrue(e.Message.Contains("One or more attribute data accesses will not be within the specified buffer's data storage"));
        }

        [TestMethod]
        public void InvalidAttributePointerType()
        {
            // The attribute data size is larger than stride.
            // It's unclear what OpenGL would do in this case.
            var mesh = new MeshA();
            mesh.AddBuffer("buffer1", new byte[vertexCount * 4 * sizeof(byte)]);
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                mesh.ConfigureAttribute(new VertexFloatAttribute("attr1", ValueCount.Four, VertexAttribPointerType.Double, false), "buffer1", 0, sizeof(byte) * 4));
            Assert.AreEqual("strideInBytes", e.ParamName);
            Assert.IsTrue(e.Message.Contains("The size of the attribute's type must not exceed the stride."));
        }

        [TestMethod]
        public void InvalidAttributeStrideTooBig()
        {
            // Attribute would read too many bytes.
            var mesh = new MeshA();
            mesh.AddBuffer("buffer1", new byte[vertexCount * 4 * sizeof(byte)]);
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                mesh.ConfigureAttribute(new VertexFloatAttribute("attr1", ValueCount.Four, VertexAttribPointerType.Byte, false), "buffer1", 0, sizeof(byte) * 5));
            Assert.IsTrue(e.Message.Contains("One or more attribute data accesses will not be within the specified buffer's data storage"));
        }

        [TestMethod]
        public void ValidAttributeMultipleBuffers()
        {
            // Buffer 2 and 3 wouldn't be valid.
            var mesh = new MeshA();
            mesh.AddBuffer("buffer2", new byte[vertexCount * 3 * sizeof(byte)]);
            mesh.AddBuffer("buffer1", new byte[vertexCount * 4 * sizeof(byte)]);
            mesh.AddBuffer("buffer3", new byte[vertexCount * 1 * sizeof(byte)]);

            mesh.ConfigureAttribute(new VertexFloatAttribute("attr1", ValueCount.Four, VertexAttribPointerType.Byte, false), "buffer1", 0, sizeof(byte) * 4);
        }
    }
}
