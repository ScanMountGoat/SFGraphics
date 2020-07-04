using OpenTK.Graphics.OpenGL;
using SFGenericModel.Utils;
using SFGraphics.GLObjects.BufferObjects;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.VertexArrays;
using System.Linq;
using System.Threading;

namespace SFGenericModel
{
    /// <summary>
    /// Contains the basic functionality for drawing indexed vertex data with OpenGL, including the element array buffer.
    /// Derived classes should configure their own implemenation for vertex array buffers.
    /// </summary>
    public abstract class GenericMeshBase
    {
        private readonly BufferObject vertexIndexBuffer = new BufferObject(BufferTarget.ElementArrayBuffer);

        /// <summary>
        /// The number of actual vertices to draw.
        /// The vertex data buffers may have fewer elements if vertices are shared.
        /// </summary>
        public int VertexIndexCount { get; }

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
        /// Initializes the index buffer for a mesh with unsigned indices.
        /// A total of <paramref name="vertexCount"/> unique indices are generated to initialize the index buffer.
        /// </summary>
        /// <param name="vertexCount">The number of vertices</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        public GenericMeshBase(int vertexCount, PrimitiveType primitiveType)
        {
            PrimitiveType = primitiveType;
            DrawElementsType = DrawElementsType.UnsignedInt;
            VertexIndexCount = vertexCount;

            // TODO: There may be an issue with interpreting signed integers as unsigned integers.
            // Models will likely never have enough vertices for this to be an issue.
            var vertexIndices = IndexUtils.GenerateIndices(vertexCount);
            vertexIndexBuffer.SetData(vertexIndices, BufferUsageHint.StaticDraw);
        }

        /// <summary>
        /// Initializes the index buffer for a mesh with unsigned indices.
        /// </summary>
        /// <param name="vertexIndices">The vertex indices used for drawing</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        public GenericMeshBase(uint[] vertexIndices, PrimitiveType primitiveType)
        {
            PrimitiveType = primitiveType;
            DrawElementsType = DrawElementsType.UnsignedInt;
            VertexIndexCount = vertexIndices.Length;

            vertexIndexBuffer.SetData(vertexIndices, BufferUsageHint.StaticDraw);
        }

        /// <summary>
        /// Initializes the index buffer for a mesh with unsigned indices.
        /// </summary>
        /// <param name="vertexIndices">The vertex indices used for drawing</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        public GenericMeshBase(ushort[] vertexIndices, PrimitiveType primitiveType)
        {
            PrimitiveType = primitiveType;
            DrawElementsType = DrawElementsType.UnsignedShort;
            VertexIndexCount = vertexIndices.Length;

            vertexIndexBuffer.SetData(vertexIndices, BufferUsageHint.StaticDraw);
        }

        /// <summary>
        /// Initializes the index buffer for a mesh with unsigned indices.
        /// </summary>
        /// <param name="vertexIndices">The vertex indices used for drawing</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        public GenericMeshBase(byte[] vertexIndices, PrimitiveType primitiveType)
        {
            PrimitiveType = primitiveType;
            DrawElementsType = DrawElementsType.UnsignedByte;
            VertexIndexCount = vertexIndices.Length;

            vertexIndexBuffer.SetData(vertexIndices, BufferUsageHint.StaticDraw);
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
            Draw(shader, VertexIndexCount, 0);
        }

        /// <summary>
        /// Configures the attribute state for a <see cref="vertexArrayObject"/> prior to drawing.
        /// The appropriate buffers should be bound and appropriate calls to GL.VertexAttribPointer(...) and GL.VertexAttribIPointer should be made.
        /// <para></para><para></para>
        /// The <see cref="vertexArrayObject"/> is bound and unbound before and after calling this method, respectively.
        /// This class handles the creation and binding of the index buffer.
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

            // Associate the bound buffers and attribute state with the vao.
            vertexIndexBuffer.Bind();
            ConfigureVertexAttributes(shader);

            vertexArrayObject.Unbind();
        }
    }
}
