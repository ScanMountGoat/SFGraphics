using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Shaders
{
    public sealed partial class Shader
    {
        /// <summary>
        /// Gets the error log containing hardware info, version number, compilation/linker errors, 
        /// and attempts to initialize invalid uniform or vertex attribute names.
        /// </summary>
        /// <returns>A String of all detected errors</returns>
        public string GetErrorLog()
        {
            // Don't append program errors until all the shaders are attached and compiled.
            errorLog.AppendProgramInfoLog(Id);

            // Collect all of the spelling mistakes.
            errorLog.AppendUniformErrors(invalidUniformByName);

            return errorLog.ToString();
        }

        private void AppendShaderCompilationErrors(string shaderName, ShaderType shaderType, int id)
        {
            errorLog.AppendShaderInfoLog(shaderName, shaderType, id);
        }
    }
}
