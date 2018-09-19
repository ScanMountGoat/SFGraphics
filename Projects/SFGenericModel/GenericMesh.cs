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
    public abstract class GenericMesh<T> : IndexedVertexData<T>, IDrawableMesh where T : struct
    {
        private int previousShaderId = -1;

        private VertexArrayObject vertexArrayObject = new VertexArrayObject();

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
        public delegate void InvalidAttribSetEventHandler(GenericMesh<T> sender, AttribSetEventArgs e);

        /// <summary>
        /// Occurs when specified vertex attribute information does not match the shader.
        /// </summary>
        public event InvalidAttribSetEventHandler OnInvalidAttribSet;

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// An index is generated for each vertex in <paramref name="vertices"/>.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="primitiveType"></param>
        public GenericMesh(List<T> vertices, PrimitiveType primitiveType) : base(vertices, primitiveType)
        {

        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="vertexIndices"></param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        public GenericMesh(List<T> vertices, List<int> vertexIndices, PrimitiveType primitiveType) 
            : base(vertices, vertexIndices, primitiveType)
        {

        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// </summary>
        /// <param name="vertexData">The vertex indices, data, and primitive type</param>
        public GenericMesh(VertexContainer<T> vertexData) 
            : this(vertexData.vertices, vertexData.vertexIndices, vertexData.primitiveType)
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

            GLRenderSettings.SetRenderSettings(renderSettings);

            vertexArrayObject.Bind();
            GL.DrawElements(PrimitiveType, count, DrawElementsType.UnsignedInt, offset);
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
        /// The order of vertex attributes in the list should match 
        /// the order of the fields in <typeparamref name="T"/>.
        /// </summary>
        /// <returns>Vertex attribute information</returns>
        public abstract List<VertexAttributeInfo> GetVertexAttributes();

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
            vertexArrayObject.Bind();

            VertexBuffer.Bind();
            VertexIndexBuffer.Bind();

            shader.EnableVertexAttributes();
            List<VertexAttributeInfo> vertexAttributes = GetVertexAttributes();
            SetVertexAttributes(shader, vertexAttributes, VertexSizeInBytes);

            // Unbind all the buffers.
            // This step may not be necessary.
            vertexArrayObject.Unbind();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        private void SetVertexAttributes(Shader shader, List<VertexAttributeInfo> attributes, int strideInBytes)
        {
            // Setting vertex attributes is handled automatically. 
            int offset = 0;
            foreach (VertexAttributeInfo attribute in attributes)
            {
                if(!VertexAttributeUtils.SetVertexAttribute(shader, attribute, offset, strideInBytes))
                    OnInvalidAttribSet?.Invoke(this, new AttribSetEventArgs(attribute.name, attribute.type, attribute.valueCount));
                offset += attribute.sizeInBytes;
            }
        }
    }
}
