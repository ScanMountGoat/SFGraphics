using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.ShaderTests.SetterTests
{
    [TestClass]
    public class SetVector4Array : ShaderTest
    {
        private readonly Vector4[] vector4Values = {
            new Vector4(1, 2, 3, 4),
            new Vector4(5, 6, 7, 8)
        };

        [TestMethod]
        public void ValidName()
        {
            shader.SetVector4("vector4Arr", vector4Values);
            Assert.IsTrue(IsValidSet("vector4Arr", ActiveUniformType.FloatVec4));

            var values = GetVector4Array("vector4Arr", vector4Values.Length);
            CollectionAssert.AreEqual(vector4Values, values);
        }


        [TestMethod]
        public void InvalidSize()
        {
            shader.SetVector4("vector4Arr", new Vector4[8]);
            Assert.IsFalse(IsValidSet("vector4Arr", ActiveUniformType.FloatVec4));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetVector4("memes", new Vector4[8]);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.FloatVec4));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetVector4("vector2Arr", new Vector4[8]);
            Assert.IsFalse(IsValidSet("vector2Arr", ActiveUniformType.FloatVec4));
        }

        private Vector4[] GetVector4Array(string name, int length)
        {
            // Array locations are sequential.
            Vector4[] values = new Vector4[length];
            for (int i = 0; i < length; i++)
            {
                float[] xyzw = new float[4];
                GL.GetUniform(shader.Id, shader.GetUniformLocation(name) + i, xyzw);
                values[i] = new Vector4(xyzw[0], xyzw[1], xyzw[2], xyzw[3]);
            }

            return values;
        }
    }
}
