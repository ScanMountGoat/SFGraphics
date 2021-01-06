using System.Collections.Generic;
using System;

namespace SFGraphics.ShaderGen.GlslShaderUtils
{
    public enum UniformType
    {
        UnsignedInt,
        Int,
        Float,
        Vec2,
        Vec3,
        Vec4,
        IVec2,
        IVec3,
        IVec4,
        UVec2,
        UVec3,
        UVec4,
        Mat3,
        Mat4,
        Sampler2D,
        SamplerCube
    }

    public class ShaderUniform
    {
        /// <summary>
        /// The variable name for the uniform such as "myUniform".
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The type of the uniform such as <see cref="UniformType.SamplerCube"/>.
        /// </summary>
        public UniformType Type { get; }

        /// <summary>
        /// The GLSL type string such as "samplerCube".
        /// </summary>
        public string TypeDeclaration { get; }

        private static readonly Dictionary<UniformType, string> nameByType = new Dictionary<UniformType, string>
        {
            { UniformType.UnsignedInt, "uint" },
            { UniformType.Int, "int" },
            { UniformType.Float, "float" },
            { UniformType.Vec2, "vec2" },
            { UniformType.Vec3, "vec3" },
            { UniformType.Vec4, "vec4" },
            { UniformType.IVec2, "ivec2" },
            { UniformType.IVec3, "ivec3" },
            { UniformType.IVec4, "ivec4" },
            { UniformType.UVec2, "uvec2" },
            { UniformType.UVec3, "uvec3" },
            { UniformType.UVec4, "uvec4" },
            { UniformType.Mat3, "mat3" },
            { UniformType.Mat4, "mat4" },
            { UniformType.Sampler2D, "sampler2D" },
            { UniformType.SamplerCube, "samplerCube" },
        };

        public ShaderUniform(string name, UniformType type)
        {
            Name = name;
            Type = type;
            TypeDeclaration = GetTypeDeclaration(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <exception cref="NotSupportedException"><paramref name="type"/> is not a supported uniform type</exception>
        public ShaderUniform(string name, string type) : this(name, GetUniformType(type))
        {

        }

        private string GetTypeDeclaration(UniformType type)
        {
            if (nameByType.TryGetValue(type, out string name))
                return name;

            throw new NotSupportedException($"Unsupported uniform type {type}");
        }

        public static UniformType GetUniformType(string name)
        {
            foreach (var pair in nameByType)
            {
                if (pair.Value == name)
                    return pair.Key;
            }

            throw new NotSupportedException($"Unsupported uniform type {name}");
        }

        public override bool Equals(object obj)
        {
            return obj is ShaderUniform uniform &&
                   Name == uniform.Name &&
                   Type == uniform.Type;
        }

        public override int GetHashCode()
        {
            int hashCode = -243844509;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<UniformType>.Default.GetHashCode(Type);
            return hashCode;
        }
    }
}
