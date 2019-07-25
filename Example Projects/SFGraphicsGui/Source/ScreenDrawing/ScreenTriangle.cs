using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Samplers;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Textures;

namespace SFGraphicsGui
{
    class ScreenTriangle : SFGenericModel.GenericMesh<ScreenVertex>
    {
        // A triangle that extends past the screen.
        private static readonly ScreenVertex[] screenTrianglePositions = 
        {
            new ScreenVertex(new Vector3(-1f, -1f, 0.0f)),
            new ScreenVertex(new Vector3( 3f, -1f, 0.0f)), 
            new ScreenVertex(new Vector3(-1f,  3f, 0.0f))
        };

        public ScreenTriangle() : base(screenTrianglePositions, PrimitiveType.Triangles)
        {

        }

        public void DrawScreenTexture(Texture2D texture, Shader shader, SamplerObject sampler)
        {
            if (texture == null)
                return;

            // Always check program creation before using shaders to prevent crashes.
            if (!shader.LinkStatusIsOk)
                return;

            // Render using the shader.
            shader.UseProgram();

            // The sampler's parameters are used instead of the texture's parameters.
            int textureUnit = 0;
            sampler.Bind(textureUnit);

            shader.SetInt("attributeIndex", 1);
            Matrix4 matrix4 = Matrix4.Identity;
            shader.SetMatrix4x4("mvpMatrix", ref matrix4);

            shader.SetTexture("uvTexture", texture, textureUnit);

            GL.Disable(EnableCap.DepthTest);

            Draw(shader);
        }
    }
}
