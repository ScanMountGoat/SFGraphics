using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFGraphics.GLObjects.BufferObjects;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace SFGenericModel.Materials
{
    /// <summary>
    /// 
    /// </summary>
    public class UniformBlock
    {
        private readonly BufferObject uniformBuffer = new BufferObject(BufferTarget.UniformBuffer);

        /// <summary>
        /// The binding point index for the uniform block.
        /// </summary>
        public int BindingPoint { get; set; } = 0;

        /// <summary>
        /// Binds the uniform buffer to <see cref="BindingPoint"/> for the appropriate index for <paramref name="shader"/>.
        /// </summary>
        /// <param name="shader">The shader containing the uniform block</param>
        /// <param name="uniformBlockName">The name of the uniform block in <paramref name="shader"/></param>
        public void BindBlock(Shader shader, string uniformBlockName)
        {
            int blockIndex = shader.GetUniformBlockIndex(uniformBlockName);
            if (blockIndex != -1)
                GL.UniformBlockBinding(shader.Id, blockIndex, BindingPoint);
        }

        /// <summary>
        /// Sets the associated buffer data for <paramref name="name"/> to <paramref name="value"/>.
        /// </summary>
        /// <param name="name">The name of the uniform inside the uniform block</param>
        /// <param name="value">The new value for the uniform</param>
        public void SetFloat(string name, float value)
        {
            // TODO: Calculate buffer size and find the proper offsets for SetSubData.
            uniformBuffer.SetData(new float[] { value }, BufferUsageHint.StaticDraw);
        }
    }
}
