using OpenTK.Graphics.OpenGL;
using SFGenericModel.VertexAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scriban;

namespace SFGraphics.ShaderGen.GlslShaderUtils
{
    internal static class GlslUtils
    {
        public static readonly string outputName = "fragColor";
        private static readonly string fragmentOutput = $"out vec4 {outputName};";

        public static readonly string vertexOutputPrefix = "vs_";

        public static string CreateVertexShaderSource(IEnumerable<VertexAttribute> attributes, int glslVersionMajor, int glslVersionMinor, string mvpMatrixName)
        {
            var template = Template.Parse(@"
#version {{ major_version }}{{ minor_version }}0

{{ vertex_inputs }}
{{ vertex_outputs }}
{{ matrix4_uniforms }}

void main() 
{
{{ vertex_output_assignments }}
{{ position_assignment }}
}
");
            var shaderText = template.Render(new
            {
                MajorVersion = glslVersionMajor,
                MinorVersion = glslVersionMinor,
                VertexInputs = GetVertexInputs(attributes),
                VertexOutputs = GetVertexOutputs(attributes),
                Matrix4Uniforms = GetMatrix4Uniforms(mvpMatrixName),
                VertexOutputAssignments = GetVertexOutputAssignments(attributes),
                PositionAssignment = GetPositionAssignment(attributes, mvpMatrixName)
            });

            return shaderText;
        }

        public static string CreateFragmentShaderSource(IEnumerable<VertexAttribute> attributes, int glslVersionMajor, int glslVersionMinor, string renderModeName)
        {
            var template = Template.Parse(@"
#version {{ major_version }}{{ minor_version }}0
{{ fragment_inputs }}

out vec4 {{ output_name }};

uniform int {{ render_mode_name }};

void main() 
{
    {{ output_name }} = vec4(0.0, 0.0, 0.0, 1.0);
    switch ({{ render_mode_name }})
    {
{{ for val in cases }}
        case {{ val.switch_value }}:
            {{ output_name }}.rgb = {{ val.case_body }};
            break;
{{ end }}
    }
}
");
            var shaderText = template.Render(new
            {
                MajorVersion = glslVersionMajor,
                MinorVersion = glslVersionMinor,
                OutputName = outputName,
                FragmentInputs = GetFragmentInputs(attributes),
                RenderModeName = renderModeName,
                Cases = GetCases(attributes)
            });

            return shaderText;
        }


        private static List<CaseStatement> GetCases(IEnumerable<VertexAttribute> attributes)
        {
            var cases = new List<CaseStatement>();
            var index = 0;
            foreach (var attribute in attributes)
            {
                string body = GlslVectorUtils.ConstructVector(ValueCount.Three, attribute.ValueCount, vertexOutputPrefix + attribute.Name);
                cases.Add(new CaseStatement(index.ToString(), body));
                index++;
            }

            return cases;
        }

        private static string GetVertexInputs(IEnumerable<VertexAttribute> attributes)
        {
            var shaderSource = new StringBuilder();
            var previousNames = new HashSet<string>();
            foreach (var attribute in attributes)
            {
                // Ignore duplicates to prevent shader compile errors.
                if (previousNames.Contains(attribute.Name))
                    continue;

                string type = GetTypeDeclaration(attribute);
                shaderSource.AppendLine($"in {type} {attribute.Name};");

                previousNames.Add(attribute.Name);
            }

            return shaderSource.ToString();
        }

        public static string GetVertexOutputs(IEnumerable<VertexAttribute> attributes)
        {
            var shaderSource = new StringBuilder();

            var previousNames = new HashSet<string>();
            foreach (var attribute in attributes)
            {
                // Ignore duplicates to prevent shader compile errors.
                if (previousNames.Contains(attribute.Name))
                    continue;

                AppendVertexOutput(shaderSource, attribute);

                previousNames.Add(attribute.Name);
            }

            return shaderSource.ToString();
        }

        public static void AppendVertexOutput(StringBuilder shaderSource, VertexAttribute attribute)
        {
            string type = GetTypeDeclaration(attribute);
            string interpolation = GetInterpolationQualifier(attribute.Type);
            shaderSource.AppendLine($"{interpolation}out {type} {vertexOutputPrefix}{attribute.Name};");
        }

        private static string GetInterpolationQualifier(VertexAttribPointerType type)
        {
            if (type == VertexAttribPointerType.Int || type == VertexAttribPointerType.UnsignedInt)
                return "flat ";
            else
                return "";
        }

        private static string GetVertexOutputAssignments(IEnumerable<VertexAttribute> attributes)
        {
            var assignments = new StringBuilder();
            var previousNames = new HashSet<string>();

            foreach (var attribute in attributes)
            {
                // Ignore duplicates to prevent shader compile errors.
                if (previousNames.Contains(attribute.Name))
                    continue;

                string output = $"{vertexOutputPrefix}{attribute.Name}";
                string input = $"{ attribute.Name}";

                string function = GetAttributeFunctionName(attribute);
                string remapOperation = GetAttributeRemapOperation(attribute);

                assignments.AppendLine($"{output} = {function}({input}){remapOperation};");

                previousNames.Add(attribute.Name);
            }

            return assignments.ToString();
        }

        private static string GetAttributeRemapOperation(VertexAttribute attribute)
        {
            if (attribute.RemapToVisibleRange)
                return " * 0.5 + 0.5";
            else
                return "";
        }

        private static string GetAttributeFunctionName(VertexAttribute attribute)
        {
            if (attribute.NormalizeVector)
                return "normalize";
            else
                return "";
        }

        public static string GetFragmentInputs(IEnumerable<VertexAttribute> attributes)
        {
            var shaderSource = new StringBuilder();

            var previousNames = new HashSet<string>();

            foreach (var attribute in attributes)
            {
                // Ignore duplicates to prevent shader compile errors.
                if (previousNames.Contains(attribute.Name))
                    continue;

                string interpolation = GetInterpolationQualifier(attribute.Type);
                string type = GetTypeDeclaration(attribute);
                string variableName = $"{vertexOutputPrefix}{attribute.Name}";

                shaderSource.AppendLine($"{interpolation}in {type} {variableName};");

                previousNames.Add(attribute.Name);
            }

            return shaderSource.ToString();
        }

        private static string GetTypeDeclaration(VertexAttribute attribute)
        {
            if (attribute.ValueCount == ValueCount.One)
                return GetScalarType(attribute);
            else
                return GetVectorType(attribute);
        }

        private static string GetVectorType(VertexAttribute attribute)
        {
            string typeName = GetVectorTypeName(attribute.Type, attribute is VertexIntAttribute);
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

        private static string GetScalarType(VertexAttribute attribute)
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

        public static string GetPositionAssignment(IEnumerable<VertexAttribute> attributes, string matrixName)
        {
            // TODO: Use the first attribute as position?
            var positionAttribute = GetPositionAttribute(attributes);
            if (positionAttribute == null)
                return "";

            string positionVariable = GlslVectorUtils.ConstructVector(ValueCount.Four, positionAttribute.ValueCount, positionAttribute.Name);
            return $"\tgl_Position = {matrixName} * {positionVariable};";
        }

        public static VertexAttribute GetPositionAttribute(IEnumerable<VertexAttribute> attributes)
        {
            return attributes.FirstOrDefault(attribute => attribute.AttributeUsage == AttributeUsage.Position);
        }

        public static string GetMatrix4Uniforms(params string[] matrixNames)
        {
            var shaderSource = new StringBuilder();
            foreach (var name in matrixNames)
            {
                shaderSource.AppendLine($"uniform mat4 {name};");
            }
            return shaderSource.ToString();
        }

        public static void AppendFragmentOutput(StringBuilder shaderSource)
        {
            shaderSource.AppendLine(fragmentOutput);
        }

        public static void AppendShadingLanguageVersion(StringBuilder shaderSource, int majorVersion, int minorVersion)
        {
            shaderSource.AppendLine($"#version {majorVersion}{minorVersion}0");
        }
    }
}
