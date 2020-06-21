using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace SFGraphics.GLObjects.Shaders
{
    public sealed partial class Shader
    {

        /// <summary>
        /// Attaches one or more shaders and links the program.
        /// </summary>
        /// <param name="shaders">(shader source, shader type, shader name)</param>
        public void LoadShaders(params ShaderObject[] shaders)
        {
            foreach (var shader in shaders)
            {
                AttachShader(shader);
            }

            // Linking only needs to be done once, which improves performance.
            LinkProgramSetUpVariables();
        }

        /// <summary>
        /// Compiles and attaches shaders from <paramref name="fragmentSource"/> and <paramref name="vertexSource"/>.
        /// </summary>
        /// <param name="vertexSource">The vertex shader source</param>
        /// <param name="fragmentSource">The fragment shader source</param>
        public void LoadShaders(string vertexSource, string fragmentSource)
        {
            LoadShaders(
                new ShaderObject(vertexSource, ShaderType.VertexShader),
                new ShaderObject(fragmentSource, ShaderType.FragmentShader)
            );
        }

        /// <summary>
        /// Gets the compiled program binary for the program <see cref="GLObject.Id"/>.
        /// This method should be called after the shaders are loaded and the program is linked.
        /// Hardware or software changes may cause compatibility issues with the program binary.
        /// </summary>
        /// <param name="programBinary">The compiled shader program binary</param>
        /// <param name="binaryFormat">The platform specific binary format</param>
        /// <returns><c>true</c> if the binary was successfully created</returns>
        public bool GetProgramBinary(out byte[] programBinary, out BinaryFormat binaryFormat)
        {
            if (!linkStatusIsOk)
            {
                programBinary = null;
                binaryFormat = 0;
                return false;
            }

            // bufSize is used for the array's length instead of the length parameter.
            GL.GetProgram(Id, (GetProgramParameterName)GL_PROGRAM_BINARY_MAX_LENGTH, out int bufSize);
            programBinary = new byte[bufSize];
            GL.GetProgramBinary(Id, bufSize, out _, out binaryFormat, programBinary);
            return true;
        }

        /// <summary>
        /// Loads the entire program from the compiled binary.
        /// <para></para><para></para>
        /// Binaries are hardware and driver version specific.
        /// If program creation fails with precompiled binaries, resort to compiling the shaders from source. 
        /// </summary>
        /// <param name="binaryFormat">The format of the compiled binary</param>
        /// <param name="programBinary">The compiled program binary</param>
        /// <returns><c>true</c> if the binary was loaded successfully</returns>
        [HandleProcessCorruptedStateExceptions]
        public bool TryLoadProgramFromBinary(byte[] programBinary, BinaryFormat binaryFormat)
        {
            try
            {
                // Linking isn't necessary when loading a program binary.
                GL.ProgramBinary(Id, binaryFormat, programBinary, programBinary.Length);
            }
            catch (AccessViolationException)
            {
                // The binary is corrupt or the wrong format. 
                return false;
            }

            LinkStatusIsOk = ShaderValidation.GetProgramLinkStatus(Id);
            TryLoadShaderVariables();
            return true;
        }

        /// <summary>
        /// Gets the shader source for all attached shaders.
        /// </summary>
        /// <returns>An array of shader sources for all attached shaders</returns>
        public string[] GetShaderSources()
        {
            return attachedShaders.Select(shader => shader.GetShaderSource()).ToArray();
        }

        private void AttachShader(ShaderObject shader)
        {
            GL.AttachShader(Id, shader.Id);
            attachedShaders.Add(shader);
            errorLog.AppendShaderInfoLog(shader);
        }

        private void LinkProgramSetUpVariables()
        {
            GL.LinkProgram(Id);
            LinkStatusIsOk = ShaderValidation.GetProgramLinkStatus(Id);

            TryLoadShaderVariables();
        }

        private bool TryLoadShaderVariables()
        {
            // Scary things happen if we do this after a linking error.
            if (!LinkStatusIsOk)
                return false;

            LoadAttributes();
            LoadUniforms();
            return true;
        }



        private void AddActiveAttribute(int index)
        {
            string name = GL.GetActiveAttrib(Id, index, out int size, out ActiveAttribType type);
            int location = GL.GetAttribLocation(Id, name);

            // Overwrite existing vertex attributes.
            activeAttribByName[name] = new ActiveAttribInfo(location, type, size);
        }

        private void AddActiveUniform(int index)
        {
            string name = GL.GetActiveUniform(Id, index, out int size, out ActiveUniformType type);

            string nameNoArrayIndex = GetNameNoArrayBrackets(name);
            string nameArrayIndex0 = nameNoArrayIndex + "[0]";

            // Uniform arrays can be "array[0]" or "array"
            int location = GL.GetUniformLocation(Id, nameNoArrayIndex);
            if (location == -1)
                location = GL.GetUniformLocation(Id, nameArrayIndex0);

            // Overwrite existing uniforms.
            activeUniformByName[nameNoArrayIndex] = new ActiveUniformInfo(location, type, size);
        }

        private static string GetNameNoArrayBrackets(string name)
        {
            if (name.Contains("["))
                return name.Substring(0, name.IndexOf('['));
            else
                return name;
        }

        private void LoadUniforms()
        {
            // Locations may change when linking the shader again.
            GL.GetProgram(Id, GetProgramParameterName.ActiveUniforms, out int activeUniformCount);
            ActiveUniformCount = activeUniformCount;

            activeUniformByName = new Dictionary<string, ActiveUniformInfo>(ActiveUniformCount);

            for (int i = 0; i < ActiveUniformCount; i++)
            {
                AddActiveUniform(i);
            }
        }

        private void LoadAttributes()
        {
            // Locations may change when linking the shader again.
            GL.GetProgram(Id, GetProgramParameterName.ActiveAttributes, out int activeAttributeCount);
            ActiveAttributeCount = activeAttributeCount;

            activeAttribByName = new Dictionary<string, ActiveAttribInfo>(ActiveAttributeCount);

            for (int i = 0; i < ActiveAttributeCount; i++)
            {
                AddActiveAttribute(i);
            }
        }
    }
}
