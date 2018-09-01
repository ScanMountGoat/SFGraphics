using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Shaders
{
    internal struct ActiveUniformInfo
    {
        public readonly int location;

        public readonly ActiveUniformType type;

        public readonly int size;

        public ActiveUniformInfo(int location, ActiveUniformType type, int size = 1)
        {
            this.location = location;
            this.type = type;
            this.size = size;
        }
    }
}
