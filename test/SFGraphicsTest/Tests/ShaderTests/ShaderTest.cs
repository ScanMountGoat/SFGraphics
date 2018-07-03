using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphicsTest.ShaderTests
{
    [TestClass]
    public partial class ShaderTest
    {
        public static Shader SetupContextCreateValidFragShader()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.CreateDummyContext();

            // Load the shader file from the embedded resources.
            // Used for testing shader setters.
            Shader shader = new Shader();
            string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphicsTest.Shaders.validFrag.frag");
            shader.LoadShader(shaderSource, ShaderType.FragmentShader);
            return shader;
        }
    }
}
