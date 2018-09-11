using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;

namespace SFGraphics.Test.RenderTests.ShaderTests.SetterTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetFloat
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
            public void ValidNameValidType()
            {
                shader.SetFloat("float1", 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.Float);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
                Assert.AreEqual(0, eventArgs.Count);
            }

            [TestMethod]
            public void InvalidType()
            {
                shader.SetFloat("int1", 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("int1", ActiveUniformType.Float);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
                Assert.AreEqual(1, eventArgs.Count);
            }

            [TestMethod]
            public void InvalidName()
            {
                shader.SetFloat("memes", 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.Float);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
                Assert.AreEqual(1, eventArgs.Count);
            }
        }
    }
}
