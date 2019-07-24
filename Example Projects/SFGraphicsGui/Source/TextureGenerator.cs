using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.BufferObjects;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;

namespace SFGraphicsGui
{
    public static class TextureGenerator
    {
        public static Texture2D CreateStripes(bool usePbo, int width, int height)
        {
            Texture2D floatTexture = new Texture2D
            {
                MinFilter = TextureMinFilter.Nearest,
                MagFilter = TextureMagFilter.Nearest
            };

            Vector3[] pixels = GetImagePixels(width, height);

            if (usePbo)
                LoadFloatTexImageDataPbo(floatTexture, pixels, width, height);
            else
                LoadFloatTexImageData(floatTexture, pixels, width, height);

            return floatTexture;
        }

        private static void LoadFloatTexImageData(Texture2D floatTexture, Vector3[] pixels, int width, int height)
        {
            floatTexture.LoadImageData(width, height, pixels, new TextureFormatUncompressed(PixelInternalFormat.Rgb, PixelFormat.Rgb, PixelType.Float));
        }

        private static void LoadFloatTexImageDataPbo(Texture2D floatTexture, Vector3[] pixels, int width, int height)
        {
            BufferObject pixelBuffer = new BufferObject(BufferTarget.PixelUnpackBuffer);
            pixelBuffer.SetData(pixels, BufferUsageHint.StaticDraw);
            floatTexture.LoadImageData(width, height, pixelBuffer, new TextureFormatUncompressed(PixelInternalFormat.Rgb, PixelFormat.Rgb, PixelType.Float));
        }

        private static Vector3[] GetImagePixels(int width, int height)
        {
            Vector3[] pixels = new Vector3[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                // Magenta and black stripes.
                pixels[i] = new Vector3(1, 0, 1) * (i % 2);
            }

            return pixels;
        }
    }
}
