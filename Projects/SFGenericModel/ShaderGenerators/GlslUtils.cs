using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using System.Text;
using System;
using SFGenericModel.VertexAttributes;

namespace SFGenericModel.ShaderGenerators
{
    internal static class GlslUtils
    {
        public static readonly string matrixName = "mvpMatrix";
        private static readonly string matrixUniform = $"uniform mat4 {matrixName};";

        public static readonly string outputName = "fragColor";
        private static readonly string fragmentOutput = $"out vec4 {outputName};";

        public static readonly string version = "330";
        private static readonly string versionInfo = $"#version {version}";

        public static readonly string vertexOutputPrefix = "vert_";

        public static readonly string[] vectorComponents = new string[] { "x", "y", "z", "w" };

        public static void AppendVertexInputs(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            foreach (var attribute in attributes)
            {
                string type = GetTypeDeclaration(attribute);
                shaderSource.AppendLine($"in {type} {attribute.attributeInfo.name};");
            }
        }

        public static void AppendVertexOutputs(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            foreach (var attribute in attributes)
            {
                string type = GetTypeDeclaration(attribute);
                string prefix = "";
                if (type == "uint" || type == "int")
                    prefix = $"flat ";
                shaderSource.AppendLine($"{prefix}out {type} {vertexOutputPrefix}{attribute.attributeInfo.name};");
            }
        }

        public static void AppendVertexOutputAssignments(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
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

        public static void AppendFragmentInputs(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            foreach (var attribute in attributes)
            {
                string type = GetTypeDeclaration(attribute);
                string prefix = "";
                if (type == "uint" || type == "int")
                    prefix = $"flat ";
                shaderSource.AppendLine($"{prefix}in {type} {vertexOutputPrefix}{attribute.attributeInfo.name};");
            }
        }

        private static string GetTypeDeclaration(VertexAttributeRenderInfo attribute)
        {
            if (attribute.attributeInfo.valueCount == ValueCount.One)
            {
                if (attribute.attributeInfo.type == VertexAttribPointerType.Float)
                    return "float";
                else if (attribute.attributeInfo.type == VertexAttribPointerType.Int)
                    return "int";
                else
                    return "uint";
            }
            else
            {
                return $"vec{(int)attribute.attributeInfo.valueCount}";
            }
        }

        public static void AppendPositionAssignment(StringBuilder shaderSource, List<VertexAttributeRenderInfo> attributes)
        {
            // Assume the first attribute is position.
            if (attributes.Count == 0)
                return;

            shaderSource.AppendLine($"\tgl_Position = {matrixName} * {ConstructVector(ValueCount.Four, attributes[0].attributeInfo.valueCount, attributes[0].attributeInfo.name)};");
        }

        public static void AppendMatrixUniform(StringBuilder shaderSource)
        {
            shaderSource.AppendLine(matrixUniform);
        }

        public static void AppendFragmentOutput(StringBuilder shaderSource)
        {
            shaderSource.AppendLine(fragmentOutput);
        }

        public static string ConstructVector(ValueCount targetCount, ValueCount sourceCount, string sourceName)
        {
            if (sourceCount == ValueCount.One)
                return $"vec{(int)targetCount}({sourceName})";

            string components = GetMaxSharedComponents(sourceCount, targetCount);

            // Add 1's for the remaining parts of the constructor.
            string paddingValues = "";
            for (int i = components.Length; i < (int)targetCount; i++)
            {
                paddingValues += ", 1";
            }

            return $"vec{(int)targetCount}({sourceName}.{components}{paddingValues})";
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

        public static void AppendShadingLanguageVersion(StringBuilder shaderSource)
        {
            shaderSource.AppendLine(versionInfo);
        }
    }
}
