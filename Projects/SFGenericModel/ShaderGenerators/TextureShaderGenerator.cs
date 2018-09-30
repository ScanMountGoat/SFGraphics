using SFGenericModel.ShaderGenerators.GlslShaderUtils;
using SFGenericModel.VertexAttributes;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using System.Text;

namespace SFGenericModel.ShaderGenerators
{
    /// <summary>
    /// Contains methods for generating a texture debug shader.
    /// </summary>
    public static class TextureShaderGenerator
    {
        private static readonly string resultName = "result";
        private static readonly string reflectionVector = "R";
        private static readonly string viewVector = "V";
        private static readonly string viewNormalName = "viewNormal";
        private static readonly string attribIndexName = "textureIndex";

        /// <summary>
        /// Generates a shader for rendering each of 
        /// the vertex attributes individually.      
        /// </summary>
        /// <param name="textures">Textures used to generate render modes</param>
        /// <param name="position"></param>
        /// <param name="normal">The normals used for sphere map rendering</param>
        /// <param name="uv0"></param>
        /// <returns>A new shader that can be used for rendering</returns>
        public static Shader CreateShader(List<TextureRenderInfo> textures, 
            VertexAttribute position, VertexAttribute normal, VertexAttribute uv0)
        {
            var attributes = new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(position),
                new VertexAttributeRenderInfo(normal),
                new VertexAttributeRenderInfo(uv0)
            };

            string vertexSource = CreateVertexSource(attributes);
            string fragSource = CreateFragmentSource(textures, attributes, uv0.Name, normal.Name);
            System.Diagnostics.Debug.WriteLine(vertexSource);
            System.Diagnostics.Debug.WriteLine(fragSource);

            return GlslUtils.CreateShader(vertexSource, fragSource);
        }

        private static string CreateVertexSource(List<VertexAttributeRenderInfo> attributes)
        {
            StringBuilder shaderSource = new StringBuilder();
            AppendVertexShader(attributes, shaderSource);

            return shaderSource.ToString();
        }

        private static void AppendVertexShader(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            GlslUtils.AppendShadingLanguageVersion(shaderSource);

            GlslUtils.AppendVertexInputs(attributes, shaderSource);
            GlslUtils.AppendVertexOutputs(attributes, shaderSource);
            AppendViewNormalOutput(shaderSource);

            GlslUtils.AppendMatrixUniform(shaderSource);

            AppendVertexMainFunction(attributes, shaderSource);
        }

        private static void AppendViewNormalOutput(StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"out vec3 {GlslUtils.vertexOutputPrefix}{viewNormalName};");
        }

        private static void AppendViewNormalInput(StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"in vec3 {GlslUtils.vertexOutputPrefix}{viewNormalName};");
        }

        private static void AppendVertexMainFunction(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            GlslUtils.AppendBeginMain(shaderSource);

            GlslUtils.AppendVertexOutputAssignments(attributes, shaderSource);
            GlslUtils.AppendPositionAssignment(shaderSource, attributes);
            AppendViewNormalAssignment(attributes, shaderSource);

            GlslUtils.AppendEndMain(shaderSource);
        }

        private static void AppendViewNormalAssignment(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            string normal = $"{attributes[1].Name}.xyz";
            string viewNormal = $"{GlslUtils.vertexOutputPrefix}{viewNormalName}.xyz";
            shaderSource.AppendLine($"\t{viewNormal} = mat3({GlslUtils.sphereMatrixName}) * {normal};");
            shaderSource.AppendLine($"\t{viewNormal} = {viewNormal} * 0.5 + 0.5;");
        }

        private static string CreateFragmentSource(List<TextureRenderInfo> textures, 
            List<VertexAttributeRenderInfo> attributes, string uv0, string normal)
        {
            StringBuilder shaderSource = new StringBuilder();
            AppendFragmentShader(textures, attributes, shaderSource, uv0, normal);

            return shaderSource.ToString();
        }

        private static void AppendFragmentShader(List<TextureRenderInfo> textures, 
            List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource, 
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

        private static void AppendTextureUniforms(List<TextureRenderInfo> textures, StringBuilder shaderSource)
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

        private static void AppendFragmentMainFunction(List<TextureRenderInfo> textures, StringBuilder shaderSource, 
            string uv0, string normal)
        {
            GlslUtils.AppendBeginMain(shaderSource);
            AppendMainFunctionBody(textures, shaderSource, uv0, normal);
            GlslUtils.AppendEndMain(shaderSource);
        }

        private static void AppendMainFunctionBody(List<TextureRenderInfo> attributes, StringBuilder shaderSource, 
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
        }

        private static void AppendViewVector(StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"\tvec3 {viewVector} = vec3(0, 0, -1) * mat3({GlslUtils.matrixName});");
        }

        private static void AppendFragmentAttributeSwitch(List<TextureRenderInfo> attributes, StringBuilder shaderSource, 
            string uv0, string normalName)
        {
            List<CaseStatement> cases = new List<CaseStatement>();
            for (int i = 0; i < attributes.Count; i++)
            {
                string caseAssignment = GetResultAssignment(ValueCount.Three, attributes[i], uv0, normalName);
                cases.Add(new CaseStatement(i.ToString(), caseAssignment));
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
