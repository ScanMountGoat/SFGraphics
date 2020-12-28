namespace SFGraphics.ShaderGen.GlslShaderUtils
{
    /// <summary>
    /// Stores information for the cases of a switch statement.
    /// </summary>
    internal class CaseStatement
    {
        /// <summary>
        /// The value for this case.
        /// </summary>
        public string SwitchValue { get; }

        /// <summary>
        /// The code to be executed.
        /// </summary>
        public string CaseBody { get; }

        public CaseStatement(string switchValue, string caseBody)
        {
            SwitchValue = switchValue;
            CaseBody = caseBody;
        }
    }
}
