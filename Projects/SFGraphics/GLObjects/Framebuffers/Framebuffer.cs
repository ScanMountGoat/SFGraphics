using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.RenderBuffers;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;
using SFGraphics.GLObjects.Textures.Utils;
using System;
using System.Collections.Generic;

namespace SFGraphics.GLObjects.Framebuffers
{
    /// <summary>
    /// Encapsulates an OpenGL framebuffer, including any attached color or depth attachments.
    /// </summary>
    public sealed partial class Framebuffer : GLObject
    {
        internal override GLObjectType ObjectType { get { return GLObjectType.FramebufferObject; } }

        /// <summary>
        /// The target which <see cref="GLObject.Id"/> is bound when calling <see cref="Bind"/>.
        /// </summary>
        public FramebufferTarget Target { get; }

        /// <summary>
        /// The internal format used for all color attachments.
        /// </summary>
        public PixelInternalFormat PixelInternalFormat { get; }

        /// <summary>
        /// All attached textures, renderbuffers, etc are resized when set. The framebuffer's contents will not be preserved when resizing.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// All attached textures, renderbuffers, etc are resized when set. The framebuffer's contents will not be preserved when resizing.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Generates an incomplete framebuffer of the specified target with no attachments. 
        /// </summary>
        /// <param name="framebufferTarget">The target to which <see cref="GLObject.Id"/> is bound</param>
        public Framebuffer(FramebufferTarget framebufferTarget) : base(GL.GenFramebuffer())
        {
            Target = framebufferTarget;
        }

        /// <summary>
        /// Generates a framebuffer with a color attachment of the specified pixel format and dimensions. 
        /// A render buffer of the same dimensions as the color attachment is generated for the depth component.
        /// Binds the framebuffer.
        /// </summary>
        /// <param name="target">The target to which <see cref="GLObject.Id"/> is bound</param>
        /// <param name="width">The width of attached textures or renderbuffers</param>
        /// <param name="height">The height of attached textures or renderbuffers</param>
        /// <param name="pixelInternalFormat">The internal format for all color attachments</param>
        /// <param name="colorAttachmentsCount">The number of color attachments to create. 
        /// Ex: <c>1</c> would only create ColorAttachment0.</param>
        /// <exception cref="ArgumentOutOfRangeException">The number of color attachments is negative.</exception>
        public Framebuffer(FramebufferTarget target, int width, int height, 
            PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba, int colorAttachmentsCount = 1) 
            : this(target)
        {
            if (colorAttachmentsCount < 0)
                throw new ArgumentOutOfRangeException("Color attachment count must be non negative.");

            if (TextureFormatTools.IsCompressed(pixelInternalFormat))
                throw new ArgumentException(TextureExceptionMessages.expectedUncompressed);

            Bind();
            PixelInternalFormat = pixelInternalFormat;
            Width = width;
            Height = height;

            CreateColorAttachments(width, height, colorAttachmentsCount);

            SetUpRboDepth(width, height);
        }

        /// <summary>
        /// Gets the framebuffer status, which indicates
        /// if the framebuffer is complete and valid for rendering.
        /// </summary>
        /// <returns>The framebuffer status</returns>
        public FramebufferErrorCode GetStatus()
        {
            Bind();
            return GL.CheckFramebufferStatus(Target);
        }

        /// <summary>
        /// Attaches <paramref name="attachment"/> to <paramref name="attachmentPoint"/>.
        /// Draw and read buffers must be configured separately.
        /// </summary>
        /// <param name="attachmentPoint">The target attachment point</param>
        /// <param name="attachment">The object to attach</param>
        public void AddAttachment(FramebufferAttachment attachmentPoint, IFramebufferAttachment attachment)
        {
            attachment.Attach(attachmentPoint, this);
        }

        /// <summary>
        /// Binds the framebuffer to the target specified at creation.
        /// </summary>
        public void Bind()
        {
            GL.BindFramebuffer(Target, Id);
        }

        /// <summary>
        /// Sets which buffers or attachments receive fragment shader outputs.
        /// Binds the framebuffer.
        /// </summary>
        /// <param name="drawBuffers">The buffers used for fragment shader output</param>
        public void SetDrawBuffers(params DrawBuffersEnum[] drawBuffers)
        {
            Bind();
            GL.DrawBuffers(drawBuffers.Length, drawBuffers);
        }

        /// <summary>
        /// Sets the color buffer used for GL.ReadPixels and GL.CopyTexImage methods.
        /// Binds the framebuffer.
        /// </summary>
        /// <param name="readBufferMode">The buffer used for read operations</param>
        public void SetReadBuffer(ReadBufferMode readBufferMode)
        {
            Bind();
            GL.ReadBuffer(readBufferMode);
        }

        private Texture2D CreateColorAttachment(int width, int height)
        {
            Texture2D texture = new Texture2D()
            {
                // Don't use mipmaps for color attachments.
                MinFilter = TextureMinFilter.Nearest,
                MagFilter = TextureMagFilter.Linear
            };
            // Necessary for texture completion.
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat, width, height, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
            return texture;
        }

        private void SetUpRboDepth(int width, int height)
        {
            // Render buffer for the depth attachment, which is necessary for depth testing.
            Renderbuffer rboDepth = new Renderbuffer(width, height, RenderbufferStorage.DepthComponent);
            AddAttachment(FramebufferAttachment.DepthAttachment, rboDepth);
        }

        private List<IFramebufferAttachment> CreateColorAttachments(int width, int height, int colorAttachmentsCount)
        {
            var colorAttachments = new List<IFramebufferAttachment>();

            List<DrawBuffersEnum> attachmentEnums = new List<DrawBuffersEnum>();
            for (int i = 0; i < colorAttachmentsCount; i++)
            {
                DrawBuffersEnum attachmentPoint = DrawBuffersEnum.ColorAttachment0 + i;
                attachmentEnums.Add(attachmentPoint);

                Texture2D texture = CreateColorAttachment(width, height);
                colorAttachments.Add(texture);
                AddAttachment((FramebufferAttachment)attachmentPoint, texture);
            }

            SetDrawBuffers(attachmentEnums.ToArray());

            return colorAttachments;
        }
    }
}
