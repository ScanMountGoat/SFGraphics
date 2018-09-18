﻿using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using System.Text;
using System;
using SFGenericModel.VertexAttributes;

namespace SFGenericModel.VertexAttributeShader
{
    /// <summary>
    /// Contains methods for automatically generating a debug shader for
    /// viewing vertex attributes. 
    /// </summary>
    public static class VertexAttributeShaderGenerator
    {
        private static readonly string resultName = "result";
        private static readonly string matrixName = "mvpMatrix";
        private static readonly string attribIndexName = "attributeIndex";
        private static readonly string outputName = "fragColor";
        private static readonly string version = "330";
        private static readonly string vertexOutputPrefix = "vert_";

        private static readonly string[] vectorComponents = new string[] { "x", "y", "z", "w" };

        /// <summary>
        /// Generates a shader for rendering each of 
        /// the vertex attributes individually.      
        /// </summary>
        /// <param name="attributes">Attributes used to generate render modes. 
        /// The first attribute is also used as the position.</param>
        /// <returns>A new shader that can be used for rendering</returns>
        public static Shader CreateShader(List<VertexAttributeRenderInfo> attributes)
        {
            Shader shader = new Shader();

            string vertexSource = CreateVertexSource(attributes);
            shader.LoadShader(vertexSource, ShaderType.VertexShader);

            string fragSource = CreateFragmentSource(attributes);
            shader.LoadShader(fragSource, ShaderType.FragmentShader);

            System.Diagnostics.Debug.WriteLine(shader.GetErrorLog());

            return shader;
        }

        private static string CreateVertexSource(List<VertexAttributeRenderInfo> attributes)
        {
            StringBuilder shaderSource = new StringBuilder();
            shaderSource.AppendLine($"#version {version}");

            AppendVertexInputs(attributes, shaderSource);
            AppendVertexOutputs(attributes, shaderSource);
            shaderSource.AppendLine($"uniform mat4 {matrixName};");

            shaderSource.AppendLine("void main()");
            shaderSource.AppendLine("{");

            AppendVertexOutputAssignments(attributes, shaderSource);
            AppendPositionAssignment(shaderSource, attributes);

            shaderSource.AppendLine("}");

            System.Diagnostics.Debug.WriteLine(shaderSource.ToString());

            return shaderSource.ToString();
        }

        private static void AppendPositionAssignment(StringBuilder shaderSource, List<VertexAttributeRenderInfo> attributes)
        {
            // Assume the first attribute is position.
            if (attributes.Count == 0)
                return;

            shaderSource.AppendLine($"\tgl_Position = {matrixName} * {ConstructVector(ValueCount.Four, attributes[0].attributeInfo.valueCount, attributes[0].attributeInfo.name)};");
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

                string constructedVector = ConstructVector(ValueCount.Three, attribute.attributeInfo.valueCount, vertexOutputPrefix + attribute.attributeInfo.name);
                shaderSource.AppendLine($"\t\t\t{resultName}.rgb = {constructedVector};");

                shaderSource.AppendLine($"\t\t\tbreak;");

                index++;
            }

            shaderSource.AppendLine("\t}");
        }

        private static string ConstructVector(ValueCount targetCount, ValueCount sourceCount, string sourceName)
        {
            string components = GetMaxSharedComponents(sourceCount, targetCount);

            // Add 1's for the remaining parts of the constructor.
            string paddingValues = "";
            for (int i = components.Length; i < (int)targetCount; i++)
            {
                paddingValues += ", 1";
            }

            string result = $"vec{(int)targetCount}({sourceName}.{components}{paddingValues})";
            return result;
        }

        private static string GetMaxSharedComponents(ValueCount sourceCount, ValueCount targetCount)
        {
            string resultingComponents = "";

            int count = Math.Min((int)sourceCount, (int)targetCount);
            for (int i = 0; i < count; i++)
            {
                resultingComponents += vectorComponents[i];
            }

            return resultingComponents;
        }
    }
}
