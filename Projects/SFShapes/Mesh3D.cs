using OpenTK.Graphics.OpenGL;
using SFGenericModel;
using SFGenericModel.VertexAttributes;
using System;
using System.Collections.Generic;

namespace SFShapes
{
    /// <summary>
    /// Draws simple geometry given a collection of vertex positions.
    /// </summary>
    public class Mesh3D : GenericMesh<Vertex3d>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertices">The points for the shape</param>
        /// <param name="primitiveType">Determines how the shape should be drawn</param>
        public Mesh3D(Vertex3d[] vertices, PrimitiveType primitiveType) : base(vertices, primitiveType)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertices">The points of the shape</param>
        public Mesh3D(Tuple<Vertex3d[], PrimitiveType> vertices) : base(vertices.Item1, vertices.Item2)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<VertexAttribute> GetRenderAttributes()
        {
            return new List<VertexAttribute>()
            {
                new VertexFloatAttribute("position", ValueCount.Three, VertexAttribPointerType.Float, false, AttributeUsage.Position, true, true)
            };
        }
    }
}
