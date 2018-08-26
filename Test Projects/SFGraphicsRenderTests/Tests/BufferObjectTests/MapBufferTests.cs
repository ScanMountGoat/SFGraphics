﻿using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects;
using OpenTK.Graphics.OpenGL;

namespace SFGraphicsRenderTests.BufferObjectTests
{
    [TestClass]
    public class MapBufferTests
    {
        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void MapBufferRead()
        {
            float[] inputData = new float[] { 1.5f, 2.5f, 3.5f };
            BufferObject buffer = new BufferObject(BufferTarget.ArrayBuffer);
            buffer.SetData(inputData, sizeof(float), BufferUsageHint.StaticDraw);

            // Copy the buffer's data to a new array using its pointer.
            IntPtr pointer = buffer.MapBuffer(BufferAccess.ReadOnly);
            float[] readData = new float[inputData.Length];
            Marshal.Copy(pointer, readData, 0, inputData.Length);
            buffer.Unmap();

            CollectionAssert.AreEqual(inputData, readData);
        }

        [TestMethod]
        public void MapBufferWrite()
        {
            float[] inputData = new float[] { 1.5f, 2.5f, 3.5f };
            BufferObject buffer = new BufferObject(BufferTarget.ArrayBuffer);
            buffer.SetData(inputData, sizeof(float), BufferUsageHint.StaticDraw);

            float[] dataToWrite = new float[] { -1f, -1f, -1f };

            // Modify the buffer's data using its pointer.
            IntPtr pointer = buffer.MapBuffer(BufferAccess.ReadWrite);
            Marshal.Copy(dataToWrite, 0, pointer, dataToWrite.Length);
            buffer.Unmap();

            CollectionAssert.AreEqual(dataToWrite, buffer.GetData<float>());
        }
    }
}