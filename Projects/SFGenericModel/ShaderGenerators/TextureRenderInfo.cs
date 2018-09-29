using SFGraphics.GLObjects.Textures;

namespace SFGenericModel.ShaderGenerators
{
    /// <summary>
    /// The texture coordinate values used for rendering.
    /// </summary>
    public enum UvCoord
    {
        /// <summary>
        /// The first UV channel.
        /// </summary>
        TexCoord0,

        /// <summary>
        /// The second UV channel.
        /// </summary>
        TexCoord1,

        /// <summary>
        /// The third UV channel.
        /// </summary>
        TexCoord2,

        /// <summary>
        /// View normals remapped to the 0 to 1 range.
        /// </summary>
        CamEnvSphere,

        /// <summary>
        /// A three component reflection vector used for cube maps.
        /// </summary>
        CubeMap
    }

    /// <summary>
    /// Contains information on how a texture should be rendered.
    /// </summary>
    public struct TextureRenderInfo
    {
        /// <summary>
        /// The name of the uniform variable.
        /// </summary>
        public readonly string name;

        /// <summary>
        /// The value of the uniform variable.
        /// </summary>
        public readonly Texture value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public TextureRenderInfo(string name, Texture value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
