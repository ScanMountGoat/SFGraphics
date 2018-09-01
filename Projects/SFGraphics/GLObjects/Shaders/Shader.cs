using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Shaders
{
    /// <summary>
    /// Encapsulates a shader program and attached shaders. 
    /// Errors are stored to an internal log, which can be exported with <see cref="GetErrorLog"/>.
    /// <para></para> <para></para>
    /// Ensure that <see cref="ProgramCreatedSuccessfully"/> returns <c>true</c> before rendering to avoid crashes.
    /// </summary>
    public sealed partial class Shader : GLObject
    {
        /// <summary>
        /// Returns the type of OpenGL object. Used for memory management.
        /// </summary>
        public override GLObjectType ObjectType { get { return GLObjectType.ShaderProgram; } }

        /// <summary>
        /// <c>true</c> when the link status is ok.
        /// If <c>false</c>, rendering with this shader will most likely cause an <see cref="AccessViolationException"/>.
        /// <para></para><para></para>
        /// The status is updated with each call to <see cref="LoadShader(string, ShaderType, string)"/>, 
        /// <see cref="AttachShader(int, ShaderType, string)"/>, or <see cref="LoadProgramBinary(byte[], BinaryFormat)"/>.
        /// </summary>
        public bool ProgramCreatedSuccessfully { get; private set; }

        /// <summary>
        /// <c>true</c> when only one sampler type is used for each texture unit
        /// and the number of active samplers does not exceed the allowed maximum.
        /// <para></para><para></para>
        /// This should be checked at runtime and only for debugging purposes.
        /// </summary>
        public bool ProgramStatusIsValid {  get { return GetProgramValidationStatus(); } }

        private int activeUniformCount;
        private int activeAttributeCount;

        private ShaderLog errorLog = new ShaderLog();

        private Dictionary<string, ActiveUniformInfo> activeUniformByName = new Dictionary<string, ActiveUniformInfo>();
        private Dictionary<string, ActiveAttribInfo> activeAttribByName = new Dictionary<string, ActiveAttribInfo>();

        private Dictionary<string, ActiveUniformInfo> invalidUniformByName = new Dictionary<string, ActiveUniformInfo>();
        private Dictionary<string, ActiveAttribInfo> invalidAttribSetByName = new Dictionary<string, ActiveAttribInfo>();

        // This isn't in OpenTK's enums for some reason.
        // https://www.khronos.org/registry/OpenGL/api/GL/glcorearb.h
        private static readonly int GL_PROGRAM_BINARY_MAX_LENGTH = 0x8741;

        /// <summary>
        /// Creates an unitialized shader program. Load shaders before using the shader program.
        /// </summary>
        public Shader() : base(GL.CreateProgram())
        {

        }

        /// <summary>
        /// Use <see cref="GLObject.Id"/> as the current program.
        /// </summary>
        public void UseProgram()
        {
            GL.UseProgram(Id);
        }

        /// <summary>
        /// Gets the index of the vertex attribute or uniform variable. Returns -1 if not found.
        /// </summary>
        /// <param name="name">The name of the vertex attribute or uniform</param>
        /// <returns>The index of the attribute/uniform or -1 if not found </returns>
        public int GetVertexAttributeUniformLocation(string name)
        {
            // TODO: Remove this method and add separate methods.
            if (activeAttribByName.ContainsKey(name))
                return activeAttribByName[name].location;
            else if (activeUniformByName.ContainsKey(name))
                return activeUniformByName[name].location;
            else
                return -1;
        }

        /// <summary>
        /// Enables the vertex attribute arrays for all active attributes
        /// for the currently bound vertex array object.
        /// Ensures that extra attributes aren't enabled, which causes crashes on Geforce drivers.
        /// </summary>
        public void EnableVertexAttributes()
        {
            // Only enable the necessary vertex attributes.
            // Enabling extra vertex attributes crashes on Nvidia.
            for (int location = 0; location < activeAttributeCount; location++)
            {
                GL.EnableVertexAttribArray(location);
            }
        }

        /// <summary>
        /// Disables the vertex attribute arrays for all active attributes 
        /// for the currently bound vertex array object.
        /// Ensures that extra attributes aren't enabled, which causes crashes on Geforce drivers.
        /// </summary>
        public void DisableVertexAttributes()
        {
            // Disable all the vertex attributes. This could be used with a VAO in the future.
            for (int location = 0; location < activeAttributeCount; location++)
            {
                GL.DisableVertexAttribArray(location);
            }
        }

        /// <summary>
        /// Attaches <paramref name="shaderId"/> and links the program. 
        /// The value for <see cref="ProgramCreatedSuccessfully"/> is updated.
        /// </summary>
        /// <param name="shaderId">The integer ID returned by <see cref="CreateGlShader(string, ShaderType)"/></param>
        /// <param name="shaderType">The type of shader.
        /// Ex: ShaderType.FragmentShader</param>        
        /// <param name="shaderName"></param>
        public void AttachShader(int shaderId, ShaderType shaderType, string shaderName = "")
        {
            GL.AttachShader(Id, shaderId);
            GL.LinkProgram(Id);

            AppendShaderCompilationErrors(shaderName, shaderType, shaderId);
            ProgramCreatedSuccessfully = CheckProgramStatus();

            // The shader won't be deleted until the program is deleted.
            GL.DeleteShader(shaderId);

            // Scary things happen if we do this after a linking error.
            if (ProgramCreatedSuccessfully)
            {
                LoadAttributes();
                LoadUniforms();
            }
        }

        /// <summary>
        /// Attempts to compile and attach the shader. 
        /// The value returned by <see cref="ProgramCreatedSuccessfully"/> is updated.
        /// Supported shader types are fragment, vertex , and geometry.
        /// </summary>
        /// <param name="shaderSource">A string containing the shader source text</param>
        /// <param name="shaderType">The type of shader to load. Ex: ShaderType.FragmentShader</param>
        /// <param name="shaderName">The title used for the compilation errors section of the error log</param>
        public void LoadShader(string shaderSource, ShaderType shaderType, string shaderName = "Shader")
        {
            // Compile and attach before linking.
            int shaderId = CreateGlShader(shaderSource, shaderType);
            AttachShader(shaderId, shaderType, shaderName);
        }

        /// <summary>
        /// Returns the integer ID created by GL.CreateShader(). Compiles the shader.
        /// This method can reduce load times by avoiding redundant shader compilations when used
        /// in conjunction with <see cref="AttachShader(int, ShaderType, string)"/>
        /// </summary>
        /// <param name="shaderSource">A string containing the shader source text</param>
        /// <param name="shaderType">The type of shader.
        /// Ex: ShaderType.FragmentShader</param>
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
            int bufSize;
            GL.GetProgram(Id, (GetProgramParameterName)GL_PROGRAM_BINARY_MAX_LENGTH, out bufSize);
            byte[] programBinary = new byte[bufSize];

            int length; 
            GL.GetProgramBinary(Id, bufSize, out length, out binaryFormat, programBinary);
            return programBinary;
        }

        /// <summary>
        /// Loads the entire program from the compiled binary and format generated 
        /// by <see cref="GetProgramBinary(out BinaryFormat)"/>.
        /// The value returned by <see cref="ProgramCreatedSuccessfully"/> is updated.
        /// <para></para><para></para>
        /// Hardware or software changes may cause compatibility issues with the program binary.
        /// If program creation fails with precompiled binaries, resort to compiling the shaders from source. 
        /// </summary>
        /// <param name="binaryFormat">The format of the compiled binary</param>
        /// <param name="programBinary">The compiled program binary</param>
        public void LoadProgramBinary(byte[] programBinary, BinaryFormat binaryFormat)
        {
            GL.ProgramBinary(Id, binaryFormat, programBinary, programBinary.Length);

            ProgramCreatedSuccessfully = CheckProgramStatus();

            // Scary things happen if we do this after a linking error.
            if (ProgramCreatedSuccessfully)
            {
                LoadAttributes();
                LoadUniforms();
            }
        }

        private void AddVertexAttribute(string name, ActiveAttribType type)
        {
            int location = GL.GetAttribLocation(Id, name);
            
            // Overwrite existing vertex attributes.
            if (!activeAttribByName.ContainsKey(name))
                activeAttribByName.Add(name, new ActiveAttribInfo(location, type));
        }

        private void AddUniform(string name, ActiveUniformType type, int size)
        {
            string nameNoArrayIndex = name;
            if (name.Contains("["))
                nameNoArrayIndex = name.Substring(0, name.IndexOf('['));

            string nameArrayIndex0 = nameNoArrayIndex + "[0]";

            // Uniform arrays can be "array[0]" or "array"
            int location = GL.GetUniformLocation(Id, nameNoArrayIndex);
            if (location == -1)
                location = GL.GetUniformLocation(Id, nameArrayIndex0);

            // Overwrite existing uniforms.
            if (!activeUniformByName.ContainsKey(nameNoArrayIndex))
                activeUniformByName.Add(nameNoArrayIndex, new ActiveUniformInfo(location, type, size));
        }

        private void LoadUniforms()
        {
            GL.GetProgram(Id, GetProgramParameterName.ActiveUniforms, out activeUniformCount);

            for (int i = 0; i < activeUniformCount; i++)
            {
                // Ignore invalid uniforms. 0 is "None" for type.
                ActiveUniformType uniformType;
                int uniformSize;
                string uniform = GL.GetActiveUniform(Id, i, out uniformSize, out uniformType);
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
                // Ignore invalid attributes.
                ActiveAttribType attributeType;
                int attributeSize;
                string attribute = GL.GetActiveAttrib(Id, i, out attributeSize, out attributeType);
                if (attributeType != ActiveAttribType.None)
                {
                    AddVertexAttribute(attribute, attributeType);
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

