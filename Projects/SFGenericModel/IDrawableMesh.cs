using SFGraphics.GLObjects.Shaders;

namespace SFGenericModel
{
    /// <summary>
    /// Defines methods for drawing a mesh using programmable shaders.
    /// </summary>
    public interface IDrawableMesh
    {
        /// <summary>
        /// Draws the geometry using the specified shader and camera.
        /// </summary>
        /// <param name="shader">The shader used for drawing</param>
        void Draw(Shader shader);
    }
}
