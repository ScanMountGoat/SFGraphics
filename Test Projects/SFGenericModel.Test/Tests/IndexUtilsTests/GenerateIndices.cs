using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SFGenericModel.Utils.Test.IndexUtilsTests
{
    [TestClass]
    public class GenerateIndices
    {
        [TestMethod]
        public void ZeroCount()
        {
            var indices = IndexUtils.GenerateIndices(0);
            Assert.AreEqual(0, indices.Length);
        }

        [TestMethod]
        public void NonZeroCount()
        {
            var indices = IndexUtils.GenerateIndices(3);
            CollectionAssert.AreEqual(new List<int>() { 0, 1, 2 }, indices);
        }
    }
}
