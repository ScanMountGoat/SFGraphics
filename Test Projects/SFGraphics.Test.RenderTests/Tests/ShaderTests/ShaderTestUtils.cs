using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace ShaderTests
{
    public static class ShaderTestUtils
    {
        public static Shader SetUpContextCreateValidShader()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.BindDummyContext();

            Shader shader = new Shader();

            string fragSource = TestTools.ResourceShaders.GetShader("SFGraphics.Test.RenderTests.Shaders.validFrag.frag");
            shader.LoadShader(fragSource, ShaderType.FragmentShader);

            string vertSource = TestTools.ResourceShaders.GetShader("SFGraphics.Test.RenderTests.Shaders.validVert.vert");
            shader.LoadShader(vertSource, ShaderType.VertexShader);

            shader.UseProgram();

            return shader;
        }

        public static string GetInvalidUniformErrorMessage(string name, ActiveUniformType type)
        {
            return $"[Warning] Attempted to set undeclared uniform variable { name } of type { type }";
        }
    }
}
