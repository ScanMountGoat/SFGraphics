using OpenTK.Graphics.OpenGL;
using SFGenericModel.VertexAttributes;
using System.Collections.Generic;
using System.Text;

namespace SFGenericModel.ShaderGenerators.GlslShaderUtils
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

        public static void AppendVertexInputs(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            foreach (var attribute in attributes)
            {
                string type = GetTypeDeclaration(attribute);
                shaderSource.AppendLine($"in {type} {attribute.attributeInfo.Name};");
            }
        }

        public static void AppendVertexOutputs(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            foreach (var attribute in attributes)
            {
                string type = GetTypeDeclaration(attribute);
                string interpolation = GetInterpolationQualifier(attribute.attributeInfo.Type);
                shaderSource.AppendLine($"{interpolation}out {type} {vertexOutputPrefix}{attribute.attributeInfo.Name};");
            }
        }

        private static string GetInterpolationQualifier(VertexAttribPointerType type)
        {
            if (type == VertexAttribPointerType.Int || type == VertexAttribPointerType.UnsignedInt)
                return "flat ";
            else
                return "";
        }

        public static void AppendVertexOutputAssignments(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            foreach (var attribute in attributes)
            {
                string output = $"{vertexOutputPrefix}{attribute.attributeInfo.Name}";
                string input = $"{ attribute.attributeInfo.Name}";
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
                string interpolation = GetInterpolationQualifier(attribute.attributeInfo.Type);
                string type = GetTypeDeclaration(attribute);
                string variableName = vertexOutputPrefix + attribute.attributeInfo.Name;

                shaderSource.AppendLine($"{interpolation}in {type} {variableName};");
            }
        }

        private static string GetTypeDeclaration(VertexAttributeRenderInfo attribute)
        {
            if (attribute.attributeInfo.ValueCount == ValueCount.One)
            {
                if (attribute.attributeInfo.Type == VertexAttribPointerType.Float)
                    return "float";
                else if (attribute.attributeInfo.Type == VertexAttribPointerType.Int)
                    return "int";
                else
                    return "uint";
            }
            else
            {
                return $"vec{(int)attribute.attributeInfo.ValueCount}";
            }
        }

        public static void AppendPositionAssignment(StringBuilder shaderSource, List<VertexAttributeRenderInfo> attributes)
        {
            // Assume the first attribute is position.
            if (attributes.Count == 0)
                return;

            string positionVariable = GlslVectorUtils.ConstructVector(ValueCount.Four, attributes[0].attributeInfo.ValueCount, attributes[0].attributeInfo.Name);
            shaderSource.AppendLine($"\tgl_Position = {matrixName} * {positionVariable};");
        }

        public static void AppendMatrixUniform(StringBuilder shaderSource)
        {
            shaderSource.AppendLine(matrixUniform);
        }

        public static void AppendFragmentOutput(StringBuilder shaderSource)
        {
            shaderSource.AppendLine(fragmentOutput);
        }

        public static void AppendShadingLanguageVersion(StringBuilder shaderSource)
        {
            shaderSource.AppendLine(versionInfo);
        }
    }
}
