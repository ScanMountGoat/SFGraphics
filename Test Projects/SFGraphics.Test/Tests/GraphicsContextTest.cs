using Microsoft.VisualStudio.TestTools.UnitTesting;
using RenderTestUtils;

namespace SFGraphics.Test
{
    [TestClass]
    public abstract class GraphicsContextTest
    {
        [TestInitialize]
        public virtual void Initialize()
        {
            // Set up the context for all the tests.
            OpenTKWindowlessContext.BindDummyContext();
        }
    }
}
