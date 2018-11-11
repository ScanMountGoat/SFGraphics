using OpenTK.Graphics;

namespace GLViewport
{
    /// <summary>
    /// Provides an easier to use interface for <see cref="OpenTK.GLControl"/>.
    /// </summary>
    public class GLViewport : OpenTK.GLControl
    {
        /// <summary>
        /// The default graphics mode for rendering. Enables depth/stencil buffers and antialiasing. 
        /// </summary>
        public static readonly GraphicsMode defaultGraphicsMode = new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 8, 16);

        /// <summary>
        /// Creates a new viewport with <see cref="defaultGraphicsMode"/>.
        /// </summary>
        public GLViewport() : base(defaultGraphicsMode)
        {

        }
    }
}
