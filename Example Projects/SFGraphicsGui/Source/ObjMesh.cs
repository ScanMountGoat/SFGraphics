using SFGenericModel;
using System.Collections.Generic;

namespace SFGraphicsGui
{
    class ObjMesh : GenericMesh<ObjVertex>
    {
        public ObjMesh(List<ObjVertex> vertices) : base(vertices, OpenTK.Graphics.OpenGL.PrimitiveType.Triangles)
        {

        }
    }
}
