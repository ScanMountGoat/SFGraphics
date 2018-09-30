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
        private static readonly string matrixUniform = $"uniform mat4 {matrixName};";

        public static readonly string outputName = "fragColor";
        private static readonly string fragmentOutput = $"out vec4 {outputName};";

        public static readonly string version = "330";
        private static readonly string versionInfo = $"#version {version}";

        public static readonly string vertexOutputPrefix = "vert_";

        public static Shader CreateShader(string vertexSource, string fragSource)
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
                if (!previousNames.Contains(attribute.Name))
                {
                    string type = GetTypeDeclaration(attribute);
                    shaderSource.AppendLine($"in {type} {attribute.Name};");

                    previousNames.Add(attribute.Name);
                }
            }
        }

        public static void AppendVertexOutputs(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            // Ignore duplicates to prevent shader compile errors.
            HashSet<string> previousNames = new HashSet<string>();
            foreach (var attribute in attributes)
            {
                if (!previousNames.Contains(attribute.Name))
                {
                    string type = GetTypeDeclaration(attribute);
                    string interpolation = GetInterpolationQualifier(attribute.AttributeInfo.Type);
                    shaderSource.AppendLine($"{interpolation}out {type} {vertexOutputPrefix}{attribute.Name};");
                }

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
            string remapOperation = "";
            if (attribute.RemapToVisibleRange)
                remapOperation = "* 0.5 + 0.5;";
            return remapOperation;
        }

        private static string GetAttributeFunction(VertexAttributeRenderInfo attribute)
        {
            string function = "";
            if (attribute.Normalize)
                function = "normalize";
            return function;
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
                string variableName = vertexOutputPrefix + attribute.Name;

                shaderSource.AppendLine($"{interpolation}in {type} {variableName};");

                previousNames.Add(attribute.Name);
            }
        }

        private static string GetTypeDeclaration(VertexAttributeRenderInfo attribute)
        {
            if (attribute.AttributeInfo.ValueCount == ValueCount.One)
            {
                if (attribute.AttributeInfo.Type == VertexAttribPointerType.Float)
                    return "float";
                else if (attribute.AttributeInfo.Type == VertexAttribPointerType.Int)
                    return "int";
                else
                    return "uint";
            }
            else
            {
                return $"vec{(int)attribute.AttributeInfo.ValueCount}";
            }
        }

        public static void AppendPositionAssignment(StringBuilder shaderSource, List<VertexAttributeRenderInfo> attributes)
        {
            // Assume the first attribute is position.
            if (attributes.Count == 0)
                return;

            string positionVariable = GlslVectorUtils.ConstructVector(ValueCount.Four, attributes[0].AttributeInfo.ValueCount, attributes[0].Name);
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
