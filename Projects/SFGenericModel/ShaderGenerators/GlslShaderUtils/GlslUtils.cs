using OpenTK.Graphics.OpenGL;
using SFGenericModel.VertexAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static void AppendVertexInputs(IEnumerable<VertexRenderingAttribute> attributes, StringBuilder shaderSource)
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

        public static void AppendVertexOutputs(IEnumerable<VertexRenderingAttribute> attributes, StringBuilder shaderSource)
        {
            // Ignore duplicates to prevent shader compile errors.
            HashSet<string> previousNames = new HashSet<string>();
            foreach (var attribute in attributes)
            {
                if (previousNames.Contains(attribute.Name))
                    continue;

                string type = GetTypeDeclaration(attribute);
                string interpolation = GetInterpolationQualifier(attribute.Type);
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

        public static void AppendVertexOutputAssignments(IEnumerable<VertexRenderingAttribute> attributes, StringBuilder shaderSource)
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

        private static string GetAttributeRemapOperation(VertexRenderingAttribute attribute)
        {
            if (attribute.RemapToVisibleRange)
                return "* 0.5 + 0.5;";
            else
                return "";
        }

        private static string GetAttributeFunction(VertexRenderingAttribute attribute)
        {
            if (attribute.NormalizeVector)
                return "normalize";
            else
                return "";
        }

        public static void AppendFragmentInputs(IEnumerable<VertexRenderingAttribute> attributes, StringBuilder shaderSource)
        {
            // Ignore duplicates to prevent shader compile errors.
            HashSet<string> previousNames = new HashSet<string>();

            foreach (var attribute in attributes)
            {
                if (previousNames.Contains(attribute.Name))
                    continue;

                string interpolation = GetInterpolationQualifier(attribute.Type);
                string type = GetTypeDeclaration(attribute);
                string variableName = $"{vertexOutputPrefix}{attribute.Name}";

                shaderSource.AppendLine($"{interpolation}in {type} {variableName};");

                previousNames.Add(attribute.Name);
            }
        }

        private static string GetTypeDeclaration(VertexRenderingAttribute attribute)
        {
            if (attribute.ValueCount == ValueCount.One)
                return GetScalarType(attribute);
            else
                return GetVectorType(attribute);
        }

        private static string GetVectorType(VertexRenderingAttribute attribute)
        {
            string typeName = GetVectorTypeName(attribute.Type, attribute.IsInteger);
            return $"{typeName}{(int)attribute.ValueCount}";
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

        private static string GetScalarType(VertexRenderingAttribute attribute)
        {
            switch (attribute.Type)
            {
                case VertexAttribPointerType.Int:
                    return "int";
                case VertexAttribPointerType.UnsignedInt:
                    return "uint";
                case VertexAttribPointerType.Float:
                    return "float";
                default:
                    throw new NotImplementedException($"Type {attribute.Type} is not supported.");
            }
        }

        public static void AppendPositionAssignment(StringBuilder shaderSource, IEnumerable<VertexRenderingAttribute> attributes)
        {
            // Assume the first attribute is position.
            if (!attributes.GetEnumerator().MoveNext())
                return;

            string positionVariable = GlslVectorUtils.ConstructVector(ValueCount.Four, attributes.First().ValueCount, attributes.First().Name);
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
