using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;

namespace SFGraphics.Test.ShaderTests
{
    [TestClass]
    class ShaderTestValidShader
    {
        protected Shader shader;
        protected List<UniformSetEventArgs> eventArgs = new List<UniformSetEventArgs>();

        [TestInitialize()]
        public void Initialize()
        {
            if (shader == null)
            {
                shader = RenderTestUtils.ShaderTestUtils.CreateValidShader();
                shader.OnInvalidUniformSet += Shader_OnInvalidUniformSet;
            }

            eventArgs.Clear();
        }

        private void Shader_OnInvalidUniformSet(object sender, UniformSetEventArgs e)
        {
            eventArgs.Add(e);
        }
    }
}
