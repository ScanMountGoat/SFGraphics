using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Linq;

namespace SFShaderLoader
{
    /// <summary>
    /// Loads and stores <see cref="Shader"/> objects from source strings or precompiled binaries.
    /// </summary>
    public class ShaderLoader
    {
        private readonly Dictionary<string, Shader> shaderByName = new Dictionary<string, Shader>();
        private readonly Dictionary<string, ShaderObject> shaderObjectByName = new Dictionary<string, ShaderObject>();

        /// <summary>
        /// Creates and compiles a new <see cref="ShaderObject"/> to be accessed by <paramref name="name"/>
        /// for subsequent calls to <see cref="AddShader(string, string[])"/>.
        /// </summary>
        /// <param name="name">The unique identifier to associate with <paramref name="shaderSource"/></param>
        /// <param name="shaderSource">The shader's GLSL source code</param>
        /// <param name="shaderType">Determines which shader stage this shader will be used for when linking the program</param>
        /// <returns><c>true</c> if the shader was added and compiled successfully</returns>
        public bool AddSource(string name, string shaderSource, ShaderType shaderType)
        {
            var shader = new ShaderObject(shaderSource, shaderType);
            shaderObjectByName[name] = shader;
            return shader.WasCompiledSuccessfully;
        }

        /// <summary>
        /// Creates and links a new <see cref="Shader"/> from <see cref="ShaderObject"/>
        /// based on the keys in <paramref name="sourceNames"/>. Invalid keys are ignored.
        /// </summary>
        /// <param name="name">A unique name for the shader program</param>
        /// <param name="sourceNames">A collection of shader object names/></param>
        /// <returns><c>true</c> if the shader was added and linked successfully</returns>
        /// <exception cref="KeyNotFoundException">A key in <paramref name="sourceNames"/> 
        /// does not refer to an available shader source</exception>
        public bool AddShader(string name, params string[] sourceNames)
        {
            var shader = new Shader();

            var shaderObjects = new ShaderObject[sourceNames.Length];
            for (int i = 0; i < sourceNames.Length; i++)
            {
                // Throw an exception because this is likely a design time typo.
                if (!shaderObjectByName.TryGetValue(sourceNames[i], out shaderObjects[i]))
                    throw new KeyNotFoundException($"Source not found for key {sourceNames[i]}");
            }
            shader.LoadShaders(shaderObjects);

            shaderByName[name] = shader;
            return shader.LinkStatusIsOk;
        }

        /// <summary>
        /// Compiles a <see cref="Shader"/> from the given source files and adds it to the shader list.
        /// Replaces the existing shader for <paramref name="name"/> if found.
        /// </summary>
        /// <param name="name">A unique name for the shader program</param>
        /// <param name="vertexSources">The source code for vertex shaders</param>
        /// <param name="fragmentSources">The source code for fragment shaders</param>
        /// <param name="geometrySources">The source code for geometry shaders</param>
        /// <returns><c>true</c> if the shader was added and linked successfully</returns>
        public bool AddShader(string name, IEnumerable<string> vertexSources, IEnumerable<string> fragmentSources, IEnumerable<string> geometrySources)
        {
            var shaderSources = CreateShaderObjects(vertexSources, fragmentSources, geometrySources);

            var shader = new Shader();
            shader.LoadShaders(shaderSources);

            // Update if already present.
            shaderByName[name] = shader;

            return shader.LinkStatusIsOk;
        }

        /// <summary>
        /// Loads a shader from precompiled shader binaries and adds it to the shader list.
        /// Replaces the existing shader for <paramref name="name"/> if found.
        /// </summary>
        /// <param name="name">A unique shader name</param>
        /// <param name="shaderBinary">The compiled shader binary</param>
        /// <param name="binaryFormat">The platform specific shader binary format</param>
        /// <returns><c>true</c> if the shader was successfully linked and added</returns>
        public bool AddShader(string name, byte[] shaderBinary, BinaryFormat binaryFormat)
        {
            var shader = new Shader();
            shader.TryLoadProgramFromBinary(shaderBinary, binaryFormat);

            // Update if already present.
            shaderByName[name] = shader;

            return shader.LinkStatusIsOk;
        }

        /// <summary>
        /// Attempts to find the shader named <paramref name="shaderName"/>.
        /// </summary>
        /// <param name="shaderName">The unique name used when the shader was added</param>
        /// <returns>the appropriate shader or <c>null</c> if not found</returns>
        public Shader GetShader(string shaderName)
        {
            if (shaderByName.ContainsKey(shaderName))
                return shaderByName[shaderName];
            else
                return null;
        }

        /// <summary>
        /// Creates the compiled program binary for the shader specified by <paramref name="shaderName"/>.
        /// </summary>
        /// <param name="shaderName">The unique name used when the shader was added</param>
        /// <param name="programBinary">The compiled shader program binary</param>
        /// <param name="binaryFormat">The platform specific shader binary format</param>
        /// <returns><c>true</c> if the binary was created successfully</returns>
        /// <exception cref="System.ArgumentException"><paramref name="shaderName"/> is not a valid shader.</exception>
        public bool CreateProgramBinary(string shaderName, out byte[] programBinary, out BinaryFormat binaryFormat)
        {
            var shader = GetShader(shaderName);
            if (shader == null)
                throw new System.ArgumentException("The specified shader could not be found.", nameof(shaderName));

            return shader.GetProgramBinary(out programBinary, out binaryFormat);
        }

        private static ShaderObject[] CreateShaderObjects(IEnumerable<string> vertexSources, 
            IEnumerable<string> fragmentSources, IEnumerable<string> geometrySources)
        {
            var shaderSources = new List<ShaderObject>();

            foreach (var source in vertexSources)
                shaderSources.Add(new ShaderObject(source, ShaderType.VertexShader));

            foreach (var source in fragmentSources)
                shaderSources.Add(new ShaderObject(source, ShaderType.FragmentShader));

            foreach (var source in geometrySources)
                shaderSources.Add(new ShaderObject(source, ShaderType.GeometryShader));

            return shaderSources.ToArray();
        }
    }
}
