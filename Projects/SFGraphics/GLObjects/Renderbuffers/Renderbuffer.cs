using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Framebuffers;

namespace SFGraphics.GLObjects.RenderBuffers
{
    /// <summary>
    /// Encapsulates a renderbuffer object, which are used exclusively as attachments 
    /// for <see cref="Framebuffer"/> objects. Renderbuffers support multisampling.
    /// </summary>
    public class Renderbuffer : GLObject
    { 
        internal override GLObjectType ObjectType { get { return GLObjectType.RenderbufferObject; } }

        /// <summary>
        /// The width of the renderbuffer in pixels.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// The height of the renderbuffer in pixels.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Creates an allocates storage for an empty renderbuffer. 
        /// </summary>
        /// <param name="width">The width of the renderbuffer in pixels</param>
        /// <param name="height">The height of the renderbuffer in pixels</param>
        /// <param name="internalFormat">The format of the image data</param>
        public Renderbuffer(int width, int height, RenderbufferStorage internalFormat)
            : base(GL.GenRenderbuffer())
        {
            Width = width;
            Height = height;

            // Allocate storage for the renderbuffer.
            Bind();
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, internalFormat, width, height);
        }

        /// <summary>
        /// Creates and allocates storage for an empty multisampled renderbuffer.
        /// </summary>
        /// <param name="width">The width of the renderbuffer in pixels</param>
        /// <param name="height">The height of the renderbuffer in pixels</param>
        /// <param name="samples">The number of samples to use for multisampling</param>
        /// <param name="internalFormat">The format of the image data</param>
        public Renderbuffer(int width, int height, int samples, RenderbufferStorage internalFormat)
            : base(GL.GenRenderbuffer())
        {
            Width = width;
            Height = height;

            // Allocate storage for the renderbuffer.
            Bind();
            GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, samples, 
                internalFormat, width, height);
        }

        /// <summary>
        /// Binds <see cref="GLObject.Id"/> to the RenderbufferTarget.Renderbuffer target.
        /// </summary>
        public void Bind()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Id);
        }
    }
}
