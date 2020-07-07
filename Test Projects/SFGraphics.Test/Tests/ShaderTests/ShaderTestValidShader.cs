using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RenderTestUtils;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;

namespace SFGraphics.Test.ShaderTests
{
    [TestClass]
    internal class ShaderTestValidShader
    {
        protected Shader shader;
        protected List<UniformSetEventArgs> eventArgs = new List<UniformSetEventArgs>();

        [TestInitialize]
        public void Initialize()
        {
            if (shader == null)
            {
                shader = ShaderTestUtils.CreateValidShader();
                shader.InvalidUniformSet += Shader_OnInvalidUniformSet;
            }

            eventArgs.Clear();
        }

        private void Shader_OnInvalidUniformSet(object sender, UniformSetEventArgs e)
        {
            eventArgs.Add(e);
        }
    }
}
