using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using SFGraphics.GLObjects.GLObjectManagement;

namespace ReferenceCountTests
{
    [TestClass]
    public class DecrementRefTest
    {
        [TestMethod]
        public void TryDecrementInvalidReference()
        {
            // Doesn't throw exception.
            ConcurrentDictionary<string, int> refCountByName = new ConcurrentDictionary<string, int>();
            ReferenceCounting.DecrementReference(refCountByName, "memes");

            Assert.IsFalse(refCountByName.ContainsKey("memes"));
        }

        [TestMethod]
        public void DecrementExistingReference()
        {
            ConcurrentDictionary<string, int> refCountByName = new ConcurrentDictionary<string, int>();
            ReferenceCounting.IncrementReference(refCountByName, "memes");
            ReferenceCounting.DecrementReference(refCountByName, "memes");

            Assert.AreEqual(0, refCountByName["memes"]);
        }
    }
}
