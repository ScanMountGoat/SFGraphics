using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SFGraphics.Controls.Test.GLViewportTests
{
    [TestClass]
    public class Dispose
    {
        [TestMethod]
        public void DisposeRenderingNotStarted()
        {
            var viewport = new GLViewport();
            viewport.Dispose();
        }

        [TestMethod]
        public void DisposeRenderingStarted()
        {
            var viewport = new GLViewport();
            viewport.RestartRendering();
            viewport.Dispose();
        }
    }
}
