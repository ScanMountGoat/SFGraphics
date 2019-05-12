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
        /// 
        /// </summary>
        public string MvpMatrixName { get; set; } = "mvpMatrix";

        /// <summary>
        /// 
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
        /// <param name="textures">Textures used to generate render modes</param>
        /// <param name="attributes"></param>
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

            // TODO: Don't assume first attribute is position.
            GetSpecialAttributeNames(attributes, out string uvName, out string normalName);

            if (uvName == "")
                throw new System.ArgumentException("No matching texture coordinates attribute was found.", "attributes");

            if (normalName == "")
                throw new System.ArgumentException("No matching vertex normal attribute was found.", "attributes");

            vertexSource = CreateVertexSource(attributes, normalName);
            fragmentSource = CreateFragmentSource(textures, attributes, uvName, normalName);
        }

        private void GetSpecialAttributeNames(IEnumerable<VertexAttribute> attributes, out string uvName, out string normalName)
        {
            uvName = "";
            normalName = "";

            // TODO: Account for multiple attributes with the same usage.
            foreach (var attribute in attributes)
            {
                if (attribute.AttributeUsage == AttributeUsage.TexCoord0)
                    uvName = attribute.Name;
                if (attribute.AttributeUsage == AttributeUsage.Normal)
                    normalName = attribute.Name;
            }
        }

        private string CreateVertexSource(IEnumerable<VertexAttribute> attributes, string normalName)
        {
            StringBuilder shaderSource = new StringBuilder();
            AppendVertexShader(attributes, normalName, shaderSource);

            return shaderSource.ToString();
        }

        private void AppendVertexShader(IEnumerable<VertexAttribute> attributes, string normalName, StringBuilder shaderSource)
        {
            GlslUtils.AppendShadingLanguageVersion(shaderSource);

            GlslUtils.AppendVertexInputs(attributes, shaderSource);
            GlslUtils.AppendVertexOutputs(attributes, shaderSource);
            AppendViewNormalOutput(shaderSource);

            GlslUtils.AppendMatrixUniforms(shaderSource, MvpMatrixName, SphereMatrixName);

            AppendVertexMainFunction(attributes, normalName, shaderSource);
        }

        private void AppendViewNormalOutput(StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"out vec3 {GlslUtils.vertexOutputPrefix}{viewNormalName};");
        }

        private void AppendViewNormalInput(StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"in vec3 {GlslUtils.vertexOutputPrefix}{viewNormalName};");
        }

        private void AppendVertexMainFunction(IEnumerable<VertexAttribute> attributes, string normalName, StringBuilder shaderSource)
        {
            GlslUtils.AppendBeginMain(shaderSource);

            GlslUtils.AppendVertexOutputAssignments(attributes, shaderSource);

            GlslUtils.AppendPositionAssignment(shaderSource, attributes, MvpMatrixName);

            AppendViewNormalAssignment(normalName, shaderSource);

            GlslUtils.AppendEndMain(shaderSource);
        }

        private void AppendViewNormalAssignment(string normalAttributeName, StringBuilder shaderSource)
        {
            // Transforms the vertex normals by the transposed inverse of the modelview matrix.
            // The result is remapped from [-1, 1] to [0, 1].
            // The effect is similar to a "matcap" material in 3d modeling programs.
            // TODO: Verify that there are enough vector components.
            string normal = $"{normalAttributeName}.xyz";
            string viewNormal = $"{GlslUtils.vertexOutputPrefix}{viewNormalName}.xyz";
            shaderSource.AppendLine($"\t{viewNormal} = mat3({SphereMatrixName}) * {normal};");
            shaderSource.AppendLine($"\t{viewNormal} = {viewNormal} * 0.5 + 0.5;");
        }

        private string CreateFragmentSource(IEnumerable<TextureRenderInfo> textures, 
            IEnumerable<VertexAttribute> attributes, string uv0AttributeName, string normalAttributeName)
        {
            StringBuilder shaderSource = new StringBuilder();
            AppendFragmentShader(textures, attributes, shaderSource, uv0AttributeName, normalAttributeName);

            return shaderSource.ToString();
        }

        private void AppendFragmentShader(IEnumerable<TextureRenderInfo> textures, 
            IEnumerable<VertexAttribute> attributes, StringBuilder shaderSource, 
            string uv0, string normal)
        {
            GlslUtils.AppendShadingLanguageVersion(shaderSource);

            GlslUtils.AppendFragmentInputs(attributes, shaderSource);
            AppendViewNormalInput(shaderSource);

            GlslUtils.AppendFragmentOutput(shaderSource);

            AppendTextureUniforms(textures, shaderSource);
            GlslUtils.AppendMatrixUniforms(shaderSource, MvpMatrixName, SphereMatrixName);

            shaderSource.AppendLine($"uniform int {attribIndexName};");

            AppendFragmentMainFunction(textures, shaderSource, uv0, normal);
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

        private void AppendFragmentMainFunction(IEnumerable<TextureRenderInfo> textures, StringBuilder shaderSource, 
            string uv0, string normal)
        {
            GlslUtils.AppendBeginMain(shaderSource);
            AppendMainFunctionBody(textures, shaderSource, uv0, normal);
            GlslUtils.AppendEndMain(shaderSource);
        }

        private void AppendMainFunctionBody(IEnumerable<TextureRenderInfo> attributes, StringBuilder shaderSource, 
            string uv0, string normal)
        {
            shaderSource.AppendLine($"\tvec3 {resultName} = vec3(0);");

            AppendViewVector(shaderSource);
            AppendReflectionVector(shaderSource, normal);

            AppendFragmentAttributeSwitch(attributes, shaderSource, uv0, normal);

            shaderSource.AppendLine($"\t{GlslUtils.outputName} = vec4({resultName}, 1);");
        }

        private void AppendReflectionVector(StringBuilder shaderSource, string normal)
        {
            shaderSource.AppendLine($"\tvec3 {reflectionVector} = reflect({viewVector}.xyz, {GlslUtils.vertexOutputPrefix}{normal}.xyz);");
            // Correct for a vertically flipped reflection.
            shaderSource.AppendLine($"\t{reflectionVector}.y *= -1;");
        }

        private void AppendViewVector(StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"\tvec3 {viewVector} = vec3(0, 0, -1) * mat3({MvpMatrixName});");
        }

        private void AppendFragmentAttributeSwitch(IEnumerable<TextureRenderInfo> attributes, StringBuilder shaderSource, 
            string uv0, string normalName)
        {
            List<CaseStatement> cases = new List<CaseStatement>();
            int index = 0;
            foreach (var attribute in attributes)
            {
                string caseAssignment = GetResultAssignment(ValueCount.Three, attribute, uv0, normalName);
                cases.Add(new CaseStatement(index.ToString(), caseAssignment));
                index++;
            }

            SwitchUtils.AppendSwitchStatement(shaderSource, attribIndexName, cases);
        }

        private string GetResultAssignment(ValueCount resultCount, TextureRenderInfo texture, 
            string uv0Name, string normalName)
        {
            string swizzle = GlslVectorUtils.GetSwizzle(texture.TextureSwizzle);
            string texCoord = GetTexCoord(texture.UvCoord, uv0Name, normalName);
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
