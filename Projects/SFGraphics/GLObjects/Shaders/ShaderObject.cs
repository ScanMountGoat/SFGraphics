using OpenTK.Graphics.OpenGL;

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
        /// Creates and compiles a shader object from <paramref name="shaderSource"/>.
        /// </summary>
        /// <param name="shaderSource">The shader's source code</param>
        /// <param name="shaderType">determines which shader stage this shader will be used for when linking the program.</param>
        public ShaderObject(string shaderSource, ShaderType shaderType) : base(GL.CreateShader(shaderType))
        {
            GL.ShaderSource(Id, shaderSource);
            GL.CompileShader(Id);
        }
    }
}
