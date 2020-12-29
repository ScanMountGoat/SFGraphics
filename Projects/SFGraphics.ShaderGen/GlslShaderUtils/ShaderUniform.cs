using System.Collections.Generic;

namespace SFGraphics.ShaderGen.GlslShaderUtils
{
    public class ShaderUniform
    {
        public string Name { get; }
        public string Type { get; }

        public ShaderUniform(string name, string type)
        {
            Name = name;
            Type = type;
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
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);
            return hashCode;
        }
    }
}
