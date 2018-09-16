using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;

namespace Tests
{
    [TestClass]
    public abstract class ContextTest
    {
        [TestInitialize()]
        public virtual void Initialize()
        {
            // Set up the context for all the tests.
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }
    }
}
