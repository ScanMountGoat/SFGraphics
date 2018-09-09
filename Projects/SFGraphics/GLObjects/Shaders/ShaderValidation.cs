﻿using OpenTK.Graphics.OpenGL;


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

        private bool CheckProgramStatus()
        {
            // Check for linker errors. 
            // Compilation errors in individual shaders will also cause linker errors.
            bool programLinkedSuccessfully = GetProgramLinkStatus();
            return programLinkedSuccessfully;
        }

        private bool GetProgramLinkStatus()
        {
            // 1: linked successfully. 0: linker errors
            int linkStatus = 1;
            GL.GetProgram(Id, GetProgramParameterName.LinkStatus, out linkStatus);
            return linkStatus != 0;
        }

        private bool GetProgramValidateStatus()
        {
            // 1: valid. 0: not valid
            int validateStatus = 1;
            GL.ValidateProgram(Id);
            GL.GetProgram(Id, GetProgramParameterName.ValidateStatus, out validateStatus);
            return validateStatus != 0;
        }
    }
}
