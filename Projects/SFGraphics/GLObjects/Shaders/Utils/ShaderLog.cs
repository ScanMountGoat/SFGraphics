using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Text;

namespace SFGraphics.GLObjects.Shaders.Utils
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

        public void AppendUniformErrors(Dictionary<string, ActiveUniformInfo> invalidUniforms)
        {
            foreach (var uniform in invalidUniforms)
                errorLog.AppendLine($"[Warning] Attempted to set undeclared uniform variable { uniform.Key } of type { uniform.Value.type }");
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
