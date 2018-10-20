using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;
using System.Collections.Generic;
using System.Linq;

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

            if (invalidUniformSets.Count > 0)
            {
                Assert.AreEqual(name, invalidUniformSets.Last().Name);
                Assert.AreEqual(type, invalidUniformSets.Last().Type);
            }

            return !shader.GetErrorLog().Contains(expected) && invalidUniformSets.Count == 0;
        }

        public float GetFloat(string name)
        {
            GL.GetUniform(shader.Id, shader.GetUniformLocation(name), out float value);
            return value;
        }

        public int GetInt(string name)
        {
            GL.GetUniform(shader.Id, shader.GetUniformLocation(name), out int value);
            int[] x = new int[16];
            GL.GetUniform(shader.Id, shader.GetUniformLocation(name), x);
            return value;
        }

        public Vector2 GetVector2(string name)
        {
            float[] value = new float[2];
            GL.GetUniform(shader.Id, shader.GetUniformLocation(name), value);
            return new Vector2(value[0], value[1]);
        }

        public Vector3 GetVector3(string name)
        {
            float[] value = new float[3];
            GL.GetUniform(shader.Id, shader.GetUniformLocation(name), value);
            return new Vector3(value[0], value[1], value[2]);
        }

        public Vector4 GetVector4(string name)
        {
            float[] value = new float[4];
            GL.GetUniform(shader.Id, shader.GetUniformLocation(name), value);
            return new Vector4(value[0], value[1], value[2], value[3]);
        }

        public uint GetUint(string name)
        {
            // HACK: The unsigned int method overload doesn't work for some reason.
            GL.GetUniform(shader.Id, shader.GetUniformLocation(name), out int value);
            return System.BitConverter.ToUInt32(System.BitConverter.GetBytes(value), 0);
        }
    }
}
