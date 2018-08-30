using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.RenderTests.ShaderTests
{
    public static class ShaderSetup
    {
        public static Shader SetupContextCreateValidFragShader()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.BindDummyContext();

            // Load the shader file from the embedded resources.
            // Used for testing shader setters.
            Shader shader = new Shader();
            string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphics.Test.RenderTests.Shaders.validFrag.frag");
            shader.LoadShader(shaderSource, ShaderType.FragmentShader);
            return shader;
        }
    }
}
