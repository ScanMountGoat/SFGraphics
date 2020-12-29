using System;
using System.Collections.Generic;

namespace SFGraphics.ShaderGen.GlslShaderUtils
{
    /// <summary>
    /// The number of vector components for a vertex attribute. Scalars should use <see cref="One"/>.
    /// </summary>
    public enum ValueCount
    {
        /// <summary>
        /// A scalar value
        /// </summary>
        One = 1,

        /// <summary>
        /// A two component vector value
        /// </summary>
        Two = 2,

        /// <summary>
        /// A three component vector value
        /// </summary>
        Three = 3,

        /// <summary>
        /// A four component vector value
        /// </summary>
        Four = 4
    }

    public enum AttributeType
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
        UVec4
    }

    public class ShaderAttribute
    {
        public string Name { get; }
        public AttributeType Type { get; }
        public string TypeDeclaration { get; }
        public string Interpolation { get; }
        public ValueCount ValueCount { get; }

        public ShaderAttribute(string name, AttributeType type)
        {
            Name = name;
            Type = type;
            ValueCount = GetValueCount(type);
            Interpolation = GetInterpolationQualifier(type);
            TypeDeclaration = GetTypeDeclaration(type);
        }

        private string GetTypeDeclaration(AttributeType type)
        {
            switch (type)
            {
                case AttributeType.UnsignedInt:
                    return "uint";
                case AttributeType.Int:
                    return "int";
                case AttributeType.Float:
                    return "float";
                case AttributeType.Vec2:
                    return "vec2";
                case AttributeType.Vec3:
                    return "vec3";
                case AttributeType.Vec4:
                    return "vec4";
                case AttributeType.IVec2:
                    return "ivec2";
                case AttributeType.IVec3:
                    return "ivec3";
                case AttributeType.IVec4:
                    return "ivec4";
                case AttributeType.UVec2:
                    return "uvec2";
                case AttributeType.UVec3:
                    return "uvec3";
                case AttributeType.UVec4:
                    return "uvec4";
                default:
                    return "";
            }
        }

        private static string GetInterpolationQualifier(AttributeType type)
        {
            switch (type)
            {
                case AttributeType.UnsignedInt:
                case AttributeType.Int:
                case AttributeType.IVec2:
                case AttributeType.IVec3:
                case AttributeType.IVec4:
                case AttributeType.UVec2:
                case AttributeType.UVec3:
                case AttributeType.UVec4:
                    return "flat";
                default:
                    return "";
            }
        }

        private static ValueCount GetValueCount(AttributeType type)
        {
            switch (type)
            {
                case AttributeType.Vec2:
                case AttributeType.IVec2:
                case AttributeType.UVec2:
                    return ValueCount.Two;
                case AttributeType.Vec3:
                case AttributeType.IVec3:
                case AttributeType.UVec3:
                    return ValueCount.Three;
                case AttributeType.Vec4:
                case AttributeType.IVec4:
                case AttributeType.UVec4:
                    return ValueCount.Four;
                default:
                    return ValueCount.One;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is ShaderAttribute attribute &&
                   Name == attribute.Name &&
                   Type == attribute.Type;
        }

        public override int GetHashCode()
        {
            int hashCode = -243844509;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            return hashCode;
        }
    }
}
