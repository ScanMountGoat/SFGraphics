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
    public class VertexAttributeShaderGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        public string MvpMatrixName { get; set; } = "mvpMatrix";

        /// <summary>
        /// 
        /// </summary>
        public string SphereMatrixName { get; set; } = "sphereMatrix";

        /// <summary>
        /// 
        /// </summary>
        public string ResultName { get; set; } = "result";

        /// <summary>
        /// 
        /// </summary>
        public string AttribIndexName { get; set; } = "attributeIndex";

        /// <summary>
        /// Generates a shader for rendering each of the vertex attributes individually. 
        /// </summary>
        /// <typeparam name="T">The vertex struct containing the <see cref="VertexAttribute"/> attributes.</typeparam>
        /// <param name="vertexSource">The generated GLSL vertex shader source</param>
        /// <param name="fragmentSource">The generated GLSL fragment shader source</param>
        public void CreateShader<T>(out string vertexSource, out string fragmentSource) where T : struct
        {
            var attributes = VertexAttributeUtils.GetAttributesFromType<T>();
            vertexSource = CreateVertexSource(attributes);
            fragmentSource = CreateFragmentSource(attributes);
        }

        /// <summary>
        /// Generates a shader for rendering each of the vertex attributes individually.      
        /// </summary>
        /// <param name="attributes">Attributes used to generate render modes. 
        /// <param name="vertexSource">The generated GLSL vertex shader source</param>
        /// <param name="fragmentSource">The generated GLSL fragment shader source</param>
        public void CreateShader(List<VertexAttribute> attributes, out string vertexSource, out string fragmentSource)
        {
            vertexSource = CreateVertexSource(attributes);
            fragmentSource = CreateFragmentSource(attributes);
        }

        private string CreateVertexSource(List<VertexAttribute> attributes)
        {
            StringBuilder shaderSource = new StringBuilder();
            AppendVertexShader(attributes, shaderSource);

            return shaderSource.ToString();
        }

        private void AppendVertexShader(List<VertexAttribute> attributes, StringBuilder shaderSource)
        {
            GlslUtils.AppendShadingLanguageVersion(shaderSource);

            GlslUtils.AppendVertexInputs(attributes, shaderSource);
            GlslUtils.AppendVertexOutputs(attributes, shaderSource);
            GlslUtils.AppendMatrix4Uniforms(shaderSource, MvpMatrixName, SphereMatrixName);

            AppendVertexMainFunction(attributes, shaderSource);
        }

        private void AppendVertexMainFunction(List<VertexAttribute> attributes, StringBuilder shaderSource)
        {
            GlslUtils.AppendBeginMain(shaderSource);

            GlslUtils.AppendVertexOutputAssignments(attributes, shaderSource);
            GlslUtils.AppendPositionAssignment(shaderSource, attributes, MvpMatrixName);

            GlslUtils.AppendEndMain(shaderSource);
        }

        private string CreateFragmentSource(List<VertexAttribute> attributes)
        {
            StringBuilder shaderSource = new StringBuilder();
            AppendFragmentShader(attributes, shaderSource);

            return shaderSource.ToString();
        }

        private void AppendFragmentShader(List<VertexAttribute> attributes, StringBuilder shaderSource)
        {
            GlslUtils.AppendShadingLanguageVersion(shaderSource);

            GlslUtils.AppendFragmentInputs(attributes, shaderSource);
            GlslUtils.AppendFragmentOutput(shaderSource);

            AppendAttributeIndexUniform(shaderSource);

            AppendFragmentMainFunction(attributes, shaderSource);
        }

        private void AppendAttributeIndexUniform(StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"uniform int {AttribIndexName};");
        }

        private void AppendFragmentMainFunction(List<VertexAttribute> attributes, StringBuilder shaderSource)
        {
            GlslUtils.AppendBeginMain(shaderSource);

            AppendMainFunctionBody(attributes, shaderSource);

            GlslUtils.AppendEndMain(shaderSource);
        }

        private void AppendMainFunctionBody(List<VertexAttribute> attributes, StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"\tvec3 {ResultName} = vec3(0);");

            AppendFragmentAttributeSwitch(attributes, shaderSource);

            shaderSource.AppendLine($"\t{GlslUtils.outputName} = vec4({ResultName}, 1);");
        }
         
        private void AppendFragmentAttributeSwitch(List<VertexAttribute> attributes, StringBuilder shaderSource)
        {
            var cases = GetCases(attributes);
            SwitchUtils.AppendSwitchStatement(shaderSource, AttribIndexName, cases);
        }

        private List<CaseStatement> GetCases(List<VertexAttribute> attributes)
        {
            List<CaseStatement> cases = new List<CaseStatement>();
            for (int i = 0; i < attributes.Count; i++)
            {
                string body = GetResultAssignment(ValueCount.Three, attributes[i].Name, attributes[i].ValueCount);
                cases.Add(new CaseStatement(i.ToString(), body));
            }

            return cases;
        }

        private string GetResultAssignment(ValueCount resultCount, string sourceName, ValueCount sourceCount)
        {
            string constructedVector = GlslVectorUtils.ConstructVector(resultCount, sourceCount,
                GlslUtils.vertexOutputPrefix + sourceName);
            return $"{ResultName}.rgb = {constructedVector};";
        }
    }
}
