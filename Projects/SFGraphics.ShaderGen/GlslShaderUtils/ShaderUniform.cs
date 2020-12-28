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
    }
}
