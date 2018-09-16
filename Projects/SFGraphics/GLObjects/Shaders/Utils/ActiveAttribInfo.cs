using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Shaders.Utils
{
    internal struct ActiveAttribInfo
    {
        public readonly int location;

        public readonly ActiveAttribType type;

        public readonly int size;

        public ActiveAttribInfo(int location, ActiveAttribType type, int size)
        {
            this.location = location;
            this.type = type;
            this.size = size;
        }
    }
}
