using Microsoft.VisualStudio.TestTools.UnitTesting;
using RenderTestUtils;

namespace Tests
{
    [TestClass]
    public abstract class ContextTest
    {
        [TestInitialize]
        public virtual void Initialize()
        {
            // Set up the context for all the tests.
            OpenTKWindowlessContext.BindDummyContext();
        }
    }
}
