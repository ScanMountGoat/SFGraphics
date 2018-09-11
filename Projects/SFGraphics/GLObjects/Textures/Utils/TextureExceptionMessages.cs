
namespace SFGraphics.GLObjects.Textures.Utils
{
    /// <summary>
    /// Error messages thrown by constructors for classes inheriting from <see cref="Texture"/>.
    /// </summary>
    internal static class TextureExceptionMessages
    {
        public static readonly string expectedCompressed = "The InternalFormat is not " +
            "a compressed image format.";

        public static readonly string expectedUncompressed = "The PixelInternalFormat is not " +
            "an uncompressed image format.";

        public static readonly string cubeFaceMipCountDifferent = "Mipmap count is not equal for all faces.";

        public static readonly string invalidDepthTexFormat = "The PixelInternalFormat not a valid depth component format.";

        public static readonly string genericCompressedFormat = "Generic compressed formats are not supported.";
    }
}
