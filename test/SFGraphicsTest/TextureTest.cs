using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Cameras;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;

namespace SFGraphicsTest.TextureTests
{
    [TestClass]
    public class TextureTest
    {
        [TestClass]
        public class ConstructorTests
        {
            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.CreateDummyContext();
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
