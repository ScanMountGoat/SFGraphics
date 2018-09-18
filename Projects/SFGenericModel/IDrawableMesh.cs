using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.Cameras;

namespace SFGenericModel
{
    /// <summary>
    /// Defines methods for drawing a mesh using programable shaders.
    /// </summary>
    public interface IDrawableMesh
    {
        /// <summary>
        /// Draws the geometry using the specified shader and camera.
        /// </summary>
        /// <param name="shader">The shader used for drawing</param>
        /// <param name="camera">The camera used to transform vertex positions</param>
        void Draw(Shader shader, Camera camera);
    }
}
