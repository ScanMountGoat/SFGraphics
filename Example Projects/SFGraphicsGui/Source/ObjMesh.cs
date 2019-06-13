using SFGenericModel;
using System.Collections.Generic;
using OpenTK;

namespace SFGraphicsGui
{
    class ObjMesh : GenericMesh<ObjVertex>
    {
        public Vector4 BoundingSphere { get; }

        public ObjMesh(List<ObjVertex> vertices) : base(vertices, OpenTK.Graphics.OpenGL.PrimitiveType.Triangles)
        {
            var positions = new List<Vector3>(vertices.Count);
            foreach (var vertex in vertices)
                positions.Add(vertex.Position);

            BoundingSphere = SFGraphics.Utils.BoundingSphereGenerator.GenerateBoundingSphere(positions);
        }
    }
}
