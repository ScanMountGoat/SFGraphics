using SFGenericModel.ShaderGenerators.GlslShaderUtils;
using SFGenericModel.VertexAttributes;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SFGenericModel.ShaderGenerators
{
    /// <summary>
    /// Contains methods for generating a texture debug shader. 
    /// The swizzling and texture coordinate display is controlled per texture.
    /// </summary>
    public class TextureShaderGenerator
    {
        /// <summary>
        /// The variable name used for the model view projection matrix.
        /// </summary>
        public string MvpMatrixName { get; set; } = "mvpMatrix";

        /// <summary>
        /// The variable name used for transforming normals to view space.
        /// </summary>
        public string SphereMatrixName { get; set; } = "sphereMatrix";

        private static readonly string resultName = "result";

        private static readonly string reflectionVector = "R";
        private static readonly string viewVector = "V";
        private static readonly string viewNormalName = "viewNormal";

        private static readonly string attribIndexName = "textureIndex";

        /// <summary>
        /// Generates a shader for rendering each of the specified textures individually.
        /// </summary>
        /// <typeparam name="T">The vertex struct used for rendering</typeparam>
        /// <param name="textures">Textures used to generate render modes</param>
        /// <param name="vertexSource">The generated GLSL vertex shader source</param>
        /// <param name="fragmentSource">The generated GLSL fragment shader source</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The struct has no attributes.</exception>
        /// <exception cref="System.ArgumentException">The struct's attributes do not contain attributes with the required usages.</exception>
        /// <returns>A new shader that can be used for rendering</returns>
        public void CreateShader<T>(List<TextureRenderInfo> textures, out string vertexSource, out string fragmentSource) where T : struct
        {
            var attributes = VertexAttributeUtils.GetAttributesFromType<T>();
            CreateShader(textures, attributes, out vertexSource, out fragmentSource);
        }

        /// <summary>
        /// Generates a shader for rendering each of the specified textures individually.
        /// </summary>
        /// <param name="textures">Textures used to generate render modes</param>
        /// <param name="attributes">The vertex attributes used for rendering</param>
        /// <param name="vertexSource">The generated GLSL vertex shader source</param>
        /// <param name="fragmentSource">The generated GLSL fragment shader source</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="attributes"/> is empty.</exception>
        /// <exception cref="System.ArgumentException"><paramref name="attributes"/> does not contain attributes with the required usages.</exception>
        /// <returns>A new shader that can be used for rendering</returns>
        public void CreateShader(List<TextureRenderInfo> textures, ICollection<VertexAttribute> attributes, 
            out string vertexSource, out string fragmentSource)
        {
            if (attributes.Count == 0)
                throw new System.ArgumentOutOfRangeException("attributes", "attributes must be non empty to generate a valid shader.");

            GetSpecialAttributes(attributes, out VertexAttribute texcoords, out VertexAttribute normals);

            if (texcoords == null)
                throw new System.ArgumentException("No matching texture coordinates attribute was found.", "attributes");

            if (normals == null)
                throw new System.ArgumentException("No matching vertex normal attribute was found.", "attributes");

            vertexSource = CreateVertexSource(attributes, normals);
            fragmentSource = CreateFragmentSource(textures, attributes, texcoords, normals);
        }

        private void GetSpecialAttributes(IEnumerable<VertexAttribute> attributes, out VertexAttribute texcoords, out VertexAttribute normals)
        {
            texcoords = null;
            normals = null;

            // TODO: Account for multiple attributes with the same usage.
            foreach (var attribute in attributes)
            {
                if (attribute.AttributeUsage == AttributeUsage.TexCoord0)
                    texcoords = attribute;
                else if (attribute.AttributeUsage == AttributeUsage.Normal)
                    normals = attribute;
            }
        }

        private string CreateVertexSource(IEnumerable<VertexAttribute> attributes, VertexAttribute normals)
        {
            StringBuilder shaderSource = new StringBuilder();
            AppendVertexShader(attributes, normals, shaderSource);

            return shaderSource.ToString();
        }

        private void AppendVertexShader(IEnumerable<VertexAttribute> attributes, VertexAttribute normals, StringBuilder shaderSource)
        {
            GlslUtils.AppendShadingLanguageVersion(shaderSource);

            GlslUtils.AppendVertexInputs(attributes, shaderSource);
            GlslUtils.AppendVertexOutputs(attributes, shaderSource);
            AppendViewNormalOutput(shaderSource);

            GlslUtils.AppendMatrix4Uniforms(shaderSource, MvpMatrixName, SphereMatrixName);

            AppendVertexMainFunction(attributes, normals, shaderSource);
        }

        private void AppendViewNormalOutput(StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"out vec3 {GlslUtils.vertexOutputPrefix}{viewNormalName};");
        }

        private void AppendViewNormalInput(StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"in vec3 {GlslUtils.vertexOutputPrefix}{viewNormalName};");
        }

        private void AppendVertexMainFunction(IEnumerable<VertexAttribute> attributes, VertexAttribute normals, StringBuilder shaderSource)
        {
            GlslUtils.AppendBeginMain(shaderSource);

            GlslUtils.AppendVertexOutputAssignments(attributes, shaderSource);
            AppendViewNormalAssignment(normals, shaderSource);
            GlslUtils.AppendPositionAssignment(shaderSource, attributes, MvpMatrixName);

            GlslUtils.AppendEndMain(shaderSource);
        }

        private void AppendViewNormalAssignment(VertexAttribute normals, StringBuilder shaderSource)
        {
            // Transforms the vertex normals to view space.
            // The effect is similar to a "matcap" material in 3d modeling programs.
            string normalVector = GlslVectorUtils.ConstructVector(ValueCount.Three, normals);
            string viewNormal = $"{GlslUtils.vertexOutputPrefix}{viewNormalName}.xyz";
            shaderSource.AppendLine($"\t{viewNormal} = mat3({SphereMatrixName}) * {normalVector};");
            shaderSource.AppendLine($"\t{viewNormal} = {viewNormal} * 0.5 + 0.5;");
        }

        private string CreateFragmentSource(IEnumerable<TextureRenderInfo> textures, 
            IEnumerable<VertexAttribute> attributes, VertexAttribute texcoords, VertexAttribute normals)
        {
            StringBuilder shaderSource = new StringBuilder();
            AppendFragmentShader(textures, attributes, shaderSource, texcoords, normals);

            return shaderSource.ToString();
        }

        private void AppendFragmentShader(IEnumerable<TextureRenderInfo> textures, IEnumerable<VertexAttribute> attributes, StringBuilder shaderSource, 
            VertexAttribute texcoords, VertexAttribute normal)
        {
            GlslUtils.AppendShadingLanguageVersion(shaderSource);

            GlslUtils.AppendFragmentInputs(attributes, shaderSource);
            AppendViewNormalInput(shaderSource);

            GlslUtils.AppendFragmentOutput(shaderSource);

            AppendTextureUniforms(textures, shaderSource);
            GlslUtils.AppendMatrix4Uniforms(shaderSource, MvpMatrixName, SphereMatrixName);

            shaderSource.AppendLine($"uniform int {attribIndexName};");

            AppendFragmentMainFunction(textures, shaderSource, texcoords, normal);
        }

        private void AppendTextureUniforms(IEnumerable<TextureRenderInfo> textures, StringBuilder shaderSource)
        {
            HashSet<string> previousNames = new HashSet<string>();
            foreach (var texture in textures)
            {
                if (!previousNames.Contains(texture.Name))
                {
                    if (texture.UvCoord == UvCoord.CubeMap)
                        shaderSource.AppendLine($"uniform samplerCube {texture.Name};");
                    else
                        shaderSource.AppendLine($"uniform sampler2D {texture.Name};");

                    previousNames.Add(texture.Name);
                }
            }
        }

        private void AppendFragmentMainFunction(IEnumerable<TextureRenderInfo> textures, StringBuilder shaderSource, VertexAttribute uv0, VertexAttribute normal)
        {
            GlslUtils.AppendBeginMain(shaderSource);
            AppendMainFunctionBody(textures, shaderSource, uv0, normal);
            GlslUtils.AppendEndMain(shaderSource);
        }

        private void AppendMainFunctionBody(IEnumerable<TextureRenderInfo> attributes, StringBuilder shaderSource, 
            VertexAttribute texcoords, VertexAttribute normals)
        {
            shaderSource.AppendLine($"\tvec3 {resultName} = vec3(0);");

            AppendViewVector(shaderSource);
            AppendReflectionVector(shaderSource, normals);

            AppendFragmentAttributeSwitch(attributes, shaderSource, texcoords, normals);

            shaderSource.AppendLine($"\t{GlslUtils.outputName} = vec4({resultName}, 1);");
        }

        private void AppendReflectionVector(StringBuilder shaderSource, VertexAttribute normal)
        {
            // The normals attribute is a vertex shader output, so the prefix needs to be added.
            string normalVector = GlslVectorUtils.ConstructVector(ValueCount.Three, normal.ValueCount, GlslUtils.vertexOutputPrefix + normal.Name);
            shaderSource.AppendLine($"\tvec3 {reflectionVector} = reflect({viewVector}.xyz, {normalVector});");

            // Correct for a vertically flipped reflection.
            shaderSource.AppendLine($"\t{reflectionVector}.y *= -1;");
        }

        private void AppendViewVector(StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"\tvec3 {viewVector} = vec3(0, 0, -1) * mat3({MvpMatrixName});");
        }

        private void AppendFragmentAttributeSwitch(IEnumerable<TextureRenderInfo> attributes, StringBuilder shaderSource, 
            VertexAttribute texcoords, VertexAttribute normals)
        {
            var cases = GetCases(attributes, texcoords, normals);
            SwitchUtils.AppendSwitchStatement(shaderSource, attribIndexName, cases);
        }

        private List<CaseStatement> GetCases(IEnumerable<TextureRenderInfo> attributes, VertexAttribute texcoords, VertexAttribute normals)
        {
            List<CaseStatement> cases = new List<CaseStatement>();
            int index = 0;
            foreach (var attribute in attributes)
            {
                string caseAssignment = GetResultAssignment(ValueCount.Three, attribute, texcoords, normals);
                cases.Add(new CaseStatement(index.ToString(), caseAssignment));
                index++;
            }

            return cases;
        }

        private string GetResultAssignment(ValueCount resultCount, TextureRenderInfo texture, 
            VertexAttribute texcoords, VertexAttribute normals)
        {
            string swizzle = GlslVectorUtils.GetSwizzle(texture.TextureSwizzle);
            string texCoord = GetTexCoord(texture.UvCoord, texcoords.Name, normals.Name);
            return $"{resultName}.rgb = texture({texture.Name}, {texCoord}).{swizzle};";
        }

        private string GetTexCoord(UvCoord uvCoord, string uv0Name, string normalName)
        {
            switch (uvCoord)
            {
                case UvCoord.TexCoord0:
                    return $"{GlslUtils.vertexOutputPrefix}{uv0Name}.xy";
                case UvCoord.CamEnvSphere:
                    return $"{GlslUtils.vertexOutputPrefix}{viewNormalName}.xy";
                case UvCoord.CubeMap:
                    return $"{reflectionVector}.xyz";
                default:
                    return "vec2(0.5)";
            }
        }
    }
}
