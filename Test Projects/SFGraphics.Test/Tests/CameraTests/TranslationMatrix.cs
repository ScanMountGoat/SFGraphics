using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Cameras;
using OpenTK;

namespace CameraTests
{
    [TestClass]
    public class TranslationMatrix
    {
        [TestMethod]
        public void CameraAtOrigin()
        {
            Camera camera = new Camera();
            camera.Position = new Vector3(0);
            Assert.AreEqual(Matrix4.Identity, camera.TranslationMatrix);
        }

        [TestMethod]
        public void NonZeroPosition()
        {
            Camera camera = new Camera();
            camera.Position = new Vector3(1, 2, 3);
            Assert.AreEqual(Matrix4.CreateTranslation(1, -2, 3), camera.TranslationMatrix);
        }

        [TestMethod]
        public void PanNoScalingRenderDimensionsZero()
        {
            Camera camera = new Camera();
            camera.Position = new Vector3(0);
            camera.RenderHeight = 0;
            camera.RenderWidth = 0;
            camera.Pan(0, 0, false);

            // Check for divide by 0.
            Assert.AreEqual(Matrix4.Identity, camera.TranslationMatrix);
        }
    }
}
