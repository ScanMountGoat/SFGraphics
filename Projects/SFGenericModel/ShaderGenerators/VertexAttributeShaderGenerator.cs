using OpenTK.Graphics.OpenGL;
using SFGenericModel.ShaderGenerators.GlslShaderUtils;
using SFGenericModel.VertexAttributes;
using SFGraphics.GLObjects.Shaders;
using System;
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

            return CreateShader(vertexSource, fragSource);
        }

        private static Shader CreateShader(string vertexSource, string fragSource)
        {
            Shader shader = new Shader();
            var shaders = new List<Tuple<string, ShaderType, string>>()
            {
                new Tuple<string, ShaderType, string>(vertexSource, ShaderType.VertexShader, ""),
                new Tuple<string, ShaderType, string>(fragSource, ShaderType.FragmentShader, "")
            };
            shader.LoadShaders(shaders);
            return shader;
        }

        private static string CreateVertexSource(List<VertexAttributeRenderInfo> attributes)
        {
            StringBuilder shaderSource = new StringBuilder();
            GlslUtils.AppendShadingLanguageVersion(shaderSource);

            GlslUtils.AppendVertexInputs(attributes, shaderSource);
            GlslUtils.AppendVertexOutputs(attributes, shaderSource);
            GlslUtils.AppendMatrixUniform(shaderSource);

            shaderSource.AppendLine("void main()");
            shaderSource.AppendLine("{");

            GlslUtils.AppendVertexOutputAssignments(attributes, shaderSource);
            GlslUtils.AppendPositionAssignment(shaderSource, attributes);

            shaderSource.AppendLine("}");

            return shaderSource.ToString();
        }

        private static string CreateFragmentSource(List<VertexAttributeRenderInfo> attributes)
        {
            StringBuilder shaderSource = new StringBuilder();
            GlslUtils.AppendShadingLanguageVersion(shaderSource);

            GlslUtils.AppendFragmentInputs(attributes, shaderSource);
            GlslUtils.AppendFragmentOutput(shaderSource);

            shaderSource.AppendLine($"uniform int {attribIndexName};");

            shaderSource.AppendLine("void main()");
            shaderSource.AppendLine("{");

            AppendMainFunctionBody(attributes, shaderSource);

            shaderSource.AppendLine("}");

            return shaderSource.ToString();
        }

        private static void AppendMainFunctionBody(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"\tvec3 {resultName} = vec3(0);");

            AppendFragmentAttributeSwitch(attributes, shaderSource);

            shaderSource.AppendLine($"\t{GlslUtils.outputName} = vec4({resultName}, 1);");
        }
         
        private static void AppendFragmentAttributeSwitch(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"\tswitch ({attribIndexName})");
            shaderSource.AppendLine("\t{");

            for (int i = 0; i < attributes.Count; i++)
            {
                string caseAssignment = GetResultAssignment(attributes[i]);
                GlslSwitchUtils.AppendSwitchCaseStatement(shaderSource, i, caseAssignment);
            }

            shaderSource.AppendLine("\t}");
        }

        private static string GetResultAssignment(VertexAttributeRenderInfo attribute)
        {
            string constructedVector = GlslVectorUtils.ConstructVector(ValueCount.Three, attribute.attributeInfo.ValueCount,
                GlslUtils.vertexOutputPrefix + attribute.attributeInfo.Name);
            return $"{resultName}.rgb = {constructedVector};";
        }
    }
}
