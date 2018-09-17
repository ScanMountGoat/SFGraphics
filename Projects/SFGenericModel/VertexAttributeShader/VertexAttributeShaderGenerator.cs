using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFGraphics.GLObjects.Shaders;
using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.VertexAttributeShader
{
    /// <summary>
    /// 
    /// </summary>
    public static class VertexAttributeShaderGenerator
    {
        private static readonly string resultName = "result";
        private static readonly string attribIndexName = "attributeIndex";
        private static readonly string outputName = "fragColor";
        private static readonly string version = "330";
        private static readonly string vertexOutputPrefix = "vert_";


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Shader CreateShader(List<VertexAttributeRenderInfo> attributes)
        {
            Shader shader = new Shader();

            string vertexSource = CreateVertexSource(attributes);
            shader.LoadShader(vertexSource, ShaderType.VertexShader);

            string fragSource = CreateFragmentSource(attributes);
            shader.LoadShader(fragSource, ShaderType.FragmentShader);

            return shader;
        }

        private static string CreateVertexSource(List<VertexAttributeRenderInfo> attributes)
        {
            StringBuilder shaderSource = new StringBuilder();
            shaderSource.AppendLine($"#version {version}");

            AppendVertexInputs(attributes, shaderSource);
            AppendVertexOutputs(attributes, shaderSource);

            shaderSource.AppendLine("void main()");
            shaderSource.AppendLine("{");

            AppendVertexOutputAssignments(attributes, shaderSource);
            // TODO: What to do about this line?
            shaderSource.AppendLine("\tgl_Position = vec4(position, 1);");

            shaderSource.AppendLine("}");

            System.Diagnostics.Debug.WriteLine(shaderSource.ToString());

            return shaderSource.ToString();
        }

        private static string CreateFragmentSource(List<VertexAttributeRenderInfo> attributes)
        {
            StringBuilder shaderSource = new StringBuilder();
            shaderSource.AppendLine($"#version {version}");

            AppendFragmentInputs(attributes, shaderSource);
            shaderSource.AppendLine($"out vec4 {outputName};");

            shaderSource.AppendLine($"uniform int {attribIndexName};");

            shaderSource.AppendLine("void main()");
            shaderSource.AppendLine("{");
            shaderSource.AppendLine($"\tvec3 {resultName} = vec3(0);");

            AppendFragmentAttributeSwitch(attributes, shaderSource);

            shaderSource.AppendLine($"\t{outputName} = vec4({resultName}, 1);");

            shaderSource.AppendLine("}");

            System.Diagnostics.Debug.WriteLine(shaderSource.ToString());

            return shaderSource.ToString();
        }

        private static void AppendVertexInputs(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            foreach (var attribute in attributes)
            {
                // TODO: Account for floats and ints.
                string type = $"vec{(int)attribute.attributeInfo.valueCount}";
                shaderSource.AppendLine($"in {type} {attribute.attributeInfo.name};");
            }
        }

        private static void AppendVertexOutputs(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            foreach (var attribute in attributes)
            {
                // TODO: Account for floats and ints.
                string type = $"vec{(int)attribute.attributeInfo.valueCount}";
                shaderSource.AppendLine($"out {type} {vertexOutputPrefix}{attribute.attributeInfo.name};");
            }
        }

        private static void AppendVertexOutputAssignments(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            foreach (var attribute in attributes)
            {
                string output = $"{vertexOutputPrefix}{attribute.attributeInfo.name}";
                string input = $"{ attribute.attributeInfo.name}";
                string function = "";
                if (attribute.normalize)
                    function = "normalize";

                string remapOperation = "";
                if (attribute.remapToVisibleRange)
                    remapOperation = "* 0.5 + 0.5;";

                shaderSource.AppendLine($"\t{output} = {function}({input}) {remapOperation};");
            }
        }

        private static void AppendFragmentInputs(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            foreach (var attribute in attributes)
            {
                // TODO: Account for floats and ints.
                string type = $"vec{(int)attribute.attributeInfo.valueCount}";
                shaderSource.AppendLine($"in {type} {vertexOutputPrefix}{attribute.attributeInfo.name};");
            }
        }

        private static void AppendFragmentAttributeSwitch(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"\tswitch ({attribIndexName})");
            shaderSource.AppendLine("\t{");

            int index = 0;
            foreach (var attribute in attributes)
            {
                shaderSource.AppendLine($"\t\tcase {index}:");
                shaderSource.AppendLine($"\t\t\t{resultName}.rgb = {vertexOutputPrefix}{attribute.attributeInfo.name}.xyz;");
                shaderSource.AppendLine($"\t\t\tbreak;");

                index++;
            }

            shaderSource.AppendLine("\t}");
        }
    }
}
