using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace SFShaderLoader
{
    /// <summary>
    /// Loads and stores <see cref="Shader"/> objects from source strings or precompiled binaries.
    /// </summary>
    public class ShaderLoader
    {
        private readonly Dictionary<string, Shader> shaderByName = new Dictionary<string, Shader>();

        /// <summary>
        /// Compiles a shader from the given source files and adds it to the shader list.
        /// Replaces the existing shader for <paramref name="name"/> if found.
        /// </summary>
        /// <param name="name">A unique shader name</param>
        /// <param name="vertexSources">The source code for vertex shaders</param>
        /// <param name="fragmentSources">The source code for fragment shaders</param>
        /// <param name="geometrySources">The source code for geometry shaders</param>
        /// <returns><c>true</c> if the shader was successfully linked</returns>
        public bool AddShader(string name, IEnumerable<string> vertexSources, IEnumerable<string> fragmentSources, IEnumerable<string> geometrySources)
        {
            var shaderSources = GetShaderSources(vertexSources, fragmentSources, geometrySources);

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
            shader.LoadProgramBinary(shaderBinary, binaryFormat);

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

        private static List<System.Tuple<string, ShaderType, string>> GetShaderSources(IEnumerable<string> vertexSources, 
            IEnumerable<string> fragmentSources, IEnumerable<string> geometrySources)
        {
            var shaderSources = new List<System.Tuple<string, ShaderType, string>>();

            foreach (var source in vertexSources)
                shaderSources.Add(new System.Tuple<string, ShaderType, string>(source, ShaderType.VertexShader, ""));

            foreach (var source in fragmentSources)
                shaderSources.Add(new System.Tuple<string, ShaderType, string>(source, ShaderType.FragmentShader, ""));

            foreach (var source in geometrySources)
                shaderSources.Add(new System.Tuple<string, ShaderType, string>(source, ShaderType.GeometryShader, ""));

            return shaderSources;
        }
    }
}
