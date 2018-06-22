using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Cameras;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;

namespace SFGraphicsTest
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
                OpenTKWindowlessContext.CreateDummyContext();
            }

            [TestMethod]
            public void GenerateId()
            {
                Texture texture = new Texture2D(1, 1);
                Assert.AreNotEqual(0, texture.Id);
            }
        }
    }
}
