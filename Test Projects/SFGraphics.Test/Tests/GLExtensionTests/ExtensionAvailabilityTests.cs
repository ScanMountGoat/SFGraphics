using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Tools;

namespace GLExtensionTests
{
    [TestClass()]
    public class ExtensionAvailabilityTests
    {
        [TestInitialize]
        public void SetUpExtensions()
        {
            // Set up the context for all the tests.
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
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