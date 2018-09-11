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
        public class SetVector3
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
                shader.SetVector3("vector3a", new Vector3(1));
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("vector3a", ActiveUniformType.FloatVec3);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
                Assert.AreEqual(0, eventArgs.Count);
            }

            [TestMethod]
            public void InvalidName()
            {
                shader.SetVector3("memes", new Vector3(1));
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.FloatVec3);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
                Assert.AreEqual(1, eventArgs.Count);
            }

            [TestMethod]
            public void InvalidType()
            {
                shader.SetVector3("float1", 1, 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.FloatVec3);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
                Assert.AreEqual(1, eventArgs.Count);
            }

            [TestMethod]
            public void FloatsValidName()
            {
                shader.SetVector3("vector3a", 1, 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("vector3a", ActiveUniformType.FloatVec3);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
                Assert.AreEqual(0, eventArgs.Count);
            }

            [TestMethod]
            public void FloatsInvalidName()
            {
                shader.SetVector3("memes2", 1, 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes2", ActiveUniformType.FloatVec3);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
                Assert.AreEqual(1, eventArgs.Count);
            }
        }
    }
}
