using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Framebuffers
{
    /// <summary>
    /// Contains methods for attaching to a <see cref="Framebuffer"/>
    /// </summary>
    public interface IFramebufferAttachment
    {
        /// <summary>
        /// Binds the object to <paramref name="attachment"/> for
        /// <paramref name="target"/>.
        /// </summary>
        /// <param name="attachment">The attachment point</param>
        /// <param name="target">The target framebuffer for attachment</param>
        void Attach(FramebufferAttachment attachment, Framebuffer target);

        /// <summary>
        /// The width of the attachment in pixels.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// The height of the attachment in pixels.
        /// </summary>
        int Height { get; }
    }
}