using OpenTK;
using SFGenericModel.VertexAttributes;

namespace SFGraphicsGui
{
    struct RenderVertex
    {
        [VertexFloat("position", ValueCount.Three, OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float, false)]
        public Vector3 Position { get; }

        [VertexFloat("normal", ValueCount.Three, OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float, false)]
        public Vector3 Normal { get; }

        [VertexFloat("texcoord0", ValueCount.Two, OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float, false)]
        public Vector2 TexCoord0 { get; }

        public RenderVertex(Vector3 position, Vector3 normal, Vector2 texCoord0)
        {
            Position = position;
            Normal = normal;
            TexCoord0 = texCoord0;
        }
    }
}
