using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Textures;

namespace SFGraphicsRenderTests.TextureTests
{
    public partial class TextureTest
    {
        [TestClass]
        public class ConstructorTests
        {
            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.BindDummyContext();
            }

            [TestMethod]

            public void GenerateId()
            {
                Texture2D texture = new Texture2D(1, 1);
                Assert.AreNotEqual(0, texture.Id);
            }
        }
    }
}
