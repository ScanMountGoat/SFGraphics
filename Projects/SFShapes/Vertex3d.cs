using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGenericModel.VertexAttributes;

namespace SFShapes
{
    /// <summary>
    /// A simple struct for rendering a 3d vertex.
    /// </summary>
    public struct Vertex3d
    {
        /// <summary>
        /// The position of the vertex
        /// </summary>
        [VertexFloatAttribute("position", ValueCount.Three, VertexAttribPointerType.Float, false)]
        public Vector3 Position { get; }

        /// <summary>
        /// Creates a new vertex from the given coordinates
        /// </summary>
        /// <param name="position">The vertex coordinates</param>
        public Vertex3d(Vector3 position)
        {
            Position = position;
        }

        /// <summary>
        /// Creates a new vertex from the given coordinates
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="z">The z coordinate</param>
        public Vertex3d(float x, float y, float z)
        {
            Position = new Vector3(x, y, z);
        }
    }
}