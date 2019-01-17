using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGenericModel;
using System.Collections.Generic;

namespace SFGraphicsGui
{
    class ScreenTriangle : GenericMesh<ScreenTriangleVertex>
    {
        // A triangle that extends past the screen.
        private static List<ScreenTriangleVertex> screenTrianglePositions = new List<ScreenTriangleVertex>()
        {
            new ScreenTriangleVertex(new Vector3(-1f, -1f, 0.0f), new Vector3(-1,0,0), new Vector4(1)),
            new ScreenTriangleVertex(new Vector3( 3f, -1f, 0.0f), new Vector3(0,-25,25), new Vector4(1)),
            new ScreenTriangleVertex(new Vector3(-1f,  3f, 0.0f), new Vector3(0,1,-1), new Vector4(1))
        };

        public ScreenTriangle() : base(screenTrianglePositions, PrimitiveType.Triangles)
        {

        }
    }
}
