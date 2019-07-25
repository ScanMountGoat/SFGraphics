using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.ShaderTests.SetterTests
{
    [TestClass]
    public class SetVector2Array : ShaderTest
    {
        private readonly Vector2[] vector2Values = 
        {
            new Vector2(1, 2),
            new Vector2(3, 4)
        };

        [TestMethod]
        public void ValidName()
        {
            shader.SetVector2("vector2Arr", vector2Values);
            Assert.IsTrue(IsValidSet("vector2Arr", ActiveUniformType.FloatVec2));

            var values = GetVector2Array("vector2Arr", 2);
            CollectionAssert.AreEqual(vector2Values, values);
        }

        [TestMethod]
        public void InvalidSize()
        {
            shader.SetVector2("vector2Arr", new Vector2[8]);
            Assert.IsFalse(IsValidSet("vector2Arr", ActiveUniformType.FloatVec2));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetVector2("memes", new Vector2[8]);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.FloatVec2));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetVector2("vector4Arr", new Vector2[8]);
            Assert.IsFalse(IsValidSet("vector4Arr", ActiveUniformType.FloatVec2));
        }

        private Vector2[] GetVector2Array(string name, int length)
        {
            // Array locations are sequential.
            Vector2[] values = new Vector2[length];
            for (int i = 0; i < length; i++)
            {
                float[] xy = new float[2];
                GL.GetUniform(shader.Id, shader.GetUniformLocation(name) + i, xy);
                values[i] = new Vector2(xy[0], xy[1]);
            }

            return values;
        }
    }
}
