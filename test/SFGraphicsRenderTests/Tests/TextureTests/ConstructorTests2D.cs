using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Textures;
using OpenTK.Graphics.OpenGL;

namespace SFGraphicsRenderTests.TextureTests
{
    public partial class TextureTest
    {
        [TestClass]
        public class ConstructorTests2D
        {
            private static readonly List<byte[]> mipmaps = new List<byte[]>
            {
                new byte[16]
            };

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

            [TestMethod]
            public void CompressedTextureCorrectFormat()
            {
                // Doesn't throw an exception.
                Texture2D texture = new Texture2D(1, 1, mipmaps, InternalFormat.CompressedRg11Eac);
            }

            [TestMethod]
            public void CompressedTextureIncorrectFormat()
            {
                // Doesn't throw an exception.
                Texture2D texture = new Texture2D(1, 1, mipmaps, InternalFormat.Rgb);
            }
        }
    }
}
