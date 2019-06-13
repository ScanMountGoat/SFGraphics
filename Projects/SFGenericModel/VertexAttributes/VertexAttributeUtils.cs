using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;

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
        public static bool SetVertexAttribute(Shader shader, VertexAttribute attribute, int offsetInBytes, int strideInBytes)
        {
            // Ignore invalid attributes to prevent OpenGL from generating errors.
            int index = shader.GetAttribLocation(attribute.Name);
            if (index == -1)
                return false;

            attribute.SetVertexAttribute(index, strideInBytes, offsetInBytes);
            return true;
        }

        /// <summary>
        /// Gets the vertex attributes for the members of <typeparamref name="T"/>
        /// with the appropriate attribute.
        /// </summary>
        /// <typeparam name="T">The vertex struct type</typeparam>
        /// <returns>The vertex attributes for <typeparamref name="T"/></returns>
        public static List<VertexAttribute> GetAttributesFromType<T>() where T : struct
        {
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
    }
}
