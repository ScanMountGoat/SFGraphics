using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFGraphicsRenderTests.GLExtensionTests
{
    [TestClass()]
    public class ExtensionAvailabilityTests
    {
        [TestInitialize]
        public void SetUpExtensions()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.BindDummyContext();
            OpenGLExtensions.InitializeCurrentExtensions();
        }

        [TestMethod()]
        [Ignore]
        public void IsAvailableTest()
        {
            Assert.Fail("No extensions?");
        }
    }
}