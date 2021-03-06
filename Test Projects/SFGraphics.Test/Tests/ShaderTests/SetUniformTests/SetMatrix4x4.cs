﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.ShaderTests.SetterTests
{
    [TestClass]
    public class SetMatrix4x4 : ShaderTest
    {
        private static readonly float[] identityMatrix = {
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1
        };

        [TestMethod]
        public void ValidNameValidType()
        {
            Matrix4 matrix4 = Matrix4.Identity;
            shader.SetMatrix4x4("matrix4a", ref matrix4);
            Assert.IsTrue(IsValidSet("matrix4a", ActiveUniformType.FloatMat4));

            float[] values = GetMatrixValues("matrix4a");
            CollectionAssert.AreEqual(identityMatrix, values);
        }

        [TestMethod]
        public void ValidNameValidTypeNoRef()
        {
            shader.SetMatrix4x4("matrix4a", Matrix4.Identity);
            Assert.IsTrue(IsValidSet("matrix4a", ActiveUniformType.FloatMat4));

            float[] values = GetMatrixValues("matrix4a");
            CollectionAssert.AreEqual(identityMatrix, values);
        }

        [TestMethod]
        public void InvalidName()
        {
            Matrix4 matrix4 = Matrix4.Identity;
            shader.SetMatrix4x4("memes", ref matrix4);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.FloatMat4));
        }

        [TestMethod]
        public void InvalidType()
        {
            Matrix4 matrix4 = Matrix4.Identity;
            shader.SetMatrix4x4("float1", ref matrix4);
            Assert.IsFalse(IsValidSet("float1", ActiveUniformType.FloatMat4));
        }

        private float[] GetMatrixValues(string name)
        {
            float[] values = new float[16];
            GL.GetUniform(shader.Id, shader.GetUniformLocation(name), values);
            return values;
        }
    }
}
