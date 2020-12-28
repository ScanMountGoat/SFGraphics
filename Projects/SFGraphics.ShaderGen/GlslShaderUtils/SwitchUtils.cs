using System.Collections.Generic;
using System.Text;

namespace SFGraphics.ShaderGen.GlslShaderUtils
{
    internal static class SwitchUtils
    {
        public static void AppendSwitchStatement(StringBuilder shaderSource, string switchVariable, List<CaseStatement> cases)
        {
            AppendBeginSwitch(shaderSource, switchVariable);
            foreach (var caseStatement in cases)
            {
                AppendSwitchCaseStatement(shaderSource, caseStatement);
            }
            AppendEndSwitch(shaderSource);
        }

        private static void AppendSwitchCaseStatement(StringBuilder shaderSource, CaseStatement caseStatement)
        {
            shaderSource.AppendLine($"\t\tcase {caseStatement.SwitchValue}:");
            shaderSource.AppendLine($"\t\t\t{caseStatement.CaseBody}");
            shaderSource.AppendLine("\t\t\tbreak;");
        }

        private static void AppendBeginSwitch(StringBuilder shaderSource, string switchVariable)
        {
            shaderSource.AppendLine($"\tswitch ({switchVariable})");
            shaderSource.AppendLine("\t{");
        }

        private static void AppendEndSwitch(StringBuilder shaderSource)
        {
            shaderSource.AppendLine("\t}");
        }
    }
}
