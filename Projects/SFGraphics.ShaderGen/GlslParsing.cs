using SFGraphics.ShaderGen.GlslShaderUtils;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SFGraphics.ShaderGen
{
    public static class GlslParsing
    {
        public static List<ShaderUniform> GetUniforms(string shaderSource)
        {
            var uniforms = new List<ShaderUniform>();

            foreach (Match match in Regex.Matches(shaderSource, @"uniform .* .*;"))
            {
                var parts = match.Value.Split(' ');
                uniforms.Add(new ShaderUniform(parts[2].TrimEnd(';'), parts[1]));
            }
            return uniforms;
        }

        public static List<ShaderAttribute> GetAttributes(string shaderSource)
        {
            var uniforms = new List<ShaderAttribute>();

            foreach (Match match in Regex.Matches(shaderSource, @"in .* .*;"))
            {
                var parts = match.Value.Split(' ');
                var name = parts[2].TrimEnd(';');
                var type = ShaderAttribute.GetAttributeType(parts[1]);
                uniforms.Add(new ShaderAttribute(name, type));
            }
            return uniforms;
        }
    }
}
