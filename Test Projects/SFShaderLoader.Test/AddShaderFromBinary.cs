﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SFGraphics.GLObjects.Shaders;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace SFShaderLoader.Test
{
    [TestClass]
    public class AddShaderFromBinary
    {
        private readonly ShaderLoader loader = new ShaderLoader();

        [TestInitialize]
        public void Setup()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void AddValidBinary()
        {
            // Compile a separate shader program to get the binaries.
            var shader = new Shader();
            shader.LoadShaders(File.ReadAllText("Shaders/valid.vert"), File.ReadAllText("Shaders/valid.frag"));
            var binary = shader.GetProgramBinary(out BinaryFormat format);

            Assert.IsTrue(loader.AddShader("validShader", binary, format));
        }

        [TestMethod]
        public void AddInvalidBinary()
        {
            // Compile a separate shader program to get the binaries.
            var shader = new Shader();
            shader.LoadShaders(File.ReadAllText("Shaders/valid.vert"), File.ReadAllText("Shaders/valid.frag"));
            var binary = shader.GetProgramBinary(out BinaryFormat format);

            Assert.IsFalse(loader.AddShader("validShader", binary, 0));
        }
    }
}
