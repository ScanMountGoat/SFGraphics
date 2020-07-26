﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using RenderTestUtils;
using SFGenericModel.VertexAttributes;
using SFGraphics.GLObjects.BufferObjects;
using System;

namespace SFGenericModel.Test.GenericMeshNonInterleavedTests
{
    [TestClass]
    public class ConfigureAttributeFromExistingBuffer
    {
        private static readonly int vertexCount = 8;
        private static BufferObject buffer1;
        private static BufferObject buffer2;
        private static BufferObject buffer3;
        private static BufferObject buffer4;

        private class MeshA : GenericMeshNonInterleaved
        {
            public MeshA() : base(new uint[16], PrimitiveType.Triangles, vertexCount) { }

        }

        [TestInitialize]
        public void Initialize()
        {
            OpenTKWindowlessContext.BindDummyContext();

            buffer1 = new BufferObject(BufferTarget.ArrayBuffer);
            buffer1.SetData(new byte[vertexCount * 4 * sizeof(byte)], BufferUsageHint.StaticDraw);

            buffer2 = new BufferObject(BufferTarget.ArrayBuffer);
            buffer2.SetData(new byte[vertexCount * 3 * sizeof(byte)], BufferUsageHint.StaticDraw);

            buffer3 = new BufferObject(BufferTarget.ArrayBuffer);
            buffer3.SetData(new byte[vertexCount * 1 * sizeof(byte)], BufferUsageHint.StaticDraw);

            // One byte of extra padding removed.
            buffer4 = new BufferObject(BufferTarget.ArrayBuffer);
            buffer4.SetData(new byte[(vertexCount * 5) - 1], BufferUsageHint.StaticDraw);
        }

        [TestMethod]
        public void InvaidAttributeNoBuffers()
        {
            var mesh = new MeshA();
            var e = Assert.ThrowsException<ArgumentException>(() =>
                mesh.ConfigureAttribute(new VertexFloatAttribute("attr1", ValueCount.Four, VertexAttribPointerType.Byte, false), "buffer1", 0, sizeof(byte) * 4));
            Assert.AreEqual("bufferName", e.ParamName);
            Assert.IsTrue(e.Message.Contains("The buffer buffer1 has not been added."));
        }

        [TestMethod]
        public void ValidAttributeSingleBuffer()
        {
            var mesh = new MeshA();
            mesh.AddBuffer("buffer1", buffer1);
            mesh.ConfigureAttribute(new VertexFloatAttribute("attr1", ValueCount.Four, VertexAttribPointerType.Byte, false), "buffer1", 0, sizeof(byte) * 4);
        }

        [TestMethod]
        public void InvalidAttributeNegativeOffset()
        {
            var mesh = new MeshA();
            mesh.AddBuffer("buffer1", buffer1);
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                mesh.ConfigureAttribute(new VertexFloatAttribute("attr1", ValueCount.Four, VertexAttribPointerType.Byte, false), "buffer1", -5, sizeof(byte) * 4));
            Assert.IsTrue(e.Message.Contains("One or more attribute data accesses will not be within the specified buffer's data storage"));
        }

        [TestMethod]
        public void InvalidAttributeNegativeStride()
        {
            var mesh = new MeshA();
            mesh.AddBuffer("buffer1", buffer1);
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
            mesh.AddBuffer("buffer1", buffer1);
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                mesh.ConfigureAttribute(new VertexFloatAttribute("attr1", ValueCount.Four, VertexAttribPointerType.Double, false), "buffer1", 0, sizeof(byte) * 4));
            Assert.IsTrue(e.Message.Contains("One or more attribute data accesses will not be within the specified buffer's data storage"));
        }

        [TestMethod]
        public void ValidAttributeSizeLessThanStride()
        {
            // The last element won't actually read one byte past the end of the buffer.
            var mesh = new MeshA();
            mesh.AddBuffer("buffer1", buffer4);
            mesh.ConfigureAttribute(new VertexFloatAttribute("attr1", ValueCount.Four, VertexAttribPointerType.Byte, false), "buffer1", 0, 5);
        }

        [TestMethod]
        public void ValidAttributeStrideTooLarge()
        {
            var mesh = new MeshA();
            mesh.AddBuffer("buffer1", buffer1);
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                mesh.ConfigureAttribute(new VertexFloatAttribute("attr1", ValueCount.Four, VertexAttribPointerType.Byte, false), "buffer1", 0, 5));
            Assert.IsTrue(e.Message.Contains("One or more attribute data accesses will not be within the specified buffer's data storage"));
        }

        [TestMethod]
        public void ValidAttributeMultipleBuffers()
        {
            // Buffer 2 and 3 wouldn't be valid.
            var mesh = new MeshA();
            mesh.AddBuffer("buffer2", buffer2);
            mesh.AddBuffer("buffer1", buffer1);
            mesh.AddBuffer("buffer3", buffer3);

            mesh.ConfigureAttribute(new VertexFloatAttribute("attr1", ValueCount.Four, VertexAttribPointerType.Byte, false), "buffer1", 0, sizeof(byte) * 4);
        }
    }
}
