using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGenericModel;
using System.Collections.Generic;

namespace SFGraphicsGui
{
    class ScreenTriangle : GenericMesh<Vector3>
    {
        // A triangle that extends past the screen.
        private static List<Vector3> screenTrianglePositions = new List<Vector3>()
        {
            new Vector3(-1f, -1f, 0.0f),
            new Vector3( 3f, -1f, 0.0f),
            new Vector3(-1f,  3f, 0.0f)
        };

        public ScreenTriangle() : base(screenTrianglePositions, PrimitiveType.Triangles)
        {

        }

        protected override List<VertexAttributeInfo> GetVertexAttributes()
        {
            return new List<VertexAttributeInfo>()
            {
                new VertexAttributeInfo("position", ValueCount.Three, VertexAttribPointerType.Float)
            };
        }
    }
}
