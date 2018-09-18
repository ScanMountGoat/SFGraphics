using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.Utils
{
    /// <summary>
    /// Stores indexed vertex data and the primitive to be used for rendering.
    /// </summary>
    /// <typeparam name="T">The struct used for each vertex</typeparam>
    public class VertexContainer<T> where T : struct
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly List<T> vertices = new List<T>();

        /// <summary>
        /// 
        /// </summary>
        public readonly List<int> vertexIndices = new List<int>();

        /// <summary>
        /// 
        /// </summary>
        public readonly PrimitiveType primitiveType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="vertexIndices"></param>
        /// <param name="primitiveType"></param>
        public VertexContainer(List<T> vertices, List<int> vertexIndices, PrimitiveType primitiveType)
        {
            this.vertices = vertices;
            this.vertexIndices = vertexIndices;
            this.primitiveType = primitiveType;
        }
    }
}
