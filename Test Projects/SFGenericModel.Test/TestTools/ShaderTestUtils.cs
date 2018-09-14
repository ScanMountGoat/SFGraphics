using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace TestTools
{
    public static class ShaderTestUtils
    {
        public static Shader SetUpContextCreateValidShader()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.BindDummyContext();

            Shader shader = new Shader();

            string fragSource = TestTools.ResourceShaders.GetShader("validFrag.frag");
            shader.LoadShader(fragSource, ShaderType.FragmentShader);

            string vertSource = TestTools.ResourceShaders.GetShader("validVert.vert");
            shader.LoadShader(vertSource, ShaderType.VertexShader);

            shader.UseProgram();

            return shader;
        }
    }
}
