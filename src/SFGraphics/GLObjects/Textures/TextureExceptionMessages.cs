

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// Error messages thrown by constructors for classes inheriting from <see cref="Texture"/>.
    /// </summary>
    internal static class TextureExceptionMessages
    {
        public static readonly string formatShouldBeCompressed = "The InternalFormat must be " +
            "a compressed image format.";

        public static readonly string formatShouldNotBeCompressed = "The PixelInternalFormat must be " +
            "an uncompressed image format.";

        public static readonly string cubeFaceMipCountDifferent = "Mipmap count must be equal for all faces.";
    }
}
