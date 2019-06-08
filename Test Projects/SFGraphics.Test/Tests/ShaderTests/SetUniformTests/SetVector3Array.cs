using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.ShaderTests.SetterTests
{
    [TestClass]
    public class SetVector3Array : ShaderTest
    {
        private readonly Vector3[] vector3Values = new Vector3[]
        {
            new Vector3(1, 2, 3),
            new Vector3(4, 5, 6)
        };

        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetVector3("vector3Arr", vector3Values);
            Assert.IsTrue(IsValidSet("vector3Arr", ActiveUniformType.FloatVec3));

            var values = GetVector3Array("vector3Arr", vector3Values.Length);
            CollectionAssert.AreEqual(vector3Values, values);
        }

        [TestMethod]
        public void InvalidSize()
        {
            shader.SetVector3("vector3Arr", new Vector3[8]);
            Assert.IsFalse(IsValidSet("vector3Arr", ActiveUniformType.FloatVec3));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetVector3("memes", new Vector3[8]);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.FloatVec3));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetVector3("vector2Arr", new Vector3[8]);
            Assert.IsFalse(IsValidSet("vector2Arr", ActiveUniformType.FloatVec3));
        }

        private Vector3[] GetVector3Array(string name, int length)
        {
            // Array locations are sequential.
            Vector3[] values = new Vector3[length];
            for (int i = 0; i < length; i++)
            {
                float[] xyz = new float[3];
                GL.GetUniform(shader.Id, shader.GetUniformLocation(name) + i, xyz);
                values[i] = new Vector3(xyz[0], xyz[1], xyz[2]);
            }

            return values;
        }
    }
}
