using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using SFGraphics.GLObjects.GLObjectManagement;

namespace SFGraphics.Test.ReferenceCountTests
{
    [TestClass]
    public class RemoveReference
    {
        [TestMethod]
        public void TryDecrementInvalidReference()
        {
            // Doesn't throw exception.
            var refCountByName = new ConcurrentDictionary<string, int>();

            ReferenceCounting.RemoveReference(refCountByName, "memes");

            Assert.IsFalse(refCountByName.ContainsKey("memes"));
        }

        [TestMethod]
        public void DecrementExistingReference()
        {
            var refCountByName = new ConcurrentDictionary<string, int>();

            ReferenceCounting.AddReference(refCountByName, "memes");
            ReferenceCounting.RemoveReference(refCountByName, "memes");

            Assert.AreEqual(0, refCountByName["memes"]);
        }

        [TestMethod]
        public void DecrementZeroReference()
        {
            var refCountByName = new ConcurrentDictionary<string, int>();

            ReferenceCounting.AddReference(refCountByName, "memes");
            ReferenceCounting.RemoveReference(refCountByName, "memes");
            ReferenceCounting.RemoveReference(refCountByName, "memes");

            Assert.AreEqual(0, refCountByName["memes"]);
        }
    }
}
