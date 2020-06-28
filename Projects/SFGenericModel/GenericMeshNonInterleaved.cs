using OpenTK.Graphics.OpenGL;
using SFGenericModel.MeshEventArgs;
using SFGenericModel.Utils;
using SFGenericModel.VertexAttributes;
using SFGraphics.GLObjects.BufferObjects;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;

namespace SFGenericModel
{
    /// <summary>
    /// A class for drawing indexed vertex data with vertex attributes stored in separate buffers.
    /// </summary>
    public class GenericMeshNonInterleaved : GenericMeshBase
    {
        private GenericMeshNonInterleaved(PrimitiveType primitiveType, DrawElementsType drawElementsType, int vertexCount) : base(primitiveType, drawElementsType, vertexCount)
        {

        }

        // TODO:
        // Support a variable number of attributes.
        // Each attribute has it's own buffer using generics (each buffer can be specified in a different type. OpenGL won't care).
        // Validate buffer sizes based on vertex shader attribute information.

        // Possible implementation.
        void SetAttributeData<T>(string attributeName, T[] attributeData)
        {
            // Maintain a dictionary of <name,buffer> for all attributes in shader.
            // Handle freeing replaced buffers.
            // Validate name from shader.
            // Validate buffer using vertex count, sizeof(T), and array length to avoid potentially nasty runtime errors.
            // Throw exception on error?

            // Unset attributes:
            // Keep track of all attributes from shader (unused attributes are optimized out by the shader compiler).
            // Check before draw for any unset attributes and throw exception.
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shader">The shader to query for attribute information</param>
        protected override void ConfigureVertexAttributes(Shader shader)
        {
            throw new System.NotImplementedException();
        }
    }
}
