using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.VertexArrays;
using System.Threading;

namespace SFGenericModel
{
    /// <summary>
    /// Contains the basic functionality for drawing indexed vertex data with OpenGL.
    /// Derived classes should configure their own implemenation for storing and configuring vertex data.
    /// </summary>
    public abstract class GenericMeshBase
    {
        /// <summary>
        /// The number of vertices stored in the buffers used for drawing.
        /// </summary>
        public int VertexCount { get; }

        /// <summary>
        /// Determines how primitives will be constructed from the vertex data.
        /// </summary>
        public PrimitiveType PrimitiveType { get; }

        /// <summary>
        /// Specifies the data type of the index values.
        /// </summary>
        public DrawElementsType DrawElementsType { get; }

        // Vertex attributes need to be reconfigured when the thread or shader changes.
        private int previousShaderId = -1;
        private int previousThreadId = -1;

        private VertexArrayObject vertexArrayObject;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="primitiveType"></param>
        /// <param name="drawElementsType"></param>
        /// <param name="vertexCount">The number of vertices in this mesh</param>
        public GenericMeshBase(PrimitiveType primitiveType, DrawElementsType drawElementsType, int vertexCount)
        {
            PrimitiveType = primitiveType;
            DrawElementsType = drawElementsType;
            VertexCount = vertexCount;
        }

        /// <summary>
        /// Draws the geometry data.
        /// </summary>
        /// <param name="shader">The shader used for drawing</param>
        /// <param name="count">The number of vertices to draw</param>
        /// <param name="offset">The offset into the index buffer</param>
        public void Draw(Shader shader, int count, int offset)
        {
            if (!shader.LinkStatusIsOk)
                return;

            shader.UseProgram();

            // Only reconfigure the vertex attributes when necessary to improve performance.
            if (shader.Id != previousShaderId || Thread.CurrentThread.ManagedThreadId != previousThreadId)
            {
                ConfigureVao(shader);

                previousShaderId = shader.Id;
                previousThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            DrawGeometry(count, offset);
        }

        /// <summary>
        /// Draws the geometry data.
        /// </summary>
        /// <param name="shader">The shader used for drawing</param>
        public void Draw(Shader shader)
        {
            Draw(shader, VertexCount, 0);
        }

        /// <summary>
        /// Configures the attribute state for a <see cref="vertexArrayObject"/> prior to drawing.
        /// The appropriate buffers should be bound and appropriate calls to GL.VertexAttribPointer(...) and GL.VertexAttribIPointer should be made.
        /// <para></para><para></para>
        /// The <see cref="vertexArrayObject"/> is bound and unbound before and after calling this method, respectively.
        /// </summary>
        /// <param name="shader">The shader queried to get vertex attribute information</param>
        protected abstract void ConfigureVertexAttributes(Shader shader);

        private void DrawGeometry(int count, int offset)
        {
            vertexArrayObject.Bind();
            GL.DrawElements(PrimitiveType, count, DrawElementsType, offset);

            // TODO: This isn't part of the OpenGL core specification.
            // Leave this enabled for compatibility with older applications.
            vertexArrayObject.Unbind();
        }

        private void ConfigureVao(Shader shader)
        {
            // Recreate the object every time in case the thread has changed.
            vertexArrayObject = new VertexArrayObject();
            vertexArrayObject.Bind();
            ConfigureVertexAttributes(shader);
            vertexArrayObject.Unbind();
        }
    }
}
