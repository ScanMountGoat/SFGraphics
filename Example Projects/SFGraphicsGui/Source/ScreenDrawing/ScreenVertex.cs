using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGenericModel.VertexAttributes;

namespace SFGraphicsGui
{
    struct ScreenVertex
    {
        [VertexFloatAttribute("position", ValueCount.Three, VertexAttribPointerType.Float, false)]
        public Vector3 Position { get; }

        public ScreenVertex(Vector3 position)
        {
            Position = position;
        }
    }
}
