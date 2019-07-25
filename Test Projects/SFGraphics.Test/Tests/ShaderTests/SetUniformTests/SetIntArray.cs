using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.ShaderTests.SetterTests
{
    [TestClass]
    public class SetIntArray : ShaderTest
    {
        private readonly int[] intValues = { -1, 0, 1 };

        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetInt("intArray1", intValues);
            Assert.IsTrue(IsValidSet("intArray1", ActiveUniformType.Int));

            var values = GetIntArray("intArray1", intValues.Length);
            CollectionAssert.AreEqual(intValues, values);
        }

        [TestMethod]
        public void InvalidSize()
        {
            shader.SetInt("intArray1", new int[8]);
            Assert.IsFalse(IsValidSet("intArray1", ActiveUniformType.Int));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetInt("memes", intValues);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.Int));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetInt("float1", intValues);
            Assert.IsFalse(IsValidSet("float1", ActiveUniformType.Int));
        }

        private int[] GetIntArray(string name, int length)
        {
            // Array locations are sequential.
            int[] values = new int[length];
            for (int i = 0; i < length; i++)
                GL.GetUniform(shader.Id, shader.GetUniformLocation(name) + i, out values[i]);

            return values;
        }
    }
}
