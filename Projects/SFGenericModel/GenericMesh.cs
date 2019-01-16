using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGenericModel.Materials;
using SFGenericModel.MeshEventArgs;
using SFGenericModel.RenderState;
using SFGenericModel.Utils;
using SFGenericModel.VertexAttributes;
using SFGraphics.Cameras;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.VertexArrays;
using System.Collections.Generic;
using SFGraphics.GLObjects.BufferObjects;

namespace SFGenericModel
{
    /// <summary>
    /// A vertex container that supports drawing indexed vertex data using a user defined 
    /// vertex struct <typeparamref name="T"/>.
    /// <para></para><para></para>
    /// Inherit from this class to override the default 
    /// materials and rendering state.
    /// </summary>
    /// <typeparam name="T">The struct used to define vertex data</typeparam>
    public abstract class GenericMesh<T> : IDrawableMesh where T : struct
    {
        // Vertex attributes only need to be reconfigured when the shader changes.
        private int previousShaderId = -1;

        private readonly int vertexSizeInBytes = 0;

        private VertexArrayObject vertexArrayObject = new VertexArrayObject();

        private BufferObject vertexBuffer = new BufferObject(BufferTarget.ArrayBuffer);
        private BufferObject vertexIndexBuffer = new BufferObject(BufferTarget.ElementArrayBuffer);

        /// <summary>
        /// The number of vertices stored in the buffers used for drawing.
        /// </summary>
        public int VertexCount { get; } = 0;

        /// <summary>
        /// Determines how primitives will be constructed from the vertex data.
        /// </summary>
        public PrimitiveType PrimitiveType { get; }

        /// <summary>
        /// Specifies the data type of the index values.
        /// </summary>
        public DrawElementsType DrawElementsType { get; }

        /// <summary>
        /// The collection of OpenGL state set prior to drawing.
        /// </summary>
        protected RenderSettings renderSettings = new RenderSettings();

        /// <summary>
        /// The collection of shader uniforms updated during drawing.
        /// </summary>
        protected GenericMaterial material = new GenericMaterial();

        /// <summary>
        /// Contains information about the arguments used to set a vertex attribute.
        /// </summary>
        /// <param name="sender">The <see cref="GenericMesh{T}"/> 
        /// instance that generated the error</param>
        /// <param name="e">The vertex attribute information</param>
        public delegate void InvalidAttribSetEventHandler(object sender, AttribSetEventArgs e);

        /// <summary>
        /// Occurs when specified vertex attribute information does not match the shader.
        /// </summary>
        public event InvalidAttribSetEventHandler OnInvalidAttribSet;

        private GenericMesh(PrimitiveType primitiveType, DrawElementsType drawElementsType, System.Type vertexType, int vertexCount)
        {
            PrimitiveType = primitiveType;
            DrawElementsType = DrawElementsType.UnsignedInt;

            // This works as expected as long as only value types are used.
            vertexSizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(vertexType);
            VertexCount = vertexCount;
        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// An index is generated for each vertex in <paramref name="vertices"/>.
        /// </summary>
        /// <param name="vertices">The vertex data</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        public GenericMesh(List<T> vertices, PrimitiveType primitiveType) 
            : this(primitiveType, DrawElementsType.UnsignedInt, typeof(T), vertices.Count)
        {
            InitializeBufferData(vertices);
        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// </summary>
        /// <param name="vertices">The vertex data</param>
        /// <param name="vertexIndices">The vertex index data</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        public GenericMesh(List<T> vertices, List<int> vertexIndices, PrimitiveType primitiveType)
            : this(primitiveType, DrawElementsType.UnsignedInt, typeof(T), vertexIndices.Count)
        {
            InitializeBufferData(vertices, vertexIndices);
        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// </summary>
        /// <param name="vertices">The vertex data</param>
        /// <param name="vertexIndices">The vertex index data</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        public GenericMesh(List<T> vertices, List<uint> vertexIndices, PrimitiveType primitiveType)
            : this(primitiveType, DrawElementsType.UnsignedInt, typeof(T), vertexIndices.Count)
        {
            InitializeBufferData(vertices, vertexIndices);
        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// </summary>
        /// <param name="vertexData">Contains the vertices, indices, and primitive type</param>
        public GenericMesh(IndexedVertexData<T> vertexData) 
            : this(vertexData.Vertices, vertexData.Indices, vertexData.PrimitiveType)
        {

        }

        /// <summary>
        /// Sets the uniforms, sets render state, and draws the mesh.
        /// </summary>
        /// <param name="shader">The shader used for drawing</param>
        /// <param name="camera">The camera used to set matrix uniforms if not <c>null</c></param>
        /// <param name="count">The number of vertices to draw</param>
        /// <param name="offset">The offset into the index buffer</param>
        public void Draw(Shader shader, Camera camera, int count, int offset = 0)
        {
            if (!shader.LinkStatusIsOk)
                return;

            shader.UseProgram();

            // Only update when the shader changes.
            if (shader.Id != previousShaderId)
            {
                ConfigureVertexAttributes(shader);
                previousShaderId = shader.Id;
            }

            // Set shader uniforms.
            SetCameraUniforms(shader, camera);
            SetMaterialUniforms(shader, material);

            // TODO: Add an option to disable this.
            GLRenderSettings.SetRenderSettings(renderSettings);

            DrawGeometry(count, offset);
        }

        private void DrawGeometry(int count, int offset)
        {
            vertexArrayObject.Bind();
            GL.DrawElements(PrimitiveType, count, DrawElementsType, offset);

            // TODO: This isn't part of the OpenGL core specification.
            // Leave this enabled for compatibility with older applications.
            vertexArrayObject.Unbind();
        }

        /// <summary>
        /// Sets the uniforms, sets render state, and draws the mesh.
        /// </summary>
        /// <param name="shader"></param>
        /// <param name="camera"></param>
        public void Draw(Shader shader, Camera camera)
        {
            Draw(shader, camera, VertexCount, 0);
        }

        /// <summary>
        /// Gets the attribute rendering information for all the members of <typeparamref name="T"/> with a <see cref="VertexAttribute"/> using reflection.
        /// When overriding this method, the order of attributes in the list should match the order of the member variables.
        /// </summary>
        /// <returns>Vertex attribute information</returns>
        public virtual List<VertexAttribute> GetVertexAttributes()
        {
            // TODO: Store this result to optimize subsequent method calls.
            var attributes = new List<VertexAttribute>();
            foreach (var member in typeof(T).GetMembers())
            {
                foreach (VertexAttribute attribute in member.GetCustomAttributes(typeof(VertexAttribute), true))
                {
                    // Break to ignore duplicate attributes.
                    attributes.Add(attribute);
                    break;
                }
            }
            return attributes;
        }

        /// <summary>
        /// Sets <c>uniform mat4 mvpMatrix</c> in the shader using <see cref="Camera.MvpMatrix"/>.
        /// Override this method to provide custom matrices. 
        /// </summary>
        /// <param name="shader">The shader used for drawing</param>
        /// <param name="camera">The camera used for drawing"/></param>
        protected virtual void SetCameraUniforms(Shader shader, Camera camera)
        {
            // Not all shaders will use camera uniforms.
            if (camera == null)
                return;

            Matrix4 matrix = camera.MvpMatrix;
            shader.SetMatrix4x4("mvpMatrix", ref matrix);
        }

        private void SetMaterialUniforms(Shader shader, GenericMaterial material)
        {
            material.SetShaderUniforms(shader);
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shader"></param>
        public void ConfigureVertexAttributes(Shader shader)
        {
            // TODO: Don't make this public.

            // The binding order here is critical.
            vertexArrayObject.Bind();

            vertexBuffer.Bind();
            vertexIndexBuffer.Bind();

            shader.EnableVertexAttributes();
            var vertexAttributes = GetVertexAttributes();
            SetVertexAttributes(shader, vertexAttributes);

            // TODO: Binding the default VAO isn't part of the core specification.
            vertexArrayObject.Unbind();

            // Unbind all the buffers.
            // This step may not be necessary.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        private void SetVertexAttributes(Shader shader, IEnumerable<VertexAttribute> attributes)
        {
            // Calculating the offset requires the list order to match the struct member order.
            int offset = 0;
            foreach (var attribute in attributes)
            {
                if (!VertexAttributeUtils.SetVertexAttribute(shader, attribute, offset, vertexSizeInBytes))
                    OnInvalidAttribSet?.Invoke(this, new AttribSetEventArgs(attribute.Name, attribute.Type, attribute.ValueCount));
                offset += attribute.SizeInBytes; 
            }
        }

        private void InitializeBufferData<K>(List<T> vertices, List<K> vertexIndices) where K : struct
        {
            vertexBuffer.SetData(vertices.ToArray(), BufferUsageHint.StaticDraw);
            vertexIndexBuffer.SetData(vertexIndices.ToArray(), BufferUsageHint.StaticDraw);
        }

        private void InitializeBufferData(List<T> vertices)
        {
            List<int> indices = IndexUtils.GenerateIndices(vertices.Count);
            InitializeBufferData(vertices, indices);
        }
    }
}
