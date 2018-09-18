﻿using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders.Utils;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;

namespace SFGraphics.GLObjects.Shaders
{
    /// <summary>
    /// Encapsulates a shader program and attached shaders. 
    /// Errors are stored to an internal log, which can be exported with <see cref="GetErrorLog"/>.
    /// <para></para> <para></para>
    /// Ensure that <see cref="LinkStatusIsOk"/> returns <c>true</c> before rendering to avoid crashes.
    /// </summary>
    public sealed partial class Shader : GLObject
    {
        internal override GLObjectType ObjectType { get { return GLObjectType.ShaderProgram; } }

        /// <summary>
        /// If <c>false</c>, rendering with this shader will most likely throw an <see cref="AccessViolationException"/>.
        /// <para></para><para></para>
        /// Does not reflect changes made using <see cref="GLObject.Id"/> directly.
        /// </summary>
        public bool LinkStatusIsOk
        {
            get { return linkStatusIsOk; }
            private set
            {
                if (linkStatusIsOk != value)
                    OnLinkStatusChanged?.Invoke(this, value);
                linkStatusIsOk = value;
            }
        }
        private bool linkStatusIsOk = false;

        /// <summary>
        /// <c>true</c> when only one sampler type is used for each texture unit
        /// and the number of active samplers does not exceed the allowed maximum.
        /// <para></para><para></para>
        /// This should be checked at runtime and only for debugging purposes.
        /// </summary>
        public bool ValidateStatusIsOk {  get { return GetProgramValidateStatus(); } }

        /// <summary>
        /// </summary>
        /// <param name="sender">The shader that generated the error</param>
        /// <param name="e">The arguments used to set the uniform</param>
        public delegate void InvalidUniformSetEventHandler(Shader sender, UniformSetEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The shader that generated the event</param>
        /// <param name="linkStatusIsOk">The new link status. 
        /// <c>true</c> when linking was successful</param>
        public delegate void LinkStatusChangedEventHandler(Shader sender, bool linkStatusIsOk);

        /// <summary>
        /// Occurs when arguments for setting a uniform don't match the shader.
        /// </summary>
        public event InvalidUniformSetEventHandler OnInvalidUniformSet;

        /// <summary>
        /// Occurs when a call to <see cref="SetTexture(string, Textures.Texture, int)"/>
        /// is made to a previously used texture unit but with a different sampler type.
        /// </summary>
        public event InvalidUniformSetEventHandler OnTextureUnitTypeMismatch;

        /// <summary>
        /// Occurs when the value of <see cref="LinkStatusIsOk"/> changes.
        /// </summary>
        public event LinkStatusChangedEventHandler OnLinkStatusChanged;

        private int activeUniformCount;
        private int activeAttributeCount;

        private ShaderLog errorLog = new ShaderLog();

        private Dictionary<string, ActiveUniformInfo> activeUniformByName = new Dictionary<string, ActiveUniformInfo>();
        private Dictionary<string, ActiveAttribInfo> activeAttribByName = new Dictionary<string, ActiveAttribInfo>();

        private Dictionary<string, ActiveUniformInfo> invalidUniformByName = new Dictionary<string, ActiveUniformInfo>();
        private Dictionary<string, ActiveAttribInfo> invalidAttribSetByName = new Dictionary<string, ActiveAttribInfo>();

        private Dictionary<int, ActiveUniformType> samplerTypeByTextureUnit = new Dictionary<int, ActiveUniformType>();

        // This isn't in OpenTK's enums for some reason.
        // https://www.khronos.org/registry/OpenGL/api/GL/glcorearb.h
        private static readonly int GL_PROGRAM_BINARY_MAX_LENGTH = 0x8741;

        /// <summary>
        /// Creates an unitialized shader program. Load shaders before using the shader program.
        /// </summary>
        public Shader() : base(GL.CreateProgram())
        {

        }

        private void LogInvalidUniformSet(UniformSetEventArgs e)
        {
            invalidUniformByName[e.Name] = new ActiveUniformInfo(-1, e.Type, e.Size);
        }

        /// <summary>
        /// Use <see cref="GLObject.Id"/> as the current program.
        /// </summary>
        public void UseProgram()
        {
            GL.UseProgram(Id);
        }

        /// <summary>
        /// Enables the vertex attribute arrays for all active attributes
        /// for the currently bound vertex array object.
        /// Ensures that extra attributes aren't enabled, which causes crashes on Geforce drivers.
        /// </summary>
        public void EnableVertexAttributes()
        {
            // Only enable the necessary vertex attributes.
            foreach (var attribute in activeAttribByName)
            {
                GL.EnableVertexAttribArray(attribute.Value.location);
            }
        }

        /// <summary>
        /// Disables the vertex attribute arrays for all active attributes 
        /// for the currently bound vertex array object.
        /// Ensures that extra attributes aren't enabled, which causes crashes on Geforce drivers.
        /// </summary>
        public void DisableVertexAttributes()
        {
            // Enabling extra vertex attributes can cause crashes on some drivers.
            foreach (var attribute in activeAttribByName)
            {
                GL.DisableVertexAttribArray(attribute.Value.location);
            }
        }

        /// <summary>
        /// Attaches <paramref name="shaderId"/> and links the program. 
        /// The value for <see cref="LinkStatusIsOk"/> is updated.
        /// </summary>
        /// <param name="shaderId">The integer ID returned by <see cref="CreateGlShader(string, ShaderType)"/></param>
        /// <param name="shaderType">The type of shader.
        /// Ex: ShaderType.FragmentShader</param>        
        /// <param name="shaderName"></param>
        public void AttachShader(int shaderId, ShaderType shaderType, string shaderName = "")
        {
            AttachShaderNoLink(shaderId, shaderType, shaderName);
            LinkProgramSetUpVariables();
        }

        /// <summary>
        /// Compiles and attaches a single shader from <paramref name="shaderSource"/>.
        /// Performs linking and shader setup.
        /// </summary>
        /// <param name="shaderSource">The shader code source</param>
        /// <param name="shaderType">The type of shader to load</param>
        /// <param name="shaderName">The title used for the compilation errors section of the error log</param>
        public void LoadShader(string shaderSource, ShaderType shaderType, string shaderName = "Shader")
        {
            int shaderId = CreateGlShader(shaderSource, shaderType);
            AttachShader(shaderId, shaderType, shaderName);
        }

        /// <summary>
        /// Compiles and attaches multiple shaders. Linking and setup is performed only once.
        /// </summary>
        /// <param name="shaders">(shader source, shader type, shader name)</param>
        public void LoadShaders(List<Tuple<string, ShaderType, string>> shaders)
        {
            foreach (var shader in shaders)
            {
                int shaderId = CreateGlShader(shader.Item1, shader.Item2);
                AttachShaderNoLink(shaderId, shader.Item2, shader.Item3);
            }

            LinkProgramSetUpVariables();
        }

        /// <summary>
        /// Creates and compiles a new shader from <paramref name="shaderSource"/>.
        /// Returns the ID created by GL.CreateShader(). 
        /// Shaders can be attached with <see cref="AttachShader(int, ShaderType, string)"/>
        /// </summary>
        /// <param name="shaderSource">A string containing the shader source text</param>
        /// <param name="shaderType">The type of shader to create</param>
        /// <returns>The integer ID created by GL.CreateShader()</returns>
        public static int CreateGlShader(string shaderSource, ShaderType shaderType)
        {
            int id = GL.CreateShader(shaderType);
            GL.ShaderSource(id, shaderSource);
            GL.CompileShader(id);
            return id;
        }

        /// <summary>
        /// Gets the compiled program binary for the program <see cref="GLObject.Id"/>.
        /// This method should be called after the shaders are loaded and the program is linked.
        /// Hardware or software changes may cause compatibility issues with the program binary.
        /// </summary>
        /// <param name="binaryFormat"></param>
        /// <returns></returns>
        public byte[] GetProgramBinary(out BinaryFormat binaryFormat)
        {
            // bufSize is used for the array's length instead of the length parameter.
            GL.GetProgram(Id, (GetProgramParameterName)GL_PROGRAM_BINARY_MAX_LENGTH, out int bufSize);
            byte[] programBinary = new byte[bufSize];

            GL.GetProgramBinary(Id, bufSize, out int length, out binaryFormat, programBinary);
            return programBinary;
        }

        /// <summary>
        /// Loads the entire program from the compiled binary and format generated 
        /// by <see cref="GetProgramBinary(out BinaryFormat)"/>.
        /// The value returned by <see cref="LinkStatusIsOk"/> is updated.
        /// <para></para><para></para>
        /// Hardware or software changes may cause compatibility issues with the program binary.
        /// If program creation fails with precompiled binaries, resort to compiling the shaders from source. 
        /// </summary>
        /// <param name="binaryFormat">The format of the compiled binary</param>
        /// <param name="programBinary">The compiled program binary</param>
        public void LoadProgramBinary(byte[] programBinary, BinaryFormat binaryFormat)
        {
            GL.ProgramBinary(Id, binaryFormat, programBinary, programBinary.Length);

            LinkStatusIsOk = CheckProgramStatus();

            // Scary things happen if we do this after a linking error.
            if (LinkStatusIsOk)
                LoadShaderVariables();
        }

        /// <summary>
        /// Gets the uniform's location from locations stored after linking.
        /// </summary>
        /// <param name="name">The name of the uniform variable</param>
        /// <returns>The location of <paramref name="name"/></returns>
        public int GetUniformLocation(string name)
        {
            string nameNoBrackets = GetNameNoArrayBrackets(name);

            if (!activeUniformByName.ContainsKey(nameNoBrackets))
                return -1;
            else
                return activeUniformByName[nameNoBrackets].location;
        }

        /// <summary>
        /// Gets the attribute's location from locations stored after linking.
        /// </summary>
        /// <param name="name">The name of the vertex attribute variable</param>
        /// <returns>The location of <paramref name="name"/></returns>
        public int GetAttribLocation(string name)
        {
            if (!activeAttribByName.ContainsKey(name))
                return -1;
            else
                return activeAttribByName[name].location;
        }

        /// <summary>
        /// Gets the block index of a uniform block.
        /// </summary>
        /// <param name="name">The name of the uniform block</param>
        /// <returns>The index of <paramref name="name"/></returns>
        public int GetUniformBlockIndex(string name)
        {
            return GL.GetUniformBlockIndex(Id, name);
        }

        private void AttachShaderNoLink(int shaderId, ShaderType shaderType, string shaderName = "")
        {
            GL.AttachShader(Id, shaderId);
            AppendShaderCompilationErrors(shaderName, shaderType, shaderId);

            // The shader won't be deleted until the program is deleted.
            GL.DeleteShader(shaderId);
        }

        private void LinkProgramSetUpVariables()
        {
            GL.LinkProgram(Id);
            LinkStatusIsOk = CheckProgramStatus();

            // Scary things happen if we do this after a linking error.
            if (LinkStatusIsOk)
                LoadShaderVariables();
        }

        private void LoadShaderVariables()
        {
            LoadAttributes();
            LoadUniforms();
        }

        private void AddVertexAttribute(string name, ActiveAttribType type, int size)
        {
            // Overwrite existing vertex attributes.
            int location = GL.GetAttribLocation(Id, name);        
            activeAttribByName[name] = new ActiveAttribInfo(location, type, size);
        }

        private void AddUniform(string name, ActiveUniformType type, int size)
        {
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
            string nameNoArrayIndex = name;
            if (name.Contains("["))
                nameNoArrayIndex = name.Substring(0, name.IndexOf('['));
            return nameNoArrayIndex;
        }

        private void LoadUniforms()
        {
            // Uniform locations may change when linking the shader again.
            activeUniformByName.Clear();

            GL.GetProgram(Id, GetProgramParameterName.ActiveUniforms, out activeUniformCount);
            for (int i = 0; i < activeUniformCount; i++)
            {
                // Ignore invalid uniforms. 0 is "None" for type.
                string uniform = GL.GetActiveUniform(Id, i, out int uniformSize, out ActiveUniformType uniformType);
                if (uniformType != 0)
                {
                    AddUniform(uniform, uniformType, uniformSize);
                }
            }
        }

        private void LoadAttributes()
        {
            GL.GetProgram(Id, GetProgramParameterName.ActiveAttributes, out activeAttributeCount);

            for (int i = 0; i < activeAttributeCount; i++)
            {
                ActiveAttribType type;
                string name = GL.GetActiveAttrib(Id, i, out int size, out type);
                // Ignore invalid attributes.
                if (type != ActiveAttribType.None)
                {
                    AddVertexAttribute(name, type, size);
                }
            }
        }

        private int LoadShaderBasedOnType(string shaderSource, ShaderType shaderType)
        {
            // Returns the shader Id that was generated.
            int id = AttachAndCompileShader(shaderSource, shaderType, Id);
            return id;
        }

        private int AttachAndCompileShader(string shaderText, ShaderType type, int program)
        {
            int id = CreateGlShader(shaderText, type);
            GL.AttachShader(program, id);
            return id;
        }
    }
}

