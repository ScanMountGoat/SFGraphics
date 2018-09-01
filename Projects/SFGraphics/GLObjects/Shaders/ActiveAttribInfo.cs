using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Shaders
{
    internal struct ActiveAttribInfo
    {
        public readonly int location;

        public readonly ActiveAttribType type;

        public ActiveAttribInfo(int location, ActiveAttribType type)
        {
            this.location = location;
            this.type = type;
        }
    }
}
