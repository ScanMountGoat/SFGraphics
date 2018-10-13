using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using System;

namespace RenderTestUtils
{
    public static class ShaderTestUtils
    {
        public static Shader CreateValidShader()
        {
            Shader shader = new Shader();

            string fragSource = ResourceShaders.GetShaderSource("valid.frag");
            string vertSource = ResourceShaders.GetShaderSource("valid.vert");

            var shaders = new List<Tuple<string, ShaderType, string>>()
            {
                new Tuple<string, ShaderType, string>(fragSource, ShaderType.FragmentShader, ""),
                new Tuple<string, ShaderType, string>(vertSource, ShaderType.VertexShader, ""),
            };
            shader.LoadShaders(shaders);

            shader.UseProgram();

            return shader;
        }

        public static string GetInvalidUniformErrorMessage(string name, ActiveUniformType type)
        {
            return $"[Warning] Attempted to set undeclared uniform variable {name} of type {type}";
        }
    }
}
