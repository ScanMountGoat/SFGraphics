using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Shaders
{
    internal struct ActiveUniformInfo
    {
        public readonly int location;

        public readonly ActiveUniformType type;

        public ActiveUniformInfo(int location, ActiveUniformType type)
        {
            this.location = location;
            this.type = type;
        }
    }
}
