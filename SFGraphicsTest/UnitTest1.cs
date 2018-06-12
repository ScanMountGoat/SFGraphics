using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics;

namespace SFGraphicsTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(1, Class1.DoStuff());
            Assert.AreEqual(42, Class1.DoStuff());
        }
    }
}
