using SFGenericModel.ShaderGenerators.GlslShaderUtils;
using SFGenericModel.VertexAttributes;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using System.Text;

namespace SFGenericModel.ShaderGenerators
{
    /// <summary>
    /// Contains methods for automatically generating a debug shader for
    /// viewing vertex attributes. 
    /// </summary>
    public static class VertexAttributeShaderGenerator
    {
        private static readonly string resultName = "result";
        private static readonly string attribIndexName = "attributeIndex";

        /// <summary>
        /// Generates a shader for rendering each of 
        /// the vertex attributes individually.      
        /// </summary>
        /// <param name="attributes">Attributes used to generate render modes. 
        /// The first attribute is also used as the position.</param>
        /// <returns>A new shader that can be used for rendering</returns>
        public static Shader CreateShader(List<VertexAttributeRenderInfo> attributes)
        {
            string vertexSource = CreateVertexSource(attributes);
            string fragSource = CreateFragmentSource(attributes);

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
            GlslUtils.AppendMatrixUniform(shaderSource);

            AppendVertexMainFunction(attributes, shaderSource);
        }

        private static void AppendVertexMainFunction(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            GlslUtils.AppendBeginMain(shaderSource);

            GlslUtils.AppendVertexOutputAssignments(attributes, shaderSource);
            GlslUtils.AppendPositionAssignment(shaderSource, attributes);

            GlslUtils.AppendEndMain(shaderSource);
        }

        private static string CreateFragmentSource(List<VertexAttributeRenderInfo> attributes)
        {
            StringBuilder shaderSource = new StringBuilder();
            AppendFragmentShader(attributes, shaderSource);

            return shaderSource.ToString();
        }

        private static void AppendFragmentShader(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            GlslUtils.AppendShadingLanguageVersion(shaderSource);

            GlslUtils.AppendFragmentInputs(attributes, shaderSource);
            GlslUtils.AppendFragmentOutput(shaderSource);

            shaderSource.AppendLine($"uniform int {attribIndexName};");

            AppendFragmentMainFunction(attributes, shaderSource);
        }

        private static void AppendFragmentMainFunction(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            GlslUtils.AppendBeginMain(shaderSource);
            AppendMainFunctionBody(attributes, shaderSource);
            GlslUtils.AppendEndMain(shaderSource);
        }

        private static void AppendMainFunctionBody(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"\tvec3 {resultName} = vec3(0);");

            AppendFragmentAttributeSwitch(attributes, shaderSource);

            shaderSource.AppendLine($"\t{GlslUtils.outputName} = vec4({resultName}, 1);");
        }
         
        private static void AppendFragmentAttributeSwitch(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            List<CaseStatement> cases = new List<CaseStatement>();
            for (int i = 0; i < attributes.Count; i++)
            {
                string caseAssignment = GetResultAssignment(ValueCount.Three, 
                    attributes[i].AttributeInfo.Name, attributes[i].AttributeInfo.ValueCount);
                cases.Add(new CaseStatement(i.ToString(), caseAssignment));
            }

            SwitchUtils.AppendSwitchStatement(shaderSource, attribIndexName, cases);
        }

        private static string GetResultAssignment(ValueCount resultCount, string sourceName, ValueCount sourceCount)
        {
            string constructedVector = GlslVectorUtils.ConstructVector(resultCount, sourceCount,
                GlslUtils.vertexOutputPrefix + sourceName);
            return $"{resultName}.rgb = {constructedVector};";
        }
    }
}
