using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGenericModel.VertexAttributes;

namespace SFGraphicsGui
{
    struct ScreenTriangleVertex
    {
        [VertexFloat("position", ValueCount.Three, VertexAttribPointerType.Float, false)]
        public readonly Vector3 position;

        [VertexFloat("normal", ValueCount.Three, VertexAttribPointerType.Float, false)]
        public readonly Vector3 normal;

        [VertexFloat("color", ValueCount.Four, VertexAttribPointerType.Float, false)]
        public readonly Vector4 color;

        public ScreenTriangleVertex(Vector3 position, Vector3 normal, Vector4 color)
        {
            this.position = position;
            this.normal = normal;
            this.color = color;
        }
    }
}
