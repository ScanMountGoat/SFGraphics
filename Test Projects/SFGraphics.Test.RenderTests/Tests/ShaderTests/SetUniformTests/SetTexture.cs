using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;
using System.Collections.Generic;

namespace SFGraphics.Test.RenderTests.ShaderTests.SetterTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetTexture
        {
            private Shader shader;
            private List<UniformSetEventArgs> eventArgs = new List<UniformSetEventArgs>();

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

            [TestMethod]
            public void ValidNamealidTarget()
            {
                shader.SetTexture("tex2D", new Texture2D(), 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("tex2D", ActiveUniformType.Sampler2D);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
                Assert.AreEqual(0, eventArgs.Count);
            }

            [TestMethod]
            public void InvalidName()
            {
                shader.SetTexture("memes", new Texture2D(), 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.Sampler2D);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
                Assert.AreEqual(1, eventArgs.Count);
            }

            [TestMethod]
            public void InvalidType()
            {
                shader.SetTexture("float1", new Texture2D(), 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.Sampler2D);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
                Assert.AreEqual(1, eventArgs.Count);
            }

            [TestMethod]
            public void InvalidTarget()
            {
                shader.SetTexture("texCube", new Texture2D(), 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("texCube", ActiveUniformType.Sampler2D);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
                Assert.AreEqual(1, eventArgs.Count);
            }
        }
    }
}
