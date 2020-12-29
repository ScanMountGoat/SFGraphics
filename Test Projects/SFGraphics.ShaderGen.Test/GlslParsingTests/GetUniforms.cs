using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.ShaderGen.GlslShaderUtils;
using System.Collections.Generic;

namespace SFGraphics.ShaderGen.Test.ShaderGeneratorTests
{
    [TestClass]
    public class GetUniforms
    {
        [TestMethod]
        public void MixedUniforms()
        {
            var glsl = @"
uniform sampler2D texture1;
uniform float float1;
uniform int int1;
uniform vec2 vector1;
uniform vec3 vector2;
uniform vec4 vector4;
uniform samplerCube cubeMap1;";

            CollectionAssert.AreEqual(new List<ShaderUniform>
            {
                new ShaderUniform("texture1", "sampler2D"),
                new ShaderUniform("float1", "float"),
                new ShaderUniform("int1", "int"),
                new ShaderUniform("vector1", "vec2"),
                new ShaderUniform("vector2", "vec3"),
                new ShaderUniform("vector4", "vec4"),
                new ShaderUniform("cubeMap1", "samplerCube")
            }, GlslParsing.GetUniforms(glsl));
        }

        [TestMethod]
        public void SingleUniform()
        {
            var glsl = "uniform float float1;";

            CollectionAssert.AreEqual(new List<ShaderUniform>
            {
                new ShaderUniform("float1", "float"),
            }, GlslParsing.GetUniforms(glsl));
        }


        [TestMethod]
        public void EmptySource()
        {
            var glsl = "";

            CollectionAssert.AreEqual(new List<ShaderUniform>(), GlslParsing.GetUniforms(glsl));
        }


        [TestMethod]
        public void MissingSemicolon()
        {
            var glsl = "uniform float float1";

            CollectionAssert.AreEqual(new List<ShaderUniform>(), GlslParsing.GetUniforms(glsl));
        }

        [TestMethod]
        public void MissingTypeDescription()
        {
            var glsl = "uniform float1";

            CollectionAssert.AreEqual(new List<ShaderUniform>(), GlslParsing.GetUniforms(glsl));
        }

        [TestMethod]
        public void UniformBlock()
        {
            var glsl = @"
uniform MaterialParams
{
    vec4 CustomVector[64];
    ivec4 CustomBoolean[20];
    vec4 CustomFloat[20];
    vec4 vec4Param;
};";

            // Uniform blocks will have to be handled separately.
            CollectionAssert.AreEqual(new List<ShaderUniform>(), GlslParsing.GetUniforms(glsl));
        }
    }
}
