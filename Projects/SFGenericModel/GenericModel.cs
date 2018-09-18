using SFGraphics.Cameras;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using SFGenericModel.ShaderGenerators;

namespace SFGenericModel
{
    /// <summary>
    /// 
    /// </summary>
    public class GenericModel
    {
        private List<IDrawableMesh> meshes = new List<IDrawableMesh>();

        private Shader vertAttributeShader;
        private Shader textureShader;

        /// <summary>
        /// Creates a model with no meshes.
        /// </summary>
        public GenericModel()
        {

        }

        /// <summary>
        /// Creates a model from <paramref name="meshes"/>.
        /// </summary>
        /// <param name="meshes">The meshes used for drawing</param>
        public GenericModel(List<IDrawableMesh> meshes)
        {
            this.meshes = meshes;
        }

        /// <summary>
        /// Draws all meshes.
        /// </summary>
        /// <param name="shader">The shader used for drawing</param>
        /// <param name="camera">The camera used for drawing all objects in <see cref="meshes"/></param>
        public void Draw(Shader shader, Camera camera)
        {
            foreach (var mesh in meshes)
            {
                mesh.Draw(shader, camera);
            }
        }

        private bool GenerateVertDebugShader(List<VertexAttributeRenderInfo> attributes)
        {
            vertAttributeShader = VertexAttributeShaderGenerator.CreateShader(attributes);
            return vertAttributeShader.LinkStatusIsOk;
        }
    }
}
