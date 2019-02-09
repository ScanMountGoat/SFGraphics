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
    public static class TextureShaderGenerator
    {
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
        /// <returns>A new shader that can be used for rendering</returns>
        public static void CreateShader(List<TextureRenderInfo> textures, IEnumerable<VertexRenderingAttribute> attributes, 
            out string vertexSource, out string fragmentSource)
        {
            // TODO: Avoid using LINQ.
            // TODO: Check for empty lists.
            var uvName = attributes.Where((item) => item.AttributeUsage == AttributeUsage.TexCoord0).First().Name;
            var normalName = attributes.Where((item) => item.AttributeUsage == AttributeUsage.Normal).First().Name;

            vertexSource = CreateVertexSource(attributes, normalName);
            fragmentSource = CreateFragmentSource(textures, attributes, uvName, normalName);
        }

        private static string CreateVertexSource(IEnumerable<VertexRenderingAttribute> attributes, string normalName)
        {
            StringBuilder shaderSource = new StringBuilder();
            AppendVertexShader(attributes, normalName, shaderSource);

            return shaderSource.ToString();
        }

        private static void AppendVertexShader(IEnumerable<VertexRenderingAttribute> attributes, string normalName, StringBuilder shaderSource)
        {
            GlslUtils.AppendShadingLanguageVersion(shaderSource);

            GlslUtils.AppendVertexInputs(attributes, shaderSource);
            GlslUtils.AppendVertexOutputs(attributes, shaderSource);
            AppendViewNormalOutput(shaderSource);

            GlslUtils.AppendMatrixUniform(shaderSource);

            AppendVertexMainFunction(attributes, normalName, shaderSource);
        }

        private static void AppendViewNormalOutput(StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"out vec3 {GlslUtils.vertexOutputPrefix}{viewNormalName};");
        }

        private static void AppendViewNormalInput(StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"in vec3 {GlslUtils.vertexOutputPrefix}{viewNormalName};");
        }

        private static void AppendVertexMainFunction(IEnumerable<VertexRenderingAttribute> attributes, string normalName, StringBuilder shaderSource)
        {
            GlslUtils.AppendBeginMain(shaderSource);

            GlslUtils.AppendVertexOutputAssignments(attributes, shaderSource);

            GlslUtils.AppendPositionAssignment(shaderSource, attributes);

            AppendViewNormalAssignment(normalName, shaderSource);

            GlslUtils.AppendEndMain(shaderSource);
        }

        private static void AppendViewNormalAssignment(string normalAttributeName, StringBuilder shaderSource)
        {
            // Transforms the vertex normals by the transposed inverse of the modelview matrix.
            // The result is remapped from [-1, 1] to [0, 1].
            // The effect is similar to a "matcap" material in 3d modeling programs.
            // TODO: Verify that there are enough vector components.
            string normal = $"{normalAttributeName}.xyz";
            string viewNormal = $"{GlslUtils.vertexOutputPrefix}{viewNormalName}.xyz";
            shaderSource.AppendLine($"\t{viewNormal} = mat3({GlslUtils.sphereMatrixName}) * {normal};");
            shaderSource.AppendLine($"\t{viewNormal} = {viewNormal} * 0.5 + 0.5;");
        }

        private static string CreateFragmentSource(IEnumerable<TextureRenderInfo> textures, 
            IEnumerable<VertexRenderingAttribute> attributes, string uv0AttributeName, string normalAttributeName)
        {
            StringBuilder shaderSource = new StringBuilder();
            AppendFragmentShader(textures, attributes, shaderSource, uv0AttributeName, normalAttributeName);

            return shaderSource.ToString();
        }

        private static void AppendFragmentShader(IEnumerable<TextureRenderInfo> textures, 
            IEnumerable<VertexRenderingAttribute> attributes, StringBuilder shaderSource, 
            string uv0, string normal)
        {
            GlslUtils.AppendShadingLanguageVersion(shaderSource);

            GlslUtils.AppendFragmentInputs(attributes, shaderSource);
            AppendViewNormalInput(shaderSource);

            GlslUtils.AppendFragmentOutput(shaderSource);

            AppendTextureUniforms(textures, shaderSource);
            GlslUtils.AppendMatrixUniform(shaderSource);

            shaderSource.AppendLine($"uniform int {attribIndexName};");

            AppendFragmentMainFunction(textures, shaderSource, uv0, normal);
        }

        private static void AppendTextureUniforms(IEnumerable<TextureRenderInfo> textures, StringBuilder shaderSource)
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

        private static void AppendFragmentMainFunction(IEnumerable<TextureRenderInfo> textures, StringBuilder shaderSource, 
            string uv0, string normal)
        {
            GlslUtils.AppendBeginMain(shaderSource);
            AppendMainFunctionBody(textures, shaderSource, uv0, normal);
            GlslUtils.AppendEndMain(shaderSource);
        }

        private static void AppendMainFunctionBody(IEnumerable<TextureRenderInfo> attributes, StringBuilder shaderSource, 
            string uv0, string normal)
        {
            shaderSource.AppendLine($"\tvec3 {resultName} = vec3(0);");

            AppendViewVector(shaderSource);
            AppendReflectionVector(shaderSource, normal);

            AppendFragmentAttributeSwitch(attributes, shaderSource, uv0, normal);

            shaderSource.AppendLine($"\t{GlslUtils.outputName} = vec4({resultName}, 1);");
        }

        private static void AppendReflectionVector(StringBuilder shaderSource, string normal)
        {
            shaderSource.AppendLine($"\tvec3 {reflectionVector} = reflect({viewVector}.xyz, {GlslUtils.vertexOutputPrefix}{normal}.xyz);");
            // Correct for a vertically flipped reflection.
            shaderSource.AppendLine($"\t{reflectionVector}.y *= -1;");
        }

        private static void AppendViewVector(StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"\tvec3 {viewVector} = vec3(0, 0, -1) * mat3({GlslUtils.matrixName});");
        }

        private static void AppendFragmentAttributeSwitch(IEnumerable<TextureRenderInfo> attributes, StringBuilder shaderSource, 
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

        private static string GetResultAssignment(ValueCount resultCount, TextureRenderInfo texture, 
            string uv0Name, string normalName)
        {
            string swizzle = GlslVectorUtils.GetSwizzle(texture.TextureSwizzle);
            string texCoord = GetTexCoord(texture.UvCoord, uv0Name, normalName);
            return $"{resultName}.rgb = texture({texture.Name}, {texCoord}).{swizzle};";
        }

        private static string GetTexCoord(UvCoord uvCoord, string uv0Name, string normalName)
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
