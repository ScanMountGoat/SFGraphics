using OpenTK;
using SFGenericModel.VertexAttributes;

namespace SFGraphicsGui
{
    struct ObjVertex
    {
        [VertexFloat("position", ValueCount.Three, OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float, false, AttributeUsage.Position, true, true)]
        public Vector3 Position { get; }

        [VertexFloat("normal", ValueCount.Three, OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float, false, AttributeUsage.Normal, true, true)]
        public Vector3 Normal { get; }

        [VertexFloat("texcoord0", ValueCount.Two, OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float, false, AttributeUsage.TexCoord0, false, false)]
        public Vector2 TexCoord0 { get; }

        public ObjVertex(Vector3 position, Vector3 normal, Vector2 texCoord0)
        {
            Position = position;
            Normal = normal;
            TexCoord0 = texCoord0;
        }
    }
}
