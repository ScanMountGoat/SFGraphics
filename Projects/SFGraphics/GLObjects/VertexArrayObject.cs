using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects
{
    /// <summary>
    /// Encapsulates an OpenGL vertex array object. Vertex array objects cannot be shared between contexts.
    /// </summary>
    public sealed class VertexArrayObject : GLObject
    {
        /// <summary>
        /// Returns the type of OpenGL object. Used for memory management.
        /// </summary>
        public override GLObjectType ObjectType { get { return GLObjectType.VertexArray; } }

        /// <summary>
        /// Creates an empty vertex array object.
        /// The vertex array object must first be bound with <see cref="Bind"/>.
        /// </summary>
        public VertexArrayObject() : base(GL.GenVertexArray())
        {

        }

        /// <summary>
        /// Binds the vertex array <see cref="GLObject.Id"/>
        /// </summary>
        public void Bind()
        {
            GL.BindVertexArray(Id);
        }

        /// <summary>
        /// Binds the default vertex array value of 0.
        /// </summary>
        public void Unbind()
        {
            GL.BindVertexArray(0);
        }
    }
}
