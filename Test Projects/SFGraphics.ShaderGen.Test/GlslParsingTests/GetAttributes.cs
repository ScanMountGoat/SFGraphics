using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.ShaderGen.GlslShaderUtils;
using System.Collections.Generic;

namespace SFGraphics.ShaderGen.Test.ShaderGeneratorTests
{
    [TestClass]
    public class GetAttributes
    {
        [TestMethod]
        public void MixedAttributes()
        {
            var glsl = @"
in uint a;
in int b;
in float c;
in vec2 d;
in vec3 e;
in vec4 f;
in uvec2 g;
in uvec3 h;
in uvec4 i;
in ivec2 j;
in ivec3 k;
in ivec4 l;";

            CollectionAssert.AreEqual(new List<ShaderAttribute>
            {
                new ShaderAttribute("a", AttributeType.UnsignedInt),
                new ShaderAttribute("b", AttributeType.Int),
                new ShaderAttribute("c", AttributeType.Float),
                new ShaderAttribute("d", AttributeType.Vec2),
                new ShaderAttribute("e", AttributeType.Vec3),
                new ShaderAttribute("f", AttributeType.Vec4),
                new ShaderAttribute("g", AttributeType.UVec2),
                new ShaderAttribute("h", AttributeType.UVec3),
                new ShaderAttribute("i", AttributeType.UVec4),
                new ShaderAttribute("j", AttributeType.IVec2),
                new ShaderAttribute("k", AttributeType.IVec3),
                new ShaderAttribute("l", AttributeType.IVec4),
            }, GlslParsing.GetAttributes(glsl));
        }

        [TestMethod]
        public void SingleAttribute()
        {
            var glsl = "in float float1;";

            CollectionAssert.AreEqual(new List<ShaderAttribute>
            {
                new ShaderAttribute("float1", AttributeType.Float),
            }, GlslParsing.GetAttributes(glsl));
        }


        [TestMethod]
        public void EmptySource()
        {
            var glsl = "";

            CollectionAssert.AreEqual(new List<ShaderAttribute>(), GlslParsing.GetAttributes(glsl));
        }


        [TestMethod]
        public void MissingSemicolon()
        {
            var glsl = "in vec3 a";

            CollectionAssert.AreEqual(new List<ShaderAttribute>(), GlslParsing.GetAttributes(glsl));
        }

        [TestMethod]
        public void MissingTypeDescription()
        {
            var glsl = "in a";

            CollectionAssert.AreEqual(new List<ShaderAttribute>(), GlslParsing.GetAttributes(glsl));
        }
    }
}
