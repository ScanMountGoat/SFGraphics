using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.BufferObjects;
using OpenTK.Graphics.OpenGL;

namespace BufferObjectTests
{
    [TestClass]
    public class MapBuffer : BufferTest
    {
        [TestMethod]
        public void ReadFromPtr()
        {
            // Copy the buffer's data to a new array using its pointer.
            IntPtr pointer = buffer.MapBuffer(BufferAccess.ReadOnly);
            float[] readData = new float[originalData.Length];
            Marshal.Copy(pointer, readData, 0, originalData.Length);
            buffer.Unmap();

            CollectionAssert.AreEqual(originalData, readData);
        }

        [TestMethod]
        public void WriteToPtr()
        {
            float[] dataToWrite = new float[] { -1f, -1f, -1f };

            // Modify the buffer's data using its pointer.
            IntPtr pointer = buffer.MapBuffer(BufferAccess.ReadWrite);
            Marshal.Copy(dataToWrite, 0, pointer, dataToWrite.Length);
            buffer.Unmap();

            CollectionAssert.AreEqual(dataToWrite, buffer.GetData<float>());
        }
    }
}
