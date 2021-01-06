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

        private static readonly Dictionary<AttributeType, string> nameByType = new Dictionary<AttributeType, string>
        {
            { AttributeType.UnsignedInt, "uint" },
            { AttributeType.Int, "int" },
            { AttributeType.Float, "float" },
            { AttributeType.Vec2, "vec2" },
            { AttributeType.Vec3, "vec3" },
            { AttributeType.Vec4, "vec4" },
            { AttributeType.IVec2, "ivec2" },
            { AttributeType.IVec3, "ivec3" },
            { AttributeType.IVec4, "ivec4" },
            { AttributeType.UVec2, "uvec2" },
            { AttributeType.UVec3, "uvec3" },
            { AttributeType.UVec4, "uvec4" },
        };

        public ShaderAttribute(string name, AttributeType type)
        {
            Name = name;
            Type = type;
            ValueCount = GetValueCount(type);
            Interpolation = GetInterpolationQualifier(type);
            TypeDeclaration = GetTypeDeclaration(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <exception cref="NotSupportedException"><paramref name="type"/> is not a supported attribute type</exception>
        public ShaderAttribute(string name, string type) : this(name, GetAttributeType(type))
        {

        }


        private string GetTypeDeclaration(AttributeType type)
        {
            if (nameByType.TryGetValue(type, out string name))
                return name;

            throw new NotSupportedException($"Unsupported uniform type {type}");
        }

        public static AttributeType GetAttributeType(string name)
        {
            foreach (var pair in nameByType)
            {
                if (pair.Value == name)
                    return pair.Key;
            }

            throw new NotSupportedException($"Unsupported uniform type {name}");
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
