﻿using SFGenericModel.ShaderGenerators.GlslShaderUtils;
using SFGenericModel.VertexAttributes;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using System.Text;

namespace SFGenericModel.ShaderGenerators
{
    /// <summary>
    /// Contains methods for generating a texture debug shader.
    /// </summary>
    public static class TextureShaderGenerator
    {
        private static readonly string resultName = "result";
        private static readonly string attribIndexName = "attributeIndex";

        /// <summary>
        /// Generates a shader for rendering each of 
        /// the vertex attributes individually.      
        /// </summary>
        /// <param name="textures">Textures used to generate render modes</param>
        /// <param name="position"></param>
        /// <param name="uv0"></param>
        /// <returns>A new shader that can be used for rendering</returns>
        public static Shader CreateShader(List<TextureRenderInfo> textures, 
            VertexAttribute position, VertexAttribute uv0)
        {
            var attributes = new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(false, false, position),
                new VertexAttributeRenderInfo(false, false, uv0)
            };

            string vertexSource = CreateVertexSource(attributes);
            string fragSource = CreateFragmentSource(textures, attributes);

            System.Diagnostics.Debug.WriteLine(vertexSource);
            System.Diagnostics.Debug.WriteLine(fragSource);

            return GlslUtils.CreateShader(vertexSource, fragSource);
        }

        private static string CreateVertexSource(List<VertexAttributeRenderInfo> attributes)
        {
            StringBuilder shaderSource = new StringBuilder();
            AppendVertexShader(attributes, shaderSource);

            return shaderSource.ToString();
        }

        private static void AppendVertexShader(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            GlslUtils.AppendShadingLanguageVersion(shaderSource);

            GlslUtils.AppendVertexInputs(attributes, shaderSource);
            GlslUtils.AppendVertexOutputs(attributes, shaderSource);
            GlslUtils.AppendMatrixUniform(shaderSource);

            AppendVertexMainFunction(attributes, shaderSource);
        }

        private static void AppendVertexMainFunction(List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            GlslUtils.AppendBeginMain(shaderSource);

            GlslUtils.AppendVertexOutputAssignments(attributes, shaderSource);
            GlslUtils.AppendPositionAssignment(shaderSource, attributes);

            GlslUtils.AppendEndMain(shaderSource);
        }

        private static string CreateFragmentSource(List<TextureRenderInfo> textures, List<VertexAttributeRenderInfo> attributes)
        {
            StringBuilder shaderSource = new StringBuilder();
            AppendFragmentShader(textures, attributes, shaderSource);

            return shaderSource.ToString();
        }

        private static void AppendFragmentShader(List<TextureRenderInfo> textures, List<VertexAttributeRenderInfo> attributes, StringBuilder shaderSource)
        {
            GlslUtils.AppendShadingLanguageVersion(shaderSource);

            GlslUtils.AppendFragmentInputs(attributes, shaderSource);
            GlslUtils.AppendFragmentOutput(shaderSource);

            AppendTextureUniforms(textures, shaderSource);

            shaderSource.AppendLine($"uniform int {attribIndexName};");

            AppendFragmentMainFunction(textures, shaderSource);
        }

        private static void AppendTextureUniforms(List<TextureRenderInfo> textures, StringBuilder shaderSource)
        {
            foreach (var texture in textures)
            {
                // TODO: Don't assume sampler type.
                shaderSource.AppendLine($"uniform sampler2D {texture.name};");
            }
        }

        private static void AppendFragmentMainFunction(List<TextureRenderInfo> textures, StringBuilder shaderSource)
        {
            GlslUtils.AppendBeginMain(shaderSource);
            AppendMainFunctionBody(textures, shaderSource);
            GlslUtils.AppendEndMain(shaderSource);
        }

        private static void AppendMainFunctionBody(List<TextureRenderInfo> attributes, StringBuilder shaderSource)
        {
            shaderSource.AppendLine($"\tvec3 {resultName} = vec3(0);");

            AppendFragmentAttributeSwitch(attributes, shaderSource);

            shaderSource.AppendLine($"\t{GlslUtils.outputName} = vec4({resultName}, 1);");
        }

        private static void AppendFragmentAttributeSwitch(List<TextureRenderInfo> attributes, StringBuilder shaderSource)
        {
            List<CaseStatement> cases = new List<CaseStatement>();
            for (int i = 0; i < attributes.Count; i++)
            {
                // TODO: Don't hard code UV names.
                string caseAssignment = GetResultAssignment(ValueCount.Three, attributes[i], "uv0");
                cases.Add(new CaseStatement(i.ToString(), caseAssignment));
            }

            SwitchUtils.AppendSwitchStatement(shaderSource, attribIndexName, cases);
        }

        private static string GetResultAssignment(ValueCount resultCount, TextureRenderInfo texture, string uv0Name)
        {
            return $"{resultName}.rgb = texture({texture.name}, {GlslUtils.vertexOutputPrefix}{uv0Name}).rgb;";
        }
    }
}