using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GlUtils;

namespace SFGraphics.Test.GLExtensionTests
{
    [TestClass]
    public class ExtensionAvailabilityTests : GraphicsContextTest
    {
        [TestMethod]
        public void CorrectName()
        {
            Assert.IsTrue(OpenGLExtensions.IsAvailable("GL_ARB_sampler_objects"));
        }

        [TestMethod]
        public void CorrectNameLowerCase()
        {
            Assert.IsTrue(OpenGLExtensions.IsAvailable("gl_arb_sampler_objects"));
        }

        [TestMethod]
        public void InvalidName()
        {
            Assert.IsFalse(OpenGLExtensions.IsAvailable("GL_dank_memes"));
        }
    }
}