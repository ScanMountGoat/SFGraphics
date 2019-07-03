using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using SFGraphics.GLObjects.GLObjectManagement;

namespace SFGraphics.Test.ReferenceCountTests
{
    [TestClass]
    public class AddReference
    {
        [TestMethod]
        public void AddNewReference()
        {
            ConcurrentDictionary<string, int> refCountByName = new ConcurrentDictionary<string, int>();
            ReferenceCounting.AddReference(refCountByName, "memes");

            Assert.AreEqual(1, refCountByName["memes"]);
        }

        [TestMethod]
        public void IncrementExistingReference()
        {
            ConcurrentDictionary<string, int> refCountByName = new ConcurrentDictionary<string, int>();
            ReferenceCounting.AddReference(refCountByName, "memes");
            ReferenceCounting.AddReference(refCountByName, "memes");

            Assert.AreEqual(2, refCountByName["memes"]);
        }
    }
}
