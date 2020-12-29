using SFGraphics.ShaderGen.GlslShaderUtils;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
