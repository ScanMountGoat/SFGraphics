using OpenTK.Graphics.OpenGL;
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

        public static string CreateVertexShaderSource(IEnumerable<ShaderAttribute> attributes, IEnumerable<ShaderUniform> uniforms, int glslVersionMajor, int glslVersionMinor, string mvpMatrixName)
        {
            var template = Template.Parse(@"
#version {{ major_version }}{{ minor_version }}0

{{~ for attribute in attributes ~}}
in {{ attribute.type_declaration }} {{ attribute.name }};
{{~ end ~}}

{{~ for attribute in attributes ~}}
{{ attribute.interpolation }} out {{ attribute.type_declaration }} {{ vertex_output_prefix }}{{ attribute.name }};
{{~ end ~}}

{{~ for uniform in uniforms ~}}
uniform {{ uniform.type }} {{ uniform.name }};
{{~ end ~}}

void main() 
{

{{~ for attribute in attributes ~}}
    {{ vertex_output_prefix }}{{ attribute.name }} = {{ attribute.name }};
{{~ end ~}}

    {{ position_assignment }}
}
");
            var shaderText = template.Render(new
            {
                MajorVersion = glslVersionMajor,
                MinorVersion = glslVersionMinor,
                Attributes = attributes,
                Uniforms = uniforms,
                VertexOutputPrefix = vertexOutputPrefix,
                PositionAssignment = GetPositionAssignment(attributes, mvpMatrixName)
            });

            return shaderText;
        }

        public static string CreateFragmentShaderSource(IEnumerable<ShaderAttribute> attributes, IEnumerable<ShaderUniform> uniforms, int glslVersionMajor, int glslVersionMinor, string renderModeName)
        {
            var template = Template.Parse(@"
#version {{ major_version }}{{ minor_version }}0

{{~ for attribute in attributes ~}}
{{ attribute.interpolation }} in {{ attribute.type_declaration }} {{ vertex_output_prefix }}{{ attribute.name }};
{{~ end ~}}

out vec4 {{ output_name }};

{{~ for uniform in uniforms ~}}
uniform {{ uniform.type }} {{ uniform.name }};
{{~ end ~}}

uniform int {{ render_mode_name }};

void main() 
{
    {{ output_name }} = vec4(0.0, 0.0, 0.0, 1.0);
    switch ({{ render_mode_name }})
    {
{{~ for item in cases ~}}
        case {{ item.switch_value }}:
            {{ output_name }}.rgb = {{ item.case_body }};
            break;
{{~ end ~}}
    }
}
");
            var shaderText = template.Render(new
            {
                MajorVersion = glslVersionMajor,
                MinorVersion = glslVersionMinor,
                OutputName = outputName,
                Attributes = attributes,
                RenderModeName = renderModeName,
                VertexOutputPrefix = vertexOutputPrefix,
                Uniforms = uniforms,
                Cases = GetCases(attributes)
            });

            return shaderText;
        }

        private static List<CaseStatement> GetCases(IEnumerable<ShaderAttribute> attributes)
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

        public static string GetPositionAssignment(IEnumerable<ShaderAttribute> attributes, string matrixName)
        {
            // TODO: Use the first attribute as position?
            var positionAttribute = attributes.FirstOrDefault();
            if (positionAttribute == null)
                return "";

            string positionVariable = GlslVectorUtils.ConstructVector(ValueCount.Four, positionAttribute.ValueCount, positionAttribute.Name);
            return $"gl_Position = {matrixName} * {positionVariable};";
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
