using System;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Framebuffers;

namespace SFGraphics.GLObjects.RenderBuffers
{
    /// <summary>
    /// Encapsulates a renderbuffer object, 
    /// a framebuffer attachment that supports multisampling.
    /// </summary>
    public class Renderbuffer : GLObject, IFramebufferAttachment
    { 
        internal override GLObjectType ObjectType => GLObjectType.RenderbufferObject;

        /// <summary>
        /// The width of the renderbuffer in pixels.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// The height of the renderbuffer in pixels.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Creates and allocates storage for an empty renderbuffer. 
        /// </summary>
        /// <param name="width">The width of the renderbuffer in pixels</param>
        /// <param name="height">The height of the renderbuffer in pixels</param>
        /// <param name="internalFormat">The format for storing the image data</param>
        public Renderbuffer(int width, int height, RenderbufferStorage internalFormat)
            : base(GL.GenRenderbuffer())
        {
            Width = width;
            Height = height;

            if (Width < 0)
                throw new ArgumentOutOfRangeException("width", "Dimensions must be non negative.");

            if (Height < 0)
                throw new ArgumentOutOfRangeException("height", "Dimensions must be non negative.");

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
        /// <param name="internalFormat">The format for storing the image data</param>
        public Renderbuffer(int width, int height, int samples, RenderbufferStorage internalFormat)
            : base(GL.GenRenderbuffer())
        {
            Width = width;
            Height = height;

            if (Width < 0)
                throw new ArgumentOutOfRangeException("width", "Dimensions must be non negative.");

            if (Height < 0)
                throw new ArgumentOutOfRangeException("height", "Dimensions must be non negative.");

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

        /// <summary>
        /// Binds the renderbuffer to <paramref name="attachment"/> for
        /// <paramref name="target"/>.
        /// </summary>
        /// <param name="attachment">The attachment point</param>
        /// <param name="target">The target framebuffer for attachment</param>
        public void Attach(FramebufferAttachment attachment, Framebuffer target)
        {
            target.Bind();
            GL.FramebufferRenderbuffer(target.Target, attachment, RenderbufferTarget.Renderbuffer, Id);
        }
    }
}
