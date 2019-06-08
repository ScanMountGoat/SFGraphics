using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.ShaderTests.SetterTests
{
    [TestClass]
    public class SetUintArray : ShaderTest
    {
        private readonly uint[] uintValues = new uint[] { 1, 2, 3 };

        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetUint("uintArray1", uintValues);
            Assert.IsTrue(IsValidSet("uintArray1", ActiveUniformType.UnsignedInt));

            var values = GetUintArray("uintArray1", uintValues.Length);
            CollectionAssert.AreEqual(uintValues, values);
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetUint("memes", uintValues);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.UnsignedInt));
        }

        [TestMethod]
        public void InvalidSize()
        {
            shader.SetUint("uintArray1", new uint[8]);
            Assert.IsFalse(IsValidSet("uintArray1", ActiveUniformType.UnsignedInt));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetUint("floatArray1", uintValues);
            Assert.IsFalse(IsValidSet("floatArray1", ActiveUniformType.UnsignedInt));
        }

        private uint[] GetUintArray(string name, int length)
        {
            // Array locations are sequential.
            uint[] values = new uint[length];
            for (int i = 0; i < length; i++)
            {
                GL.GetUniform(shader.Id, shader.GetUniformLocation(name) + i, out int value);
                values[i] = System.BitConverter.ToUInt32(System.BitConverter.GetBytes(value), 0);
            }

            return values;
        }
    }
}
