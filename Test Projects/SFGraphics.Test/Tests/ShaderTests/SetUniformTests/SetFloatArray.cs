using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.ShaderTests.SetterTests
{
    [TestClass]
    public class SetFloatArray : ShaderTest
    {
        private readonly float[] floatValues = new float[] { 1.5f, 2.5f, 3.5f };

        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetFloat("floatArray1", floatValues);
            Assert.IsTrue(IsValidSet("floatArray1", ActiveUniformType.Float));

            float[] values = GetFloatArray("floatArray1", floatValues.Length);
            CollectionAssert.AreEqual(floatValues, values);
        }

        [TestMethod]
        public void InvalidSize()
        {
            shader.SetFloat("floatArray1", new float[8]);
            Assert.IsFalse(IsValidSet("floatArray1", ActiveUniformType.Float));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetFloat("intArray1", floatValues);
            Assert.IsFalse(IsValidSet("intArray1", ActiveUniformType.Float));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetFloat("memesArray", floatValues);
            Assert.IsFalse(IsValidSet("memesArray", ActiveUniformType.Float));
        }

        private float[] GetFloatArray(string name, int length)
        {
            // Array locations are sequential.
            float[] values = new float[length];
            for (int i = 0; i < length; i++)
                GL.GetUniform(shader.Id, shader.GetUniformLocation(name) + i, out values[i]);

            return values;
        }
    }
}
