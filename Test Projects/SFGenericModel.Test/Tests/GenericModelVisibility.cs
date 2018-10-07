using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGenericModel.GenericModels;
using SFGraphics.Cameras;
using SFGraphics.GLObjects.Shaders;

namespace RenderSettingsTests
{
    [TestClass]
    public class GenericModelVisibility
    {
        private class TestMesh : SFGenericModel.IDrawableMesh
        {
            public void Draw(Shader shader, Camera camera)
            {
                throw new System.NotImplementedException();
            }
        }

        [TestMethod]
        public void HideAll()
        {
            GenericModel model = new GenericModel();
            model.Meshes.Add(new HideableMesh(new TestMesh(), true));
            model.Meshes.Add(new HideableMesh(new TestMesh(), true));
            model.Meshes.Add(new HideableMesh(new TestMesh(), true));

            model.HideAll();
            foreach (var mesh in model.Meshes)
            {
                Assert.IsFalse(mesh.Visible);
            }
        }

        [TestMethod]
        public void DisplayAll()
        {
            GenericModel model = new GenericModel();
            model.Meshes.Add(new HideableMesh(new TestMesh(), false));
            model.Meshes.Add(new HideableMesh(new TestMesh(), false));
            model.Meshes.Add(new HideableMesh(new TestMesh(), false));

            model.DisplayAll();
            foreach (var mesh in model.Meshes)
            {
                Assert.IsTrue(mesh.Visible);
            }
        }
    }
}
