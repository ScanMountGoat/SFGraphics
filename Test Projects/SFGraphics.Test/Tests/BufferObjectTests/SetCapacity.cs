﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.BufferObjectTests
{
    [TestClass]
    public class SetCapacity : BufferTest
    {
        [TestMethod]
        public void EmptyBuffer()
        {
            buffer.SetCapacity(0, BufferUsageHint.StaticDraw);
            Assert.AreEqual(0, buffer.SizeInBytes);
        }

        [TestMethod]
        public void ValidCapacity()
        {
            buffer.SetCapacity(0, BufferUsageHint.StaticDraw);
            buffer.SetCapacity(1024, BufferUsageHint.StaticDraw);

            // Shouldn't throw exception.
            buffer.SetSubData(new float[3], 50);
            Assert.AreEqual(1024, buffer.SizeInBytes);
        }

        [TestMethod]
        public void NegativeCapacity()
        {
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() => buffer.SetCapacity(-5, BufferUsageHint.StaticDraw));
            Assert.AreEqual("sizeInBytes", e.ParamName);
            Assert.IsTrue(e.Message.Contains("The buffer size must be non negative."));
        }
    }
}
