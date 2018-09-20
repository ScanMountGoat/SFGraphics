using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures.TextureFormats;
using SFGraphics.GLObjects.Textures.Utils;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// A TextureTarget.TextureCubeMap texture that supports mipmaps.
    /// </summary>
    public class TextureCubeMap : Texture
    {
        /// <summary>
        /// Creates an empty cube map texture. 
        /// The texture is incomplete until the dimensions and format are set.
        /// </summary>
        public TextureCubeMap() : base(TextureTarget.TextureCubeMap)
        {

        }

        /// <summary>
        /// Initializes an uncompressed cube map without mipmaps from vertically arranged faces in <paramref name="cubeMapFaces"/>.
        /// </summary>
        /// <param name="cubeMapFaces">Faces arranged from top to bottom in the order
        /// X+, X-, Y+, Y-, Z+, Z- </param>
        /// <param name="faceSideLength">The length in pixels of a side of any of the faces</param>
        public void LoadImageData(System.Drawing.Bitmap cubeMapFaces, int faceSideLength = 128)
        {
            Width = faceSideLength;
            Height = faceSideLength;

            // Don't use mipmaps.
            MagFilter = TextureMagFilter.Linear;
            MinFilter = TextureMinFilter.Linear;

            const int cubeMapFaceCount = 6;

            // Faces are arranged vertically from top to bottom in the following order:
            // X +
            // X - 
            // Y + 
            // Y -
            // Z +
            // Z -
            System.Drawing.Rectangle[] cubeMapFaceRegions = new System.Drawing.Rectangle[cubeMapFaceCount];
            for (int i = 0; i < cubeMapFaceRegions.Length; i++)
            {
                cubeMapFaceRegions[i] = new System.Drawing.Rectangle(0, i * faceSideLength, faceSideLength, faceSideLength);
            }

            Bind();
            for (int i = 0; i < cubeMapFaceCount; i++)
            {
                // Copy the pixels for the appropriate face.
                System.Drawing.Bitmap image = cubeMapFaces.Clone(cubeMapFaceRegions[i], cubeMapFaces.PixelFormat);

                // Load the data to the texture.
                System.Drawing.Imaging.BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                image.UnlockBits(data);
            }
        }

        /// <summary>
        /// Initializes a compressed cube map with mipmaps.
        /// Each face should have the same dimensions, image format, and number of mipmaps.
        /// </summary>
        /// <param name="faceSideLength">The side length in pixels of each face. Faces must be square.</param>
        /// <param name="internalFormat"></param>
        /// <param name="mipsPosX">Mipmaps for the positive x target</param>
        /// <param name="mipsNegX">Mipmaps for the negative x target</param>
        /// <param name="mipsPosY">Mipmaps for the positive y target</param>
        /// <param name="mipsNegY">Mipmaps for the negative y target</param>
        /// <param name="mipsPosZ">Mipmaps for the positive z target</param>
        /// <param name="mipsNegZ">Mipmaps for the negative z target</param>
        /// <exception cref="ArgumentException"><paramref name="internalFormat"/> is not a compressed format.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The mipmap counts are not equal for all faces.</exception>
        public void LoadImageData(int faceSideLength, InternalFormat internalFormat, 
            List<byte[]> mipsPosX, List<byte[]> mipsNegX, List<byte[]> mipsPosY, 
            List<byte[]> mipsNegY, List<byte[]> mipsPosZ, List<byte[]> mipsNegZ)
        {
            Width = faceSideLength;
            Height = faceSideLength;

            if (!TextureFormatTools.IsCompressed(internalFormat))
                throw new ArgumentException(TextureExceptionMessages.expectedCompressed);

            bool equalMipCounts = CheckMipMapCountEquality(mipsPosX, mipsNegX, mipsPosY, mipsNegY, mipsPosZ, mipsNegZ);
            if (!equalMipCounts)
                throw new ArgumentOutOfRangeException(TextureExceptionMessages.cubeFaceMipCountDifferent);

            // Necessary to access mipmaps past the base level.
            MinFilter = TextureMinFilter.LinearMipmapLinear;

            Bind();
            MipmapLoading.LoadFacesMipmaps(faceSideLength, internalFormat, mipsPosX, mipsNegX, mipsPosY, 
                mipsNegY, mipsPosZ, mipsNegZ);
        }

        /// <summary>
        /// Initializes an uncompressed cube map without mipmaps.
        /// Each face should have the same dimensions and image format.
        /// </summary>
        /// <param name="faceSideLength"></param>
        /// <param name="textureFormat"></param>
        /// <param name="facePosX">The base mip level for the positive x target</param>
        /// <param name="faceNegX">The base mip level for the negative x target</param>
        /// <param name="facePosY">The base mip level for the positive y target</param>
        /// <param name="faceNegY">The base mip level for the negative y target</param>
        /// <param name="facePosZ">The base mip level for the positive z target</param>
        /// <param name="faceNegZ">The base mip level for the negative z target</param>
        public void LoadImageData(int faceSideLength, TextureFormatUncompressed textureFormat, 
            byte[] facePosX, byte[] faceNegX, byte[] facePosY, byte[] faceNegY, byte[] facePosZ, byte[] faceNegZ) 
        {
            Width = faceSideLength;
            Height = faceSideLength;

            // Don't use mipmaps.
            MagFilter = TextureMagFilter.Linear;
            MinFilter = TextureMinFilter.Linear;

            MipmapLoading.LoadFacesBaseLevel(faceSideLength, textureFormat, facePosX, faceNegX, facePosY, 
                faceNegY, facePosZ, faceNegZ);
        }

        private static bool CheckMipMapCountEquality(List<byte[]> mipsPosX, List<byte[]> mipsNegX, List<byte[]> mipsPosY, List<byte[]> mipsNegY, List<byte[]> mipsPosZ, List<byte[]> mipsNegZ)
        {
            bool equalMipCountX = mipsPosX.Count == mipsNegX.Count;
            bool equalMipCountY = mipsPosY.Count == mipsNegY.Count;
            bool equalMipCountZ = mipsPosZ.Count == mipsNegZ.Count;
            bool equalMipCounts = equalMipCountX && equalMipCountY && equalMipCountZ;
            return equalMipCounts;
        }
    }
}
