using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// A <see cref="Texture"/> class for TextureTarget.TextureCubeMap textures. 
    /// Provides a constructor for initializing the cube map faces from faces arranged vertically in a single bitmap.
    /// </summary>
    public class TextureCubeMap : Texture
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cubeMapFaces">Faces arranged from top to bottom in the order
        /// X+, X-, Y+, Y-, Z+, Z- </param>
        /// <param name="sideLength">The length in pixels of a side of any of the faces</param>
        public TextureCubeMap(Bitmap cubeMapFaces, int sideLength = 128) : base(TextureTarget.TextureCubeMap, PixelInternalFormat.Rgba)
        {
            Bind();

            // Don't use mipmaps.
            MagFilter = TextureMagFilter.Linear;
            MinFilter = TextureMinFilter.Linear;

            const int cubeMapFaceCount = 6;

            // The cube map resolution is currently hardcoded...
            // Faces are arranged vertically in the following order from top to bottom:
            // X+, X-, Y+, Y-, Z+, Z-
            Rectangle[] cubeMapFaceRegions = new Rectangle[cubeMapFaceCount];
            for (int i = 0; i < cubeMapFaceRegions.Length; i++)
            {
                cubeMapFaceRegions[i] = new Rectangle(0, i * sideLength, sideLength, sideLength);
            }

            for (int i = 0; i < cubeMapFaceCount; i++)
            {
                // Copy the pixels for the appropriate face.
                Bitmap image = cubeMapFaces.Clone(cubeMapFaceRegions[i], cubeMapFaces.PixelFormat);

                // Load the data to the texture.
                BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                image.UnlockBits(data);
            }
        }

        /// <summary>
        /// Initializes a compressed cube map with mipmaps.
        /// Each face should have the same dimensions, image format, and number of mipmaps.
        /// </summary>
        /// <param name="faceWidth">The width in pixels of each face</param>
        /// <param name="faceHeight">The height in pixels of each face</param>
        /// <param name="internalFormat"></param>
        /// <param name="mipsPosX">Mipmaps for the positive x target</param>
        /// <param name="mipsNegX">Mipmaps for the negative x target</param>
        /// <param name="mipsPosY">Mipmaps for the positive y target</param>
        /// <param name="mipsNegY">Mipmaps for the negative y target</param>
        /// <param name="mipsPosZ">Mipmaps for the positive z target</param>
        /// <param name="mipsNegZ">Mipmaps for the negative z target</param>
        public TextureCubeMap(int faceWidth, int faceHeight, InternalFormat internalFormat, List<byte[]> mipsPosX, List<byte[]> mipsNegX,
            List<byte[]> mipsPosY, List<byte[]> mipsNegY, List<byte[]> mipsPosZ, List<byte[]> mipsNegZ) : base(TextureTarget.TextureCubeMap)
        {
            // TODO: Check that mip counts are equal. Check internalFormat for compressed format.

            // Necessary to access mipmaps past the base level.
            MinFilter = TextureMinFilter.LinearMipmapLinear;

            Bind();

            // The number of mipmaps needs to be specified first.
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMaxLevel, mipsPosX.Count);
            GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);

            // Load mipmaps for all faces.
            MipmapLoading.LoadCompressedMipMaps(TextureTarget.TextureCubeMapPositiveX, faceWidth, faceHeight, mipsPosX, internalFormat);
            MipmapLoading.LoadCompressedMipMaps(TextureTarget.TextureCubeMapNegativeX, faceWidth, faceHeight, mipsNegX, internalFormat);

            MipmapLoading.LoadCompressedMipMaps(TextureTarget.TextureCubeMapPositiveY, faceWidth, faceHeight, mipsPosY, internalFormat);
            MipmapLoading.LoadCompressedMipMaps(TextureTarget.TextureCubeMapNegativeY, faceWidth, faceHeight, mipsNegY, internalFormat);

            MipmapLoading.LoadCompressedMipMaps(TextureTarget.TextureCubeMapPositiveZ, faceWidth, faceHeight, mipsPosZ, internalFormat);
            MipmapLoading.LoadCompressedMipMaps(TextureTarget.TextureCubeMapNegativeZ, faceWidth, faceHeight, mipsNegZ, internalFormat);
        }
    }
}
