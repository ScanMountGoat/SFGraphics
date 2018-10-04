using OpenTK.Graphics.OpenGL;
using SFGenericModel.VertexAttributes;
using SFGraphics.GLObjects.Shaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFGenericModel.ShaderGenerators.GlslShaderUtils
{
    internal static class GlslUtils
    {
        public static readonly string matrixName = "mvpMatrix";
        public static readonly string sphereMatrixName = "sphereMatrix";

        private static readonly string matrixUniform = $"uniform mat4 {matrixName};";
        private static readonly string sphereMatrixUniform = $"uniform mat4 {sphereMatrixName};";

        public static readonly string outputName = "fragColor";
        private static readonly string fragmentOutput = $"out vec4 {outputName};";

        public static readonly string version = "330";
        private static readonly string versionInfo = $"#version {version}";

        public static readonly string vertexOutputPrefix = "vert_";

        public static void AppendEndMain(StringBuilder shaderSource)
        {
            shaderSource.AppendLine("}");
        }

        public static void AppendBeginMain(StringBuilder shaderSource)
        {
            shaderSource.AppendLine("void main()");
            shaderSource.AppendLine("{");
        }

        public static void AppendVertexInputs(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            // Ignore duplicates to prevent shader compile errors.
            HashSet<string> previousNames = new HashSet<string>();
            foreach (var attribute in attributes)
            {
                if (previousNames.Contains(attribute.Name))
                    continue;

                string type = GetTypeDeclaration(attribute);
                shaderSource.AppendLine($"in {type} {attribute.Name};");

                previousNames.Add(attribute.Name);
            }
        }

        public static void AppendVertexOutputs(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            // Ignore duplicates to prevent shader compile errors.
            HashSet<string> previousNames = new HashSet<string>();
            foreach (var attribute in attributes)
            {
                if (previousNames.Contains(attribute.Name))
                    continue;

                string type = GetTypeDeclaration(attribute);
                string interpolation = GetInterpolationQualifier(attribute.AttributeInfo.Type);
                shaderSource.AppendLine($"{interpolation}out {type} {vertexOutputPrefix}{attribute.Name};");

                previousNames.Add(attribute.Name);
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
            // Ignore duplicates to prevent shader compile errors.
            HashSet<string> previousNames = new HashSet<string>();

            foreach (var attribute in attributes)
            {
                if (previousNames.Contains(attribute.Name))
                    continue;

                string output = $"{vertexOutputPrefix}{attribute.Name}";
                string input = $"{ attribute.Name}";

                string function = GetAttributeFunction(attribute);
                string remapOperation = GetAttributeRemapOperation(attribute);

                shaderSource.AppendLine($"\t{output} = {function}({input}) {remapOperation};");

                previousNames.Add(attribute.Name);
            }
        }

        private static string GetAttributeRemapOperation(VertexAttributeRenderInfo attribute)
        {
            if (attribute.RemapToVisibleRange)
                return "* 0.5 + 0.5;";
            else
                return "";
        }

        private static string GetAttributeFunction(VertexAttributeRenderInfo attribute)
        {
            if (attribute.Normalize)
                return "normalize";
            else
                return "";
        }

        public static void AppendFragmentInputs(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            // Ignore duplicates to prevent shader compile errors.
            HashSet<string> previousNames = new HashSet<string>();

            foreach (var attribute in attributes)
            {
                if (previousNames.Contains(attribute.Name))
                    continue;

                string interpolation = GetInterpolationQualifier(attribute.AttributeInfo.Type);
                string type = GetTypeDeclaration(attribute);
                string variableName = $"{vertexOutputPrefix}{attribute.Name}";

                shaderSource.AppendLine($"{interpolation}in {type} {variableName};");

                previousNames.Add(attribute.Name);
            }
        }

        private static string GetTypeDeclaration(VertexAttributeRenderInfo attribute)
        {
            if (attribute.AttributeInfo.ValueCount == ValueCount.One)
                return GetScalarType(attribute);
            else
                return GetVectorType(attribute);
        }

        private static string GetVectorType(VertexAttributeRenderInfo attributeInfo)
        {
            var attribute = attributeInfo.AttributeInfo;

            string typeName = GetVectorTypeName(attribute.Type, attribute is VertexAttributeIntInfo);
            return $"{typeName}{(int)attributeInfo.AttributeInfo.ValueCount}";
        }

        private static string GetVectorTypeName(VertexAttribPointerType type, bool isInteger)
        {
            if (isInteger)
            {
                if (type == VertexAttribPointerType.Int)
                    return "ivec";
                else if (type == VertexAttribPointerType.UnsignedInt)
                    return "uvec";
                else
                    throw new NotImplementedException($"Type {type} is not supported.");
            }
            else
            {
                if (type == VertexAttribPointerType.Float)
                    return "vec";
                else
                    throw new NotImplementedException($"Type {type} is not supported.");
            }
        }

        private static string GetScalarType(VertexAttributeRenderInfo attribute)
        {
            switch (attribute.AttributeInfo.Type)
            {
                case VertexAttribPointerType.Int:
                    return "int";
                case VertexAttribPointerType.UnsignedInt:
                    return "uint";
                case VertexAttribPointerType.Float:
                    return "float";
                default:
                    throw new NotImplementedException($"Type {attribute.AttributeInfo.Type} is not supported.");
            }
        }

        public static void AppendPositionAssignment(StringBuilder shaderSource, List<VertexAttributeRenderInfo> attributes)
        {
            if (attributes.Count == 0)
                return;

            // Assume the first attribute is position.
            string positionVariable = GlslVectorUtils.ConstructVector(ValueCount.Four, attributes[0].AttributeInfo.ValueCount, attributes[0].Name);
            shaderSource.AppendLine($"\tgl_Position = {matrixName} * {positionVariable};");
        }

        public static void AppendMatrixUniform(StringBuilder shaderSource)
        {
            shaderSource.AppendLine(matrixUniform);
            // Unused uniforms will be optimized out by the compiler.
            shaderSource.AppendLine(sphereMatrixUniform);
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
