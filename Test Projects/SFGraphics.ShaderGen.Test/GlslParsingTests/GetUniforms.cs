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
                new ShaderUniform("texture1", UniformType.Sampler2D),
                new ShaderUniform("float1", UniformType.Float),
                new ShaderUniform("int1", UniformType.Int),
                new ShaderUniform("vector1", UniformType.Vec2),
                new ShaderUniform("vector2", UniformType.Vec3),
                new ShaderUniform("vector4", UniformType.Vec4),
                new ShaderUniform("cubeMap1", UniformType.SamplerCube)
            }, GlslParsing.GetUniforms(glsl));
        }

        [TestMethod]
        public void MatrixUniforms()
        {
            var glsl = @"
uniform mat3 var1;
uniform mat4 var2;";

            CollectionAssert.AreEqual(new List<ShaderUniform>
            {
                new ShaderUniform("var1", UniformType.Mat3),
                new ShaderUniform("var2", UniformType.Mat4),
            }, GlslParsing.GetUniforms(glsl));
        }

        [TestMethod]
        public void UVecUniforms()
        {
            var glsl = @"
uniform uvec2 var1;
uniform uvec3 var2;
uniform uvec4 var3;";

            CollectionAssert.AreEqual(new List<ShaderUniform>
            {
                new ShaderUniform("var1", UniformType.UVec2),
                new ShaderUniform("var2", UniformType.UVec3),
                new ShaderUniform("var3", UniformType.UVec4),
            }, GlslParsing.GetUniforms(glsl));
        }

        [TestMethod]
        public void IVecUniforms()
        {
            var glsl = @"
uniform ivec2 var1;
uniform ivec3 var2;
uniform ivec4 var3;";

            CollectionAssert.AreEqual(new List<ShaderUniform>
            {
                new ShaderUniform("var1", UniformType.IVec2),
                new ShaderUniform("var2", UniformType.IVec3),
                new ShaderUniform("var3", UniformType.IVec4),
            }, GlslParsing.GetUniforms(glsl));
        }

        [TestMethod]
        public void SingleUniform()
        {
            var glsl = "uniform float float1;";

            CollectionAssert.AreEqual(new List<ShaderUniform>
            {
                new ShaderUniform("float1", UniformType.Float),
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
