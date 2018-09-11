using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;

namespace SFGraphics.Test.RenderTests.ShaderTests.SetterTests
{
    [TestClass]
    public class SetBase
    {
        protected Shader shader;
        protected List<UniformSetEventArgs> eventArgs = new List<UniformSetEventArgs>();

        [TestInitialize()]
        public void Initialize()
        {
            if (shader == null)
            {
                shader = ShaderTestUtils.SetUpContextCreateValidShader();
                shader.OnInvalidUniformSet += Shader_OnInvalidUniformSet;
            }

            eventArgs.Clear();
        }

        private void Shader_OnInvalidUniformSet(Shader sender, UniformSetEventArgs e)
        {
            eventArgs.Add(e);
        }
    }
}
