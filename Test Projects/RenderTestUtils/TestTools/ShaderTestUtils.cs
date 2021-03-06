﻿using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;
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

            shader.LoadShaders(
                new ShaderObject(fragSource, ShaderType.FragmentShader),
                new ShaderObject(vertSource, ShaderType.VertexShader));

            shader.UseProgram();

            return shader;
        }

        public static string GetInvalidUniformErrorMessage(string name, ActiveUniformType type)
        {
            return $"[Warning] Attempted to set undeclared uniform variable {name} of type {type}";
        }
    }
}
