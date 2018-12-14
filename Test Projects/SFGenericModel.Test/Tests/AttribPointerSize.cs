using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGenericModel.VertexAttributes;

namespace AttribPointerUtilsTest
{
    [TestClass]
    public class AttribPointerSize
    {
        [TestMethod]
        public void Byte()
        {
            CheckAttribPointerSize(sizeof(byte), VertexAttribPointerType.Byte);
        }

        [TestMethod]
        public void UnsignedByte()
        {
            CheckAttribPointerSize(sizeof(byte), VertexAttribPointerType.UnsignedByte);
        }

        [TestMethod]
        public void Int()
        {
            CheckAttribPointerSize(sizeof(int), VertexAttribPointerType.Int);
        }

        [TestMethod]
        public void UnsignedInt()
        {
            CheckAttribPointerSize(sizeof(int), VertexAttribPointerType.UnsignedInt);
        }

        [TestMethod]
        public void Short()
        {
            CheckAttribPointerSize(sizeof(short), VertexAttribPointerType.Short);
        }

        [TestMethod]
        public void UnsignedShort()
        {
            CheckAttribPointerSize(sizeof(short), VertexAttribPointerType.UnsignedShort);
        }

        [TestMethod]
        public void Float()
        {
            CheckAttribPointerSize(sizeof(float), VertexAttribPointerType.Float);
        }

        [TestMethod]
        public void HalfFloat()
        {
            CheckAttribPointerSize(sizeof(float) / 2, VertexAttribPointerType.HalfFloat);
        }

        [TestMethod]
        public void Double()
        {
            CheckAttribPointerSize(sizeof(double), VertexAttribPointerType.Double);
        }

        private static void CheckAttribPointerSize(int expected, VertexAttribPointerType type)
        {
            Assert.AreEqual(expected, AttribPointerUtils.GetSizeInBytes(type));
        }
    }
}
