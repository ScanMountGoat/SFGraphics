using SFGenericModel.VertexAttributes;
using SFGraphics.ShaderGen.GlslShaderUtils;
using System.Collections.Generic;

namespace SFGraphics.ShaderGen
{
    /// <summary>
    /// Contains methods for automatically generating a debug shader for
    /// viewing vertex attributes. 
    /// </summary>
    public class VertexAttributeShaderGenerator
    {
        /// <summary>
        /// The variable name used for the model view projection matrix.
        /// </summary>
        public string MvpMatrixName { get; set; } = "mvpMatrix";

        /// <summary>
        /// The variable name used for transforming normals to view space.
        /// </summary>
        public string SphereMatrixName { get; set; } = "sphereMatrix";

        /// <summary>
        /// The variable name used for selecting the 0 indexed attribute to render.
        /// </summary>
        public string AttribIndexName { get; set; } = "attributeIndex";

        /// <summary>
        /// The minor shading language version. Ex: 4.20 is major version 4.
        /// </summary>
        public int GlslVersionMajor { get; set; } = 3;

        /// <summary>
        /// The minor shading language version. Ex: 4.20 is minor version 2.
        /// </summary>
        public int GlslVersionMinor { get; set; } = 3;

        /// <summary>
        /// Generates a shader for rendering each of the vertex attributes individually. 
        /// </summary>
        /// <typeparam name="T">The vertex struct containing the <see cref="VertexAttribute"/> attributes.</typeparam>
        /// <param name="vertexSource">The generated GLSL vertex shader source</param>
        /// <param name="fragmentSource">The generated GLSL fragment shader source</param>
        public void CreateShader<T>(out string vertexSource, out string fragmentSource) where T : struct
        {
            var attributes = VertexAttributeUtils.GetAttributesFromType<T>();
            CreateShader(attributes, out vertexSource, out fragmentSource);
        }

        /// <summary>
        /// Generates a shader for rendering each of the vertex attributes individually.      
        /// </summary>
        /// <param name="attributes">Attributes used to generate render modes</param>
        /// <param name="vertexSource">The generated GLSL vertex shader source</param>
        /// <param name="fragmentSource">The generated GLSL fragment shader source</param>
        public void CreateShader(List<VertexAttribute> attributes, out string vertexSource, out string fragmentSource)
        {
            // TODO: Use an enum for the uniform type.
            var uniforms = new List<ShaderUniform>
            {
                new ShaderUniform(MvpMatrixName, "mat4")
            };

            vertexSource = GlslUtils.CreateVertexShaderSource(attributes, uniforms, GlslVersionMajor, GlslVersionMinor, MvpMatrixName);
            fragmentSource = GlslUtils.CreateFragmentShaderSource(attributes, uniforms, GlslVersionMajor, GlslVersionMinor, AttribIndexName);
        }
    }
}
