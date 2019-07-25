using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SFGraphics.Cameras;

namespace SFGraphics.Test.CameraTests
{
    [TestClass]
    public class TranslationMatrix
    {
        [TestMethod]
        public void CameraAtOrigin()
        {
            Camera camera = new Camera
            {
                Translation = new Vector3(0)
            };
            Assert.AreEqual(Matrix4.Identity, camera.TranslationMatrix);
        }

        [TestMethod]
        public void NonZeroPosition()
        {
            Camera camera = new Camera
            {
                Translation = new Vector3(1, 2, 3)
            };
            Assert.AreEqual(Matrix4.CreateTranslation(1, -2, 3), camera.TranslationMatrix);
        }

        [TestMethod]
        public void PanNoScalingRenderDimensionsZero()
        {
            Camera camera = new Camera
            {
                Translation = new Vector3(0),
                RenderHeight = 0,
                RenderWidth = 0
            };
            camera.Pan(0, 0, false);

            // Check for divide by 0.
            Assert.AreEqual(Matrix4.Identity, camera.TranslationMatrix);
        }
    }
}
