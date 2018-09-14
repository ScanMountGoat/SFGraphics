using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace BufferObjectTests
{
    [TestClass]
    public class BufferData : BufferTest
    {
        [TestMethod]
        public void GetFloats()
        {
            float[] readData = buffer.GetData<float>();
            CollectionAssert.AreEqual(originalData, readData);
        }
        
        [TestMethod]
        public void GetVector3FromFloats()
        {
            // The 3 floats should make a single Vector3 struct.
            Vector3[] expectedData = new Vector3[] { new Vector3(1.5f, 2.5f, 3.5f) };
            Vector3[] readData = buffer.GetData<Vector3>();
            CollectionAssert.AreEqual(expectedData, readData);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void GetVector4FromFloats()
        {
            Vector4[] readData = buffer.GetData<Vector4>();
        }
    }
}
