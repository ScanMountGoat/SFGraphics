using System;
using System.IO;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Text;

namespace SFGraphics.GLObjects.Shaders
{
    /// <summary>
    /// Stores hardware info, OpenGL/GLSL version, invalid uniform/attribute names, shader compilation errors, and linker errors.
    /// </summary>
    class ShaderLog
    {
        private StringBuilder errorLog = new StringBuilder();

        public ShaderLog()
        {
            AppendHardwareAndVersionInfo();
        }

        public void AppendProgramInfoLog(int programId)
        {
            errorLog.AppendLine("Program Errors:");
            string error = GL.GetProgramInfoLog(programId);
            errorLog.AppendLine(error);
        }

        public void AppendUniformNameErrors(HashSet<string> invalidUniformNames)
        {
            foreach (string uniform in invalidUniformNames)
                errorLog.AppendLine($"[Warning] Attempted to set undeclared uniform variable { uniform }");
        }
        
        public void AppendUniformTypeErrors(Dictionary<string, ActiveUniformType> invalidUniformTypes)
        {
            foreach (var uniform in invalidUniformTypes)
                errorLog.AppendLine($"[Warning] No uniform variable { uniform.Key } of type { uniform.Value }");
        }

        private void AppendHardwareAndVersionInfo()
        {
            errorLog.AppendLine($"Vendor: { GL.GetString(StringName.Vendor) }");
            errorLog.AppendLine($"Renderer: { GL.GetString(StringName.Renderer) }");
            errorLog.AppendLine($"OpenGL Version: { GL.GetString(StringName.Version) } ");
            errorLog.AppendLine($"GLSL Version: { GL.GetString(StringName.ShadingLanguageVersion) }");
            errorLog.AppendLine();
        }

        public void AppendShaderInfoLog(string shaderName, ShaderType shaderType, int shader)
        {
            // Append compilation errors for the current shader. 
            errorLog.AppendLine($"{ shaderName } { shaderType} Log:");

            string error = GL.GetShaderInfoLog(shader);
            if (error == "")
                errorLog.AppendLine("No Error");
            else
                errorLog.AppendLine(error);

            errorLog.AppendLine(); // line between shaders
        }

        override public string ToString()
        {
            return errorLog.ToString();
        }
    }
}
