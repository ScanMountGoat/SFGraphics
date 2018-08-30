using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SFGraphics.GLObjects.GLObjectManagement;

namespace SFGraphicsTest.Tests.ReferenceCountTests
{
    [TestClass]
    public class GetObjectsWithNoReferencesTests
    {
        [TestMethod]
        public void OneObjectWithNoReferences()
        {
            ConcurrentDictionary<string, int> refCountByName = new ConcurrentDictionary<string, int>();
            refCountByName.TryAdd("a", 1);
            refCountByName.TryAdd("b", 0);
            refCountByName.TryAdd("c", 1);

            HashSet<string> namesWithNoReferences = ReferenceCounting.GetObjectsWithNoReferences(refCountByName);
            Assert.AreEqual(1, namesWithNoReferences.Count);
            Assert.IsTrue(namesWithNoReferences.Contains("b"));
        }

        [TestMethod]
        public void DuplicateObjecstWithNoReferences()
        {
            ConcurrentDictionary<string, int> refCountByName = new ConcurrentDictionary<string, int>();
            refCountByName.TryAdd("a", 0);
            refCountByName.TryAdd("a", 0);
            refCountByName.TryAdd("a", 0);

            // Names are unique, so there is no need to delete an OpenGL object more than once.
            HashSet<string> namesWithNoReferences = ReferenceCounting.GetObjectsWithNoReferences(refCountByName);
            Assert.AreEqual(1, namesWithNoReferences.Count);
            Assert.IsTrue(namesWithNoReferences.Contains("a"));
        }

        [TestMethod]
        public void EmptyDictionary()
        {
            ConcurrentDictionary<string, int> refCountByName = new ConcurrentDictionary<string, int>();

            HashSet<string> namesWithNoReferences = ReferenceCounting.GetObjectsWithNoReferences(refCountByName);
            Assert.AreEqual(0, namesWithNoReferences.Count);
        }
    }
}
