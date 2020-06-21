using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;
using System.Collections.Generic;
using System.Text;

namespace SFGraphics.GLObjects.Shaders.Utils
{
    /// <summary>
    /// Stores hardware info, OpenGL/GLSL version, invalid uniform/attribute names, shader compilation errors, and linker errors.
    /// </summary>
    internal sealed class ShaderLog
    {
        private readonly Dictionary<string, ActiveUniformInfo> invalidUniformByName = new Dictionary<string, ActiveUniformInfo>();

        private readonly Dictionary<int, ActiveUniformType> samplerTypeByTextureUnit = new Dictionary<int, ActiveUniformType>();

        private readonly StringBuilder errorLog = new StringBuilder();

        public ShaderLog()
        {
            AppendHardwareAndVersionInfo();
        }

        public bool IsValidSamplerType(int textureUnit, ActiveUniformType samplerType)
        {
            // Only one texture type can be bound to a texture unit.
            if (!samplerTypeByTextureUnit.ContainsKey(textureUnit))
            {
                samplerTypeByTextureUnit.Add(textureUnit, samplerType);
                return true;
            }
            else
            {
                return samplerTypeByTextureUnit[textureUnit] == samplerType;
            }
        }

        public bool IsValidUniform(Dictionary<string, ActiveUniformInfo> uniforms, string name, ActiveUniformType type, int size = 1)
        {
            if (!uniforms.ContainsKey(name))
                return false;

            if (uniforms[name].type != type)
                return false;

            if (uniforms[name].size != size)
                return false;

            return true;
        }

        public void LogInvalidUniformSet(UniformSetEventArgs e)
        {
            invalidUniformByName[e.Name] = new ActiveUniformInfo(-1, e.Type, e.Size);
        }

        public void AppendProgramInfoLog(int programId)
        {
            errorLog.AppendLine("Program Errors:");
            string error = GL.GetProgramInfoLog(programId);
            errorLog.AppendLine(error);
        }

        public void AppendUniformErrors()
        {
            foreach (var uniform in invalidUniformByName)
                errorLog.AppendLine($"[Warning] Attempted to set undeclared uniform variable {uniform.Key} of type { uniform.Value.type }");
        }
        
        public void AppendShaderInfoLog(ShaderObject shader)
        {
            // Append compilation errors for the current shader. 
            errorLog.AppendLine($"{shader.ShaderType} Log:");

            string error = GL.GetShaderInfoLog(shader.Id);
            if (error == "")
                errorLog.AppendLine("No Error");
            else
                errorLog.AppendLine(error);

            errorLog.AppendLine(); // line between shaders
        }

        public override string ToString()
        {
            // Collect all of the spelling mistakes.
            AppendUniformErrors();

            return errorLog.ToString();
        }

        private void AppendHardwareAndVersionInfo()
        {
            errorLog.AppendLine($"Vendor: {GL.GetString(StringName.Vendor)}");
            errorLog.AppendLine($"Renderer: {GL.GetString(StringName.Renderer)}");
            errorLog.AppendLine($"OpenGL Version: {GL.GetString(StringName.Version)} ");
            errorLog.AppendLine($"GLSL Version: {GL.GetString(StringName.ShadingLanguageVersion)}");
            errorLog.AppendLine();
        }
    }
}
