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
        public string Name { get; }

        /// <summary>
        /// Determines what UV coordinates will be used
        /// </summary>
        public UvCoord UvCoord { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="uvCoord"></param>
        public TextureRenderInfo(string name, UvCoord uvCoord) : this()
        {
            Name = name;
            UvCoord = uvCoord;
        }
    }
}
