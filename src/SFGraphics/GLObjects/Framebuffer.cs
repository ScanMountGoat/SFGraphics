﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace SFGraphics.GLObjects
{
    /// <summary>
    /// Encapsulates an OpenGL framebuffer, including any attached color or depth attachments.
    /// </summary>
    public class Framebuffer : IGLObject
    {
        /// <summary>
        /// The value generated by GL.GenFramebuffer(). Do not attempt to bind <see cref="Id"/> when the object has become unreachable.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The target which <see cref="Id"/> is bound when calling <see cref="Bind"/>.
        /// </summary>
        public FramebufferTarget FramebufferTarget { get; }

        /// <summary>
        /// 
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

        private int colorAttachment0Tex;
        /// <summary>
        /// The Id of the first color attachment.
        /// </summary>
        public int ColorAttachment0Tex { get { return colorAttachment0Tex; } }

        private int rboDepth;

        /// <summary>
        /// Generates an empty framebuffer bound to the specified target. Binds the framebuffer.
        /// </summary>
        /// <param name="framebufferTarget">The target which <see cref="Id"/> is bound.</param>
        public Framebuffer(FramebufferTarget framebufferTarget)
        {
            Id = GL.GenFramebuffer();
            GLObjectManager.AddReference(GLObjectManager.referenceCountByFramebufferId, Id);
            FramebufferTarget = framebufferTarget;
            Bind();
        }

        /// <summary>
        /// Decrement the reference count for <see cref="Id"/>. The context probably isn't current, so the data is deleted later by <see cref="GLObjectManager"/>.
        /// </summary>
        ~Framebuffer()
        {
            GLObjectManager.RemoveReference(GLObjectManager.referenceCountByFramebufferId, Id);
        }

        /// <summary>
        /// Generates a framebuffer with a color attachment of the specified pixel format and dimensions. A render buffer of the same dimensions as the color attachment is generated for the depth component.
        /// Binds the framebuffer.
        /// </summary>
        /// <param name="framebufferTarget"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixelInternalFormat"></param>
        public Framebuffer(FramebufferTarget framebufferTarget, int width, int height, PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba) : this(framebufferTarget)
        {
            Bind();
            PixelInternalFormat = pixelInternalFormat;
            this.width = width;
            this.height = height;

            SetupColorAttachment0(width, height);
            SetupRboDepth(width, height);
        }

        /// <summary>
        /// Gets the named framebuffer status for this framebuffer.
        /// </summary>
        /// <returns></returns>
        public String GetStatus()
        {
            // Check if any of the settings were incorrect when creating the fbo.
            return GL.CheckNamedFramebufferStatus(Id, FramebufferTarget).ToString();
        }

        private void SetupColorAttachment0(int width, int height)
        {
            // First color attachment.
            colorAttachment0Tex = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, ColorAttachment0Tex);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.FramebufferTexture2D(FramebufferTarget, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, ColorAttachment0Tex, 0);
        }

        private void SetupRboDepth(int width, int height)
        {
            // Render buffer for the depth attachment, which is necessary for depth testing.
            GL.GenRenderbuffers(1, out rboDepth);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rboDepth);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, rboDepth);
        }

        /// <summary>
        /// Reads the framebuffer's contents into a Bitmap using GL.ReadPixels. 
        /// This is intended for screenshots, so it only works properly for framebuffers of type 
        /// PixelFormat.Rgba.
        /// </summary>
        /// <param name="saveAlpha">The alpha channel is saved when true or set to 255 (white) when false</param>
        /// <returns></returns>
        public Bitmap ReadImagePixels(bool saveAlpha = false)
        {
            // Calculate the number of bytes needed.
            int pixelByteLength = width * height * sizeof(float);
            byte[] pixels = new byte[pixelByteLength];

            // Read the pixels from the framebuffer. PNG uses the BGRA format. 
            // This probably won't work for HDR textures.
            Bind();
            GL.ReadPixels(0, 0, width, height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, pixels);
            byte[] fixedPixels = CopyImagePixels(width, height, saveAlpha, pixelByteLength, pixels);

            // Format and save the data
            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(fixedPixels, 0, bmpData.Scan0, fixedPixels.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        /// <summary>
        /// The origin (0,0) corresponds to the top left of the screen.
        /// The coordinates are based on the framebuffer's dimensions 
        /// and not the screen's dimensions.
        /// </summary>
        /// <param name="x">The horizontal pixel coordinate</param>
        /// <param name="y">The vertical pixel coordinate</param>
        /// <returns>A color with the RGBA values of the selected pixel</returns>
        public Color SamplePixelColor(int x, int y)
        {
            Bind();
            // Only RGBA is supported for now.
            byte[] rgba = new byte[4];
            GL.ReadPixels(x, y, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, rgba);
            return Color.FromArgb(rgba[3], rgba[0], rgba[1], rgba[2]);
        }

        private static byte[] CopyImagePixels(int width, int height, bool saveAlpha, int pixelByteLength, byte[] pixels)
        {
            // Flip data because glReadPixels reads it in from bottom row to top row
            byte[] fixedPixels = new byte[pixelByteLength];
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    // Remove alpha blending from the end image - we just want the post-render colors
                    if (!saveAlpha)
                        pixels[((w + h * width) * sizeof(float)) + 3] = 255;

                    // Copy a 4 byte pixel one at a time
                    Array.Copy(pixels, (w + h * width) * sizeof(float), fixedPixels, ((height - h - 1) * width + w) * sizeof(float), sizeof(float));
                }
            }

            return fixedPixels;
        }

        /// <summary>
        /// Binds the framebuffer to the target specified at creation.
        /// </summary>
        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget, Id);
        }

        private void Resize()
        {
            Bind();

            // First color attachment (regular texture).
            GL.BindTexture(TextureTarget.Texture2D, ColorAttachment0Tex);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
            GL.FramebufferTexture2D(FramebufferTarget, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, ColorAttachment0Tex, 0);

            // Render buffer for the depth attachment, which is necessary for depth testing.
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rboDepth);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, rboDepth);

            // Bind the default framebuffer again.
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
    }
}
