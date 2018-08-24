using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects;
using SFGraphics.Cameras;
using SFGenericModel.Materials;
using SFGenericModel.RenderState;

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
    public abstract class GenericMesh<T> where T : struct
    {
        private readonly int vertexSizeInBytes;
        private readonly int vertexCount;

        // The vertex data is immutable, so buffers only need to be initialized once.
        private BufferObject vertexBuffer = new BufferObject(BufferTarget.ArrayBuffer);
        private BufferObject vertexIndexBuffer = new BufferObject(BufferTarget.ElementArrayBuffer);
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
        /// The type of primitive used for drawing.
        /// </summary>
        public PrimitiveType PrimitiveType { get; set; } = PrimitiveType.Triangles;

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// An index is generated for each vertex in <paramref name="vertices"/>.
        /// Vertex data is initialized only once.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="vertexSizeInBytes"></param>
        public GenericMesh(List<T> vertices, int vertexSizeInBytes)
        {
            this.vertexSizeInBytes = vertexSizeInBytes;
            vertexCount = vertices.Count;

            // Generate a unique index for each vertex.
            List<int> vertexIndices = GenerateIndices(vertices);

            // Generate basic indices.
            InitializeBufferData(vertices, vertexIndices);
        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// Vertex data is initialized only once.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="vertexIndices"></param>
        /// <param name="vertexSizeInBytes">The total size in bytes of <typeparamref name="T"/></param>
        public GenericMesh(List<T> vertices, List<int> vertexIndices, int vertexSizeInBytes)
        {
            this.vertexSizeInBytes = vertexSizeInBytes;
            vertexCount = vertexIndices.Count;

            InitializeBufferData(vertices, vertexIndices);
        }

        private static List<int> GenerateIndices(List<T> vertices)
        {
            List<int> vertexIndices = new List<int>();
            for (int i = 0; i < vertices.Count; i++)
            {
                vertexIndices.Add(i);
            }

            return vertexIndices;
        }

        /// <summary>
        /// Sets the uniforms, sets render state, and draws the mesh.
        /// </summary>
        /// <param name="shader"></param>
        /// <param name="camera">The camera used to set matrix uniforms if not <c>null</c></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        public void Draw(Shader shader, Camera camera, int count, int offset = 0)
        {
            if (!shader.ProgramCreatedSuccessfully)
                return;

            shader.UseProgram();

            // Set shader uniforms.
            SetCameraUniforms(shader, camera);
            SetMaterialUniforms(shader, material);

            ConfigureVertexAttributes(shader);

            SetRenderSettings();

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
            Draw(shader, camera, vertexCount, 0);
        }

        /// <summary>
        /// The order of vertex attributes in the list should match 
        /// the order of the fields in <typeparamref name="T"/>.
        /// </summary>
        /// <returns>Vertex attribute information</returns>
        protected abstract List<VertexAttributeInfo> GetVertexAttributes();

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

        private void SetRenderSettings()
        {
            SetFaceCulling(renderSettings.faceCullingSettings);
            SetAlphaBlending(renderSettings.alphaBlendSettings);
            SetAlphaTesting(renderSettings.alphaTestSettings);
        }

        private void SetMaterialUniforms(Shader shader, GenericMaterial material)
        {
            material.SetShaderUniforms(shader);
        }

        private static void SetGLEnableCap(EnableCap enableCap, bool enabled)
        {
            if (enabled)
                GL.Enable(enableCap);
            else
                GL.Disable(enableCap);
        }

        private void SetFaceCulling(RenderSettings.FaceCullingSettings settings)
        {
            SetGLEnableCap(EnableCap.CullFace, settings.enableFaceCulling);

            GL.CullFace(settings.cullFaceMode);
        }

        private void SetDepthTesting(RenderSettings.DepthTestSettings settings)
        {
            SetGLEnableCap(EnableCap.DepthTest, settings.enableDepthTest);

            GL.DepthFunc(settings.depthFunction);
            GL.DepthMask(settings.depthMask);
        }

        private void SetAlphaBlending(RenderSettings.AlphaBlendSettings settings)
        {
            SetGLEnableCap(EnableCap.Blend, settings.enableAlphaBlending);

            GL.BlendFunc(settings.sourceFactor, settings.destinationFactor);
            GL.BlendEquationSeparate(settings.blendingEquationRgb, settings.blendingEquationAlpha);
        }

        private void SetAlphaTesting(RenderSettings.AlphaTestSettings settings)
        {
            SetGLEnableCap(EnableCap.AlphaTest, settings.enableAlphaTesting);

            // TODO: Should this be a float or an int?
            GL.AlphaFunc(settings.alphaFunction, settings.referenceAlpha / 255.0f);
        }

        private void InitializeBufferData(List<T> vertices, List<int> vertexIndices)
        {
            vertexBuffer.SetData(vertices.ToArray(), vertexSizeInBytes, BufferUsageHint.StaticDraw);
            vertexIndexBuffer.SetData(vertexIndices.ToArray(), sizeof(int), BufferUsageHint.StaticDraw);
        }

        private void ConfigureVertexAttributes(Shader shader)
        {
            vertexArrayObject.Bind();

            vertexBuffer.Bind();
            vertexIndexBuffer.Bind();

            shader.EnableVertexAttributes();
            SetVertexAttributes(shader);

            // Unbind all the buffers.
            // This step may not be necessary.
            vertexArrayObject.Unbind();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        private void SetVertexAttributes(Shader shader)
        {
            // Setting vertex attributes is handled automatically. 
            List<VertexAttributeInfo> vertexAttributes = GetVertexAttributes();
            int offset = 0;
            foreach (VertexAttributeInfo attribute in vertexAttributes)
            {
                // -1 means not found, which is usually a result of the attribute being unused.
                int index = shader.GetVertexAttributeUniformLocation(attribute.name);
                if (index != -1)
                    GL.VertexAttribPointer(index, attribute.valueCount, attribute.vertexAttribPointerType, false, vertexSizeInBytes, offset);
                // Move offset to next attribute.
                offset += attribute.sizeInBytes;
            }
        }
    }
}
