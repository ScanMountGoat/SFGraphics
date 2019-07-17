using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.BufferObjects;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;

namespace SFGenericModel.Materials
{
    /// <summary>
    /// Stores a collection of uniforms in a buffer to improve performance and allow sharing uniforms
    /// between shader programs.
    /// </summary>
    public class UniformBlock
    {
        private readonly BufferObject uniformBuffer = new BufferObject(BufferTarget.UniformBuffer);

        private readonly Dictionary<string, int> offsetByUniformName = new Dictionary<string, int>();

        /// <summary>
        /// The binding point index for the uniform block.
        /// </summary>
        public int BlockBinding { get; set; } = 0;

        /// <summary>
        /// Initializes the uniform buffer based on the layout of <paramref name="uniformBlockName"/> in <paramref name="shader"/>.
        /// </summary>
        /// <param name="shader">The shader containing the uniform block</param>
        /// <param name="uniformBlockName">The name of the uniform block</param>
        public UniformBlock(Shader shader, string uniformBlockName)
        {
            Initialize(shader, uniformBlockName);
        }

        /// <summary>
        /// Binds the uniform buffer to <see cref="BlockBinding"/> for the appropriate index for <paramref name="shader"/>.
        /// </summary>
        /// <param name="shader">The shader containing the uniform block</param>
        /// <param name="uniformBlockName">The name of the uniform block in <paramref name="shader"/></param>
        public void BindBlock(Shader shader, string uniformBlockName)
        {
            uniformBuffer.BindBase(BufferRangeTarget.UniformBuffer, BlockBinding);

            int blockIndex = shader.GetUniformBlockIndex(uniformBlockName);
            if (blockIndex != -1)
                GL.UniformBlockBinding(shader.Id, blockIndex, BlockBinding);
        }

        /// <summary>
        /// Sets the associated buffer data for <paramref name="uniformName"/> to <paramref name="value"/>.
        /// </summary>
        /// <param name="uniformName">The name of the uniform inside the uniform block</param>
        /// <param name="value">The new value for the uniform</param>
        public void SetValue<T>(string uniformName, T value) where T : struct
        {
            if (offsetByUniformName.ContainsKey(uniformName))
                uniformBuffer.SetSubData(new [] { value }, offsetByUniformName[uniformName]);
        }

        /// <summary>
        /// Sets the associated buffer data for <paramref name="uniformName"/> to <paramref name="values"/>.
        /// </summary>
        /// <param name="uniformName">The name of the uniform inside the uniform block</param>
        /// <param name="values">The new value for the uniform</param>
        public void SetValues<T>(string uniformName, T[] values) where T : struct
        {
            if (offsetByUniformName.ContainsKey(uniformName))
                uniformBuffer.SetSubData(values, offsetByUniformName[uniformName]);
        }

        private static int[] GetUniformOffsets(Shader shader, int uniformCount, int[] uniformIndices)
        {
            var offsets = new int[uniformCount];
            GL.GetActiveUniforms(shader.Id, uniformCount, uniformIndices, ActiveUniformParameter.UniformOffset, offsets);
            return offsets;
        }

        private static int[] GetUniformIndices(Shader shader, string uniformBlockName, int uniformCount)
        {
            var uniformIndices = new int[uniformCount];
            GL.GetActiveUniformBlock(shader.Id, shader.GetUniformBlockIndex(uniformBlockName), ActiveUniformBlockParameter.UniformBlockActiveUniformIndices, uniformIndices);
            return uniformIndices;
        }

        private void Initialize(Shader shader, string uniformBlockName)
        {
            InitializeUniformOffsets(shader, uniformBlockName);

            // Set buffer capacity, so uniforms can be initialized later.
            GL.GetActiveUniformBlock(shader.Id, shader.GetUniformBlockIndex(uniformBlockName), ActiveUniformBlockParameter.UniformBlockDataSize, out int dataSize);
            uniformBuffer.SetCapacity(dataSize, BufferUsageHint.DynamicDraw);
        }

        private void InitializeUniformOffsets(Shader shader, string uniformBlockName)
        {
            // TODO: This method is pretty slow.
            // There may be a better way to associate uniform names with buffer offsets.
            GL.GetActiveUniformBlock(shader.Id, shader.GetUniformBlockIndex(uniformBlockName), ActiveUniformBlockParameter.UniformBlockActiveUniforms, out int uniformCount);

            // TODO: Move these methods to the shader class.
            int[] uniformIndices = GetUniformIndices(shader, uniformBlockName, uniformCount);
            int[] offsets = GetUniformOffsets(shader, uniformCount, uniformIndices);

            offsetByUniformName.Clear();
            for (int i = 0; i < uniformCount; i++)
            {
                // Remove array brackets.
                string name = GL.GetActiveUniformName(shader.Id, uniformIndices[i]);
                if (name.Contains("["))
                    name = name.Substring(0, name.IndexOf('['));
                offsetByUniformName.Add(name, offsets[i]);
            }
        }
    }
}
