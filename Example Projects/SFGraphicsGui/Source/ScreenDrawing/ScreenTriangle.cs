using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Samplers;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Textures;
using System.Collections.Generic;

namespace SFGraphicsGui
{
    class ScreenTriangle : SFGenericModel.GenericMesh<ScreenTriangleVertex>
    {
        // A triangle that extends past the screen.
        private static List<ScreenTriangleVertex> screenTrianglePositions = new List<ScreenTriangleVertex>()
        {
            new ScreenTriangleVertex(new Vector3(-1f, -1f, 0.0f), new Vector3(-1,0,0), new Vector4(1)),
            new ScreenTriangleVertex(new Vector3( 3f, -1f, 0.0f), new Vector3(0,-25,25), new Vector4(1)),
            new ScreenTriangleVertex(new Vector3(-1f,  3f, 0.0f), new Vector3(0,1,-1), new Vector4(1))
        };

        public ScreenTriangle() : base(screenTrianglePositions, PrimitiveType.Triangles)
        {

        }

        public void DrawScreenTexture(Texture2D texture, Texture3D lut, Shader shader, SamplerObject sampler)
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
            shader.SetTexture("lutTexture", lut, textureUnit + 1);

            Draw(shader);
        }
    }
}
