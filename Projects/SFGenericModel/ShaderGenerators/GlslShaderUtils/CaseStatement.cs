namespace SFGenericModel.ShaderGenerators.GlslShaderUtils
{
    internal class CaseStatement
    {
        public string SwitchValue { get; }
        public string CaseBody { get; }

        public CaseStatement(string switchValue, string caseBody)
        {
            SwitchValue = switchValue;
            CaseBody = caseBody;
        }
    }
}
