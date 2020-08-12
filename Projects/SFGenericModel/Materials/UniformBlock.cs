using System;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.BufferObjects;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SFGenericModel.Materials
{
    /// <summary>
    /// Stores a collection of uniforms in a <see cref="BufferObject"/>. 
    /// Uniforms are set all at once using <see cref="BindBlock(Shader)"/>, which has better performance than setting uniforms individually.
    /// </summary>
    public class UniformBlock
    {
        private readonly BufferObject uniformBuffer = new BufferObject(BufferTarget.UniformBuffer);

        private readonly Dictionary<string, int> offsetByUniformName = new Dictionary<string, int>();

        /// <summary>
        /// The name of the uniform block in the shader.
        /// </summary>
        public string UniformBlockName { get; }

        /// <summary>
        /// The binding point index for the uniform block.
        /// This should not be shared with other <see cref="UniformBlock"/> instances.
        /// </summary>
        public int BlockBinding { get; set; } = 0;

        /// <summary>
        /// Initializes the buffer's storage capacity based on the declaration of <paramref name="uniformBlockName"/> in <paramref name="shader"/>.
        /// </summary>
        /// <param name="shader">The shader containing the uniform block</param>
        /// <param name="uniformBlockName">The name of the uniform block in <paramref name="shader"/></param>
        /// <exception cref="ArgumentException"><paramref name="uniformBlockName"/> is not an active uniform block in <paramref name="shader"/></exception>
        public UniformBlock(Shader shader, string uniformBlockName)
        {
            UniformBlockName = uniformBlockName;
            Initialize(shader, UniformBlockName);
        }

        /// <summary>
        /// Binds the uniform buffer to <see cref="BlockBinding"/> for the appropriate index for <paramref name="shader"/>.
        /// </summary>
        /// <param name="shader">The shader containing the uniform block</param>
        public void BindBlock(Shader shader)
        {
            uniformBuffer.BindBase(BufferRangeTarget.UniformBuffer, BlockBinding);

            int blockIndex = shader.GetUniformBlockIndex(UniformBlockName);
            if (blockIndex != -1)
                GL.UniformBlockBinding(shader.Id, blockIndex, BlockBinding);
        }

        /// <summary>
        /// Sets the associated buffer data for <paramref name="uniformName"/> to <paramref name="value"/>.
        /// </summary>
        /// <param name="uniformName">The name of the uniform inside the uniform block</param>
        /// <param name="value">The new value for the uniform</param>
        /// <returns><c>true</c> if <paramref name="uniformName"/> was set successfully</returns>
        /// <exception cref="ArgumentOutOfRangeException">Setting <paramref name="value"/> would result in an invalid buffer access</exception>
        public bool SetValue<T>(string uniformName, T value) where T : struct
        {
            if (!offsetByUniformName.ContainsKey(uniformName))
                return false;

            uniformBuffer.SetSubData(value, offsetByUniformName[uniformName]);
            return true;
        }

        /// <summary>
        /// Sets the associated buffer data for <paramref name="uniformName"/> to <paramref name="values"/>.
        /// </summary>
        /// <param name="uniformName">The name of the uniform inside the uniform block</param>
        /// <param name="values">The new value for the uniform</param>
        /// <returns><c>true</c> if <paramref name="uniformName"/> was set successfully</returns>
        /// <exception cref="ArgumentOutOfRangeException">Setting <paramref name="values"/> would result in an invalid buffer access</exception>
        public bool SetValues<T>(string uniformName, T[] values) where T : struct
        {
            if (!offsetByUniformName.ContainsKey(uniformName))
                return false;

            uniformBuffer.SetSubData(values, offsetByUniformName[uniformName]);
            return true;
        }

        private static int[] GetUniformOffsets(Shader shader, int uniformCount, int[] uniformIndices)
        {
            var offsets = new int[uniformCount];
            GL.GetActiveUniforms(shader.Id, uniformCount, uniformIndices, ActiveUniformParameter.UniformOffset, offsets);
            return offsets;
        }

        private static int[] GetUniformIndices(Shader shader, int uniformBlockIndex, int uniformCount)
        {
            var uniformIndices = new int[uniformCount];
            GL.GetActiveUniformBlock(shader.Id, uniformBlockIndex, ActiveUniformBlockParameter.UniformBlockActiveUniformIndices, uniformIndices);
            return uniformIndices;
        }

        private void Initialize(Shader shader, string uniformBlockName)
        {
            // All subsequent operations will fail, so just throw an exception.
            int index = shader.GetUniformBlockIndex(uniformBlockName);
            if (index == -1)
                throw new ArgumentException($"{uniformBlockName} is not an active uniform block in the specified shader.", nameof(uniformBlockName));

            InitializeUniformOffsets(shader, index);

            // Set buffer capacity, so uniforms can be initialized later.
            GL.GetActiveUniformBlock(shader.Id, shader.GetUniformBlockIndex(uniformBlockName), ActiveUniformBlockParameter.UniformBlockDataSize, out int dataSize);
            uniformBuffer.SetCapacity(dataSize, BufferUsageHint.DynamicDraw);
        }

        private void InitializeUniformOffsets(Shader shader, int uniformBlockIndex)
        {
            // TODO: This method is pretty slow.
            // There may be a better way to associate uniform names with buffer offsets.
            GL.GetActiveUniformBlock(shader.Id, uniformBlockIndex, ActiveUniformBlockParameter.UniformBlockActiveUniforms, out int uniformCount);

            // TODO: Move these methods to the shader class.
            int[] uniformIndices = GetUniformIndices(shader, uniformBlockIndex, uniformCount);
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
