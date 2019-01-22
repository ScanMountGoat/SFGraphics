using SFGenericModel.ShaderGenerators.GlslShaderUtils;
using SFGenericModel.VertexAttributes;
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
        /// Generates a shader for rendering each of the vertex attributes individually.      
        /// </summary>
        /// <param name="attributes">Attributes used to generate render modes. 
        /// The first attribute is also used as the position.</param>
        /// <param name="vertexSource">The generated GLSL vertex shader source</param>
        /// <param name="fragmentSource">The generated GLSL fragment shader source</param>
        public static void CreateShader(List<VertexRenderingAttribute> attributes, 
            out string vertexSource, out string fragmentSource)
        {
            vertexSource = CreateVertexSource(attributes);
            fragmentSource = CreateFragmentSource(attributes);
        }

        /// <summary>
        /// Generates a shader for rendering each of the vertex attributes individually. 
        /// The first attribute is also used as the position.
        /// </summary>
        /// <typeparam name="T">The vertex struct containing the <see cref="VertexRenderingAttribute"/> attributes.</typeparam>
        /// <param name="vertexSource">The generated GLSL vertex shader source</param>
        /// <param name="fragmentSource">The generated GLSL fragment shader source</param>
        public static void CreateShader<T>(out string vertexSource, out string fragmentSource) where T : struct
        {
            List<VertexRenderingAttribute> attributes = GetRenderingAttributes<T>();
            vertexSource = CreateVertexSource(attributes);
            fragmentSource = CreateFragmentSource(attributes);
        }

        private static List<VertexRenderingAttribute> GetRenderingAttributes<T>() where T : struct
        {
            var attributes = new List<VertexRenderingAttribute>();
            foreach (var member in typeof(T).GetMembers())
            {
                foreach (VertexRenderingAttribute attribute in member.GetCustomAttributes(typeof(VertexRenderingAttribute), true))
                {
                    // Break to ignore duplicate attributes.
                    attributes.Add(attribute);
                    break;
                }
            }

            return attributes;
        }

        private static string CreateVertexSource(List<VertexRenderingAttribute> attributes)
        {
            StringBuilder shaderSource = new StringBuilder();
            AppendVertexShader(attributes, shaderSource);

            return shaderSource.ToString();
        }

        private static void AppendVertexShader(List<VertexRenderingAttribute> attributes, StringBuilder shaderSource)
        {
            GlslUtils.AppendShadingLanguageVersion(shaderSource);

            GlslUtils.AppendVertexInputs(attributes, shaderSource);
            GlslUtils.AppendVertexOutputs(attributes, shaderSource);
            GlslUtils.AppendMatrixUniform(shaderSource);

            AppendVertexMainFunction(attributes, shaderSource);
        }

        private static void AppendVertexMainFunction(List<VertexRenderingAttribute> attributes, StringBuilder shaderSource)
        {
            GlslUtils.AppendBeginMain(shaderSource);

            GlslUtils.AppendVertexOutputAssignments(attributes, shaderSource);
            GlslUtils.AppendPositionAssignment(shaderSource, attributes);

            GlslUtils.AppendEndMain(shaderSource);
        }

        private static string CreateFragmentSource(List<VertexRenderingAttribute> attributes)
        {
            StringBuilder shaderSource = new StringBuilder();
            AppendFragmentShader(attributes, shaderSource);

            return shaderSource.ToString();
        }

        private static void AppendFragmentShader(List<VertexRenderingAttribute> attributes, StringBuilder shaderSource)
        {
            GlslUtils.AppendShadingLanguageVersion(shaderSource);

            GlslUtils.AppendFragmentInputs(attributes, shaderSource);
            GlslUtils.AppendFragmentOutput(shaderSource);

            shaderSource.AppendLine($"uniform int {attribIndexName};");

            AppendFragmentMainFunction(attributes, shaderSource);
        }

        private static void AppendFragmentMainFunction(List<VertexRenderingAttribute> attributes, StringBuilder shaderSource)
        {
            GlslUtils.AppendBeginMain(shaderSource);
            AppendMainFunctionBody(attributes, shaderSource);
            GlslUtils.AppendEndMain(shaderSource);
        }

        private static void AppendMainFunctionBody(List<VertexRenderingAttribute> attributes, StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"\tvec3 {resultName} = vec3(0);");

            AppendFragmentAttributeSwitch(attributes, shaderSource);

            shaderSource.AppendLine($"\t{GlslUtils.outputName} = vec4({resultName}, 1);");
        }
         
        private static void AppendFragmentAttributeSwitch(List<VertexRenderingAttribute> attributes, StringBuilder shaderSource)
        {
            List<CaseStatement> cases = new List<CaseStatement>();
            for (int i = 0; i < attributes.Count; i++)
            {
                string caseAssignment = GetResultAssignment(ValueCount.Three, 
                    attributes[i].Name, attributes[i].ValueCount);
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
