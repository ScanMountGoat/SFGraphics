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
    /// The channels of the texture assigned to the output color.
    /// </summary>
    public enum TextureSwizzle
    {
        /// <summary>
        /// Use the red, green, and blue channels. Alpha is set to 1.
        /// </summary>
        Rgb,
        
        /// <summary>
        /// Use the red channel for the output RGB channels. Alpha is set to 1.
        /// </summary>
        R,

        /// <summary>
        /// Use the green channel for the output RGB channels. Alpha is set to 1.
        /// </summary>
        G,

        /// <summary>
        /// Use the blue channel for the output RGB channels. Alpha is set to 1.
        /// </summary>
        B,

        /// <summary>
        /// Use the alpha channel for the output RGB channels. Alpha is set to 1.
        /// </summary>
        A
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
        /// The channels of the texture that will be assigned to the output
        /// </summary>
        public TextureSwizzle TextureSwizzle { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="uvCoord"></param>
        /// <param name="textureSwizzle"></param>
        public TextureRenderInfo(string name, UvCoord uvCoord, TextureSwizzle textureSwizzle) : this()
        {
            Name = name;
            UvCoord = uvCoord;
            TextureSwizzle = textureSwizzle;
        }
    }
}
