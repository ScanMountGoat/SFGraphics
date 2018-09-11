using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;
using System.Collections.Generic;

namespace SFGraphics.Test.RenderTests.ShaderTests.SetterTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetVector2
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
            public void ValidName()
            {
                shader.SetVector2("vector2a", new Vector2(1));
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("vector2a", ActiveUniformType.FloatVec2);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
                Assert.AreEqual(0, eventArgs.Count);
            }

            [TestMethod]
            public void InvalidName()
            {
                shader.SetVector2("memes", new Vector2(1));
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.FloatVec2);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void FloatsValidName()
            {
                shader.SetVector2("vector2a", 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("vector2a", ActiveUniformType.FloatVec2);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
                Assert.AreEqual(0, eventArgs.Count);
            }

            [TestMethod]
            public void FloatsInvalidName()
            {
                shader.SetVector2("memes2", 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes2", ActiveUniformType.FloatVec2);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
                Assert.AreEqual(1, eventArgs.Count);
            }

            [TestMethod]
            public void InvalidType()
            {
                shader.SetVector2("float1", 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.FloatVec2);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
                Assert.AreEqual(1, eventArgs.Count);
            }
        }
    }
}
