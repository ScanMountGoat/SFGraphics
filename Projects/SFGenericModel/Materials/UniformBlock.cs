using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.BufferObjects;
using SFGraphics.GLObjects.Shaders;

namespace SFGenericModel.Materials
{
    /// <summary>
    /// Stores a collection of uniforms in a buffer to improve performance and allow sharing uniforms
    /// between shader programs.
    /// </summary>
    public class UniformBlock
    {
        private readonly BufferObject uniformBuffer = new BufferObject(BufferTarget.UniformBuffer);

        /// <summary>
        /// The binding point index for the uniform block.
        /// </summary>
        public int BlockBinding { get; set; } = 0;

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
        /// Sets the associated buffer data for <paramref name="name"/> to <paramref name="values"/>.
        /// </summary>
        /// <param name="name">The name of the uniform inside the uniform block</param>
        /// <param name="values">The new value for the uniform</param>
        public void SetMatrix4(string name, Matrix4[] values)
        {
            // TODO: Calculate buffer size and find the proper offsets for SetSubData.
            uniformBuffer.SetData(values, BufferUsageHint.DynamicDraw);
        }
    }
}
