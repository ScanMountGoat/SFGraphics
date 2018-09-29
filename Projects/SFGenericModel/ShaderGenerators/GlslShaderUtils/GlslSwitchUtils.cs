using System.Text;

namespace SFGenericModel.ShaderGenerators.GlslShaderUtils
{
    internal static class GlslSwitchUtils
    {
        public static void AppendSwitchCaseStatement(StringBuilder shaderSource, int index, string caseAssignment)
        {
            shaderSource.AppendLine($"\t\tcase {index}:");
            shaderSource.AppendLine($"\t\t\t{caseAssignment}");
            shaderSource.AppendLine($"\t\t\tbreak;");
        }
    }
}
