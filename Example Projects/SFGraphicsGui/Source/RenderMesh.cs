using SFGenericModel;
using System.Collections.Generic;
using OpenTK;

namespace SFGraphicsGui
{
    class RenderMesh : GenericMesh<RenderVertex>
    {
        public Vector4 BoundingSphere { get; }

        public RenderMesh(List<RenderVertex> vertices) : base(vertices, OpenTK.Graphics.OpenGL.PrimitiveType.Triangles)
        {
            var positions = new List<Vector3>(vertices.Count);
            foreach (var vertex in vertices)
                positions.Add(vertex.Position);

            BoundingSphere = SFGraphics.Utils.BoundingSphereGenerator.GenerateBoundingSphere(positions);
        }
    }
}
