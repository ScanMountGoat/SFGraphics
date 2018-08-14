using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using SFGraphics.GLObjects.GLObjectManagement;

namespace SFGraphicsTest.Tests.ReferenceCountTests
{
    [TestClass]
    public class IncrementRefTest
    {
        [TestMethod]
        public void AddNewReference()
        {
            ConcurrentDictionary<string, int> refCountByName = new ConcurrentDictionary<string, int>();
            ReferenceCounting.IncrementReference(refCountByName, "memes");

            Assert.AreEqual(1, refCountByName["memes"]);
        }

        [TestMethod]
        public void IncrementExistingReference()
        {
            ConcurrentDictionary<string, int> refCountByName = new ConcurrentDictionary<string, int>();
            ReferenceCounting.IncrementReference(refCountByName, "memes");
            ReferenceCounting.IncrementReference(refCountByName, "memes");

            Assert.AreEqual(2, refCountByName["memes"]);
        }
    }
}
