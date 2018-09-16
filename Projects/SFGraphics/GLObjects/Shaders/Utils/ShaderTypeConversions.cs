using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace SFGraphics.GLObjects.Shaders.Utils
{
    internal static class ShaderTypeConversions
    {
        private static Dictionary<TextureTarget, ActiveUniformType> uniformTypeByTextureTarget = new Dictionary<TextureTarget, ActiveUniformType>()
        {
            { TextureTarget.Texture2D, ActiveUniformType.Sampler2D },
            { TextureTarget.TextureCubeMap, ActiveUniformType.SamplerCube }
        };
    
        public static ActiveUniformType GetUniformType(TextureTarget target)
        {
            if (!uniformTypeByTextureTarget.ContainsKey(target))
                throw new NotImplementedException($"{target.ToString()} is not supported");
            else
                return uniformTypeByTextureTarget[target];
        }
    }
}
