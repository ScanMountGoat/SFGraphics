namespace SFShaderLoader
{
    /// <summary>
    /// Stores the data needed for OpenGL to load a precompiled shader.
    /// </summary>
    public struct BinaryShaderData
    {
        /// <summary>
        /// The platform specific binary format.
        /// </summary>
        public int Format { get; }

        /// <summary>
        /// The compiled shader binary data.
        /// </summary>
        public byte[] ShaderBinary { get; }

        /// <summary>
        /// Creates a new instance of <see cref="BinaryShaderData"/>.
        /// </summary>
        /// <param name="format">the platform specific binary format</param>
        /// <param name="shaderBinary">the compiled shader binary data</param>
        public BinaryShaderData(int format, byte[] shaderBinary) : this()
        {
            Format = format;
            ShaderBinary = shaderBinary;
        }
    }
}
