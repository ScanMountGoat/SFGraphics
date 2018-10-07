using SFGraphics.Cameras;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;

namespace SFGenericModel.GenericModels
{
    /// <summary>
    /// Allows for drawing a collection of renderable meshes.
    /// </summary>
    public class GenericModel
    {
        /// <summary>
        /// The collection of meshes that 
        /// will be rendered during <see cref="Draw(Shader, Camera)"/>.
        /// </summary>
        public List<HideableMesh> Meshes { get; }

        /// <summary>
        /// Creates a model from <paramref name="meshes"/>.
        /// </summary>
        /// <param name="meshes">The meshes used for drawing</param>
        public GenericModel(List<HideableMesh> meshes)
        {
            Meshes = meshes;
        }

        /// <summary>
        /// Creates an empty model.
        /// </summary>
        public GenericModel()
        {
            Meshes = new List<HideableMesh>();
        }

        /// <summary>
        /// Draws all meshes.
        /// </summary>
        /// <param name="shader">The shader used for drawing</param>
        /// <param name="camera">The camera used for drawing all objects in <see cref="Meshes"/></param>
        public void Draw(Shader shader, Camera camera)
        {
            foreach (var mesh in Meshes)
            {
                if (mesh.Visible)
                    mesh.Mesh.Draw(shader, camera);
            }
        }

        /// <summary>
        /// Enables rendering for all meshes.
        /// </summary>
        public void DisplayAll()
        {
            foreach (var mesh in Meshes)
            {
                mesh.Visible = true;
            }
        }

        /// <summary>
        /// Disables rendering for all meshes.
        /// </summary>
        public void HideAll()
        {
            foreach (var mesh in Meshes)
            {
                mesh.Visible = false;
            }
        }
    }
}
