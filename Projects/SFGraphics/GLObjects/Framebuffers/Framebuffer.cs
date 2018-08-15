using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;

namespace SFGraphics.GLObjects
{
    /// <summary>
    /// Encapsulates an OpenGL framebuffer, including any attached color or depth attachments.
    /// </summary>
    public sealed partial class Framebuffer : GLObject
    {
        /// <summary>
        /// Returns the type of OpenGL object. Used for memory management.
        /// </summary>
        public override GLObjectType ObjectType { get { return GLObjectType.FramebufferObject; } }

        /// <summary>
        /// The target which <see cref="GLObject.Id"/> is bound when calling <see cref="Bind"/>.
        /// </summary>
        public FramebufferTarget FramebufferTarget { get; }

        /// <summary>
        /// The internal format used for all color attachments.
        /// </summary>
        public PixelInternalFormat PixelInternalFormat { get; }

        /// <summary>
        /// All attached textures, renderbuffers, etc are resized when set. The framebuffer's contents will not be preserved when resizing.
        /// </summary>
        public int Width
        {
            get { return width; }
            set
            {
                width = value;
                Resize();
            }
        }
        private int width = 1;

        /// <summary>
        /// All attached textures, renderbuffers, etc are resized when set. The framebuffer's contents will not be preserved when resizing.
        /// </summary>
        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                Resize();
            }
        }
        private int height = 1;

        /// <summary>
        /// All color attachment textures. 
        /// </summary>
        public ReadOnlyCollection<Texture2D> ColorAttachments { get { return colorAttachments.AsReadOnly(); } }

        private List<Texture2D> colorAttachments = new List<Texture2D>();

        private Renderbuffer rboDepth;

        /// <summary>
        /// Generates an empty framebuffer with no attachments bound to the specified target. 
        /// Binds the framebuffer.
        /// </summary>
        /// <param name="framebufferTarget">The target to which <see cref="GLObject.Id"/> is bound</param>
        public Framebuffer(FramebufferTarget framebufferTarget) : base(GL.GenFramebuffer())
        {
            FramebufferTarget = framebufferTarget;
            Bind();
        }

        /// <summary>
        /// Generates a framebuffer with a color attachment of the specified pixel format and dimensions. 
        /// A render buffer of the same dimensions as the color attachment is generated for the depth component.
        /// Binds the framebuffer.
        /// </summary>
        /// <param name="framebufferTarget">The target to which <see cref="GLObject.Id"/> is bound</param>
        /// <param name="width">The width of attached textures or renderbuffers</param>
        /// <param name="height">The height of attached textures or renderbuffers</param>
        /// <param name="pixelInternalFormat">The internal format for all color attachments</param>
        /// <param name="colorAttachmentsCount">The number of color attachments to create. 
        /// Ex: <c>1</c> would only create ColorAttachment0.</param>
        /// <exception cref="ArgumentOutOfRangeException">The number of color attachments is negative.</exception>
        public Framebuffer(FramebufferTarget framebufferTarget, int width, int height, 
            PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba, int colorAttachmentsCount = 1) 
            : this(framebufferTarget)
        {
            if (colorAttachmentsCount < 0)
                throw new ArgumentOutOfRangeException("Color attachment count must be non negative.");

            if (TextureFormatTools.IsCompressed(pixelInternalFormat))
                throw new ArgumentException(TextureExceptionMessages.expectedUncompressed);

            Bind();
            PixelInternalFormat = pixelInternalFormat;
            this.width = width;
            this.height = height;

            colorAttachments = CreateColorAttachments(width, height, colorAttachmentsCount);

            SetUpRboDepth(width, height);
        }

        /// <summary>
        /// Gets the framebuffer status for this framebuffer.
        /// </summary>
        /// <returns></returns>
        public string GetStatus()
        {
            // Check if any of the settings were incorrect when creating the fbo.
            Bind();
            return GL.CheckFramebufferStatus(FramebufferTarget).ToString();
        }

        /// <summary>
        /// Attaches <paramref name="texture"/> to <paramref name="framebufferAttachment"/>.
        /// Draw and read buffers must be configured separately.
        /// </summary>
        /// <param name="framebufferAttachment">The attachment target for the texture</param>
        /// <param name="texture">The texture to attach</param>
        public void AttachTexture(FramebufferAttachment framebufferAttachment, Texture2D texture)
        {
            Bind();
            GL.FramebufferTexture2D(FramebufferTarget, framebufferAttachment, TextureTarget.Texture2D, texture.Id, 0);
        }

        /// <summary>
        /// Attaches <paramref name="depthTexture"/> to <paramref name="framebufferAttachment"/>.
        /// </summary>
        /// <param name="framebufferAttachment">The attachment target for the texture. This should be a depth attachment.</param>
        /// <param name="depthTexture">The depth texture to attach</param>
        public void AttachDepthTexture(FramebufferAttachment framebufferAttachment, DepthTexture depthTexture)
        {
            Bind();
            GL.FramebufferTexture2D(FramebufferTarget, framebufferAttachment, TextureTarget.Texture2D, depthTexture.Id, 0);
        }

        /// <summary>
        /// Attaches <paramref name="renderbuffer"/> to <paramref name="framebufferAttachment"/>.
        /// Draw and read buffers must be configured separately.
        /// </summary>
        /// <param name="framebufferAttachment">The attachment target for the renderbuffer</param>
        /// <param name="renderbuffer">The renderbuffer to attach</param>
        public void AttachRenderbuffer(FramebufferAttachment framebufferAttachment, Renderbuffer renderbuffer)
        {
            Bind();
            GL.FramebufferRenderbuffer(FramebufferTarget, framebufferAttachment,
                RenderbufferTarget.Renderbuffer, renderbuffer.Id);
        }

        /// <summary>
        /// Binds the framebuffer to the target specified at creation.
        /// </summary>
        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget, Id);
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

        private Texture2D CreateColorAttachment(int width, int height, FramebufferAttachment framebufferAttachment)
        {
            Texture2D texture = CreateColorAttachmentTexture(width, height);
            return texture;
        }

        private Texture2D CreateColorAttachmentTexture(int width, int height)
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
            AttachRenderbuffer(FramebufferAttachment.DepthAttachment, rboDepth);
        }

        private List<Texture2D> CreateColorAttachments(int width, int height, int colorAttachmentsCount)
        {
            List<Texture2D> colorAttachments = new List<Texture2D>();
            List<DrawBuffersEnum> attachmentEnums = new List<DrawBuffersEnum>();
            for (int i = 0; i < colorAttachmentsCount; i++)
            {
                Texture2D colorAttachment = CreateColorAttachment(width, height, FramebufferAttachment.ColorAttachment0 + i);

                colorAttachments.Add(colorAttachment);
                attachmentEnums.Add(DrawBuffersEnum.ColorAttachment0 + i);

                AttachTexture(FramebufferAttachment.ColorAttachment0 + i, colorAttachment);
            }

            // Draw to all color attachments.
            SetDrawBuffers(attachmentEnums.ToArray());

            return colorAttachments;
        }

        private void Resize()
        {
            Bind();
            
            // Resize all attachments.
            for (int i = 0; i < colorAttachments.Count; i++)
            {
                colorAttachments[i].Bind();
                // TODO: Is it faster to just make a new texture?
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat, width, height, 0, 
                    PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
                AttachTexture(FramebufferAttachment.ColorAttachment0 + i, colorAttachments[i]);
            }

            // Render buffer for the depth attachment, which is necessary for depth testing.
            rboDepth = new Renderbuffer(width, height, RenderbufferStorage.DepthComponent);
            AttachRenderbuffer(FramebufferAttachment.DepthAttachment, rboDepth);
        }
    }
}
