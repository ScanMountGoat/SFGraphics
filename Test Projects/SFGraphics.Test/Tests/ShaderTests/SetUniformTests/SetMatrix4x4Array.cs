using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace SFGraphics.Test.ShaderTests.SetterTests
{
    [TestClass]
    public class SetMatrix4x4Arrays : ShaderTest
    {
        private readonly Matrix4[] matrix4Values = new Matrix4[]
        {
            Matrix4.Identity,
            Matrix4.Identity * 2
        };

        private static readonly float[] expectedValues = new float[]
        {
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1,
            2, 0, 0, 0,
            0, 2, 0, 0,
            0, 0, 2, 0,
            0, 0, 0, 2
        };

        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetMatrix4x4("matrix4Arr", matrix4Values);
            Assert.IsTrue(IsValidSet("matrix4Arr", ActiveUniformType.FloatMat4));

            var values = GetMatrixArray("matrix4Arr", matrix4Values.Length);
            CollectionAssert.AreEqual(expectedValues, values);
        }

        [TestMethod]
        public void InvalidSize()
        {
            shader.SetMatrix4x4("matrix4Arr", new Matrix4[8]);
            Assert.IsFalse(IsValidSet("matrix4Arr", ActiveUniformType.FloatMat4));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetMatrix4x4("memes", matrix4Values);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.FloatMat4));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetMatrix4x4("vector2Arr", new Matrix4[8]);
            Assert.IsFalse(IsValidSet("vector2Arr", ActiveUniformType.FloatMat4));
        }

        private float[] GetMatrixArray(string name, int length)
        {
            var result = new System.Collections.Generic.List<float>(16 * length);
            for (int i = 0; i < length; i++)
            {
                float[] matrixValues = new float[16];
                GL.GetUniform(shader.Id, shader.GetUniformLocation(name) + i, matrixValues);
                result.AddRange(matrixValues);
            }
            return result.ToArray();
        }
    }
}
