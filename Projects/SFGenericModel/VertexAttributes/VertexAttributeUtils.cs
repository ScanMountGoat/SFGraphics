using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace SFGenericModel.VertexAttributes
{
    /// <summary>
    /// Methods for configuring vertex attributes for a <see cref="Shader"/>.
    /// </summary>
    public static class VertexAttributeUtils
    {
        /// <summary>
        /// Configures a vertex attribute for the currently bound element array buffer.
        /// Returns false on error.
        /// </summary>
        /// <param name="shader">The current shader used for rendering</param>
        /// <param name="attribute">The vertex attribute information</param>
        /// <param name="offsetInBytes">The offset into the vertex data</param>
        /// <param name="strideInBytes">The size in bytes of each vertex</param>
        /// <returns><c>true</c> if the set was successful</returns>
        public static bool SetVertexAttribute(Shader shader, VertexAttributeInfo attribute, int offsetInBytes, int strideInBytes)
        {
            // Ignore invalid attributes to prevent OpenGL from generating errors.
            int index = shader.GetAttribLocation(attribute.name);
            if (index == -1)
                return false;

            GL.VertexAttribPointer(index, (int)attribute.valueCount, attribute.type, false, strideInBytes, offsetInBytes);
            return true;
        }
    }
}
