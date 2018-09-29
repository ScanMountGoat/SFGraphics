using System.Text;

namespace SFGenericModel.ShaderGenerators.GlslShaderUtils
{
    internal static class SwitchUtils
    {
        public static void AppendSwitchCaseStatement(StringBuilder shaderSource, int index, string caseAssignment)
        {
            shaderSource.AppendLine($"\t\tcase {index}:");
            shaderSource.AppendLine($"\t\t\t{caseAssignment}");
            shaderSource.AppendLine($"\t\t\tbreak;");
        }

        public static void AppendEndSwitch(StringBuilder shaderSource)
        {
            shaderSource.AppendLine("\t}");
        }

        public static void AppendBeginSwitch(StringBuilder shaderSource, string switchVariableName)
        {
            shaderSource.AppendLine($"\tswitch ({switchVariableName})");
            shaderSource.AppendLine("\t{");
        }
    }
}
