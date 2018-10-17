using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests
{
    [TestClass]
    public abstract class ShaderTest : Tests.ContextTest
    {
        protected Shader shader;

        protected List<UniformSetEventArgs> invalidUniformSets = new List<UniformSetEventArgs>();
        protected List<TextureSetEventArgs> invalidTextureSets = new List<TextureSetEventArgs>();

        [TestInitialize()]
        public override void Initialize()
        {
            base.Initialize();

            if (shader == null)
            {
                shader = RenderTestUtils.ShaderTestUtils.CreateValidShader();
                shader.OnInvalidUniformSet += Shader_OnInvalidUniformSet;
                shader.OnTextureUnitTypeMismatch += Shader_OnTextureUnitTypeMismatch;
            }

            invalidUniformSets.Clear();
            invalidTextureSets.Clear();
        }

        private void Shader_OnTextureUnitTypeMismatch(object sender, TextureSetEventArgs e)
        {
            invalidTextureSets.Add(e);
        }

        private void Shader_OnInvalidUniformSet(object sender, UniformSetEventArgs e)
        {
            invalidUniformSets.Add(e);
        }

        public bool IsValidSet(string name, ActiveUniformType type)
        {
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage(name, type);
            return !shader.GetErrorLog().Contains(expected) && invalidUniformSets.Count == 0;
        }
    }
}
