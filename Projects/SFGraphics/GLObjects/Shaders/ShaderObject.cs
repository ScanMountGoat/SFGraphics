using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders.Utils;

namespace SFGraphics.GLObjects.Shaders
{
    /// <summary>
    /// Encapsulates a single shader object, 
    /// which are attached to instances of <see cref="Shader"/> to create shader programs.
    /// </summary>
    public class ShaderObject : GLObject
    {
        internal override GLObjectType ObjectType => GLObjectType.Shader;

        /// <summary>
        /// Determines which shader stage this shader will be used for when linking the program.
        /// </summary>
        public ShaderType ShaderType { get; }

        /// <summary>
        /// <c>true</c> if the shader compiled successfully when created.
        /// </summary>
        public bool WasCompiledSuccessfully { get; }

        /// <summary>
        /// Creates and compiles a shader object from <paramref name="shaderSource"/>.
        /// </summary>
        /// <param name="shaderSource">The shader's source code</param>
        /// <param name="shaderType">determines which shader stage this shader will be used for when linking the program.</param>
        public ShaderObject(string shaderSource, ShaderType shaderType) : base(GL.CreateShader(shaderType))
        {
            if (!string.IsNullOrEmpty(shaderSource))
            {
                GL.ShaderSource(Id, shaderSource);
                GL.CompileShader(Id);
            }

            WasCompiledSuccessfully = ShaderValidation.GetShaderObjectCompileStatus(Id);
        }

        /// <summary>
        /// Gets the source code from OpenGL associated with this shader.
        /// </summary>
        /// <returns>The shader source</returns>
        public string GetShaderSource()
        {
            GL.GetShader(Id, ShaderParameter.ShaderSourceLength, out int length);
            string source = "";
            if (length != 0)
                GL.GetShaderSource(Id, length, out _, out source);
            return source;
        }

        /// <summary>
        /// Gets the info log containing compilation errors and other information.
        /// </summary>
        /// <returns>The shader info log</returns>
        public string GetInfoLog()
        {
            return GL.GetShaderInfoLog(Id);
        }
    }
}
