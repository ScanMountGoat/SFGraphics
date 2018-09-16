using OpenTK;

namespace SFGraphicsGui
{
    struct ScreenTriangleVertex
    {
        public readonly Vector3 position;

        public readonly Vector3 normal;

        public readonly Vector4 color;

        public ScreenTriangleVertex(Vector3 position, Vector3 normal, Vector4 color)
        {
            this.position = position;
            this.normal = normal;
            this.color = color;
        }
    }
}
