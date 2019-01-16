using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace SFGraphics.Utils
{
    /// <summary>
    /// Contains methods for calculating vertex properties for PrimitiveType.Triangles.
    /// </summary>
    public static class TriangleListUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="normals"></param>
        /// <param name="uvs"></param>
        /// <param name="indices"></param>
        /// <param name="tangents"></param>
        /// <param name="bitangents"></param>
        public static void CalculateTangentsBitangents(IList<Vector3> positions, IList<Vector3> normals, IList<Vector2> uvs, IList<int> indices, out Vector3[] tangents, out Vector3[] bitangents)
        {
            // TODO: Validate collection sizes.
            // TODO: Set capacity?
            tangents = new Vector3[positions.Count];
            bitangents = new Vector3[positions.Count];

            // Calculate the vectors.
            for (int i = 0; i < indices.Count; i += 3)
            {
                VectorUtils.GenerateTangentBitangent(positions[indices[i]], positions[indices[i + 1]], positions[indices[i + 2]],
                    uvs[indices[i]], uvs[indices[i + 1]], uvs[indices[i + 2]], out Vector3 tangent, out Vector3 bitangent);

                tangents[indices[i]] += tangent;
                tangents[indices[i + 1]] += tangent;
                tangents[indices[i + 2]] += tangent;

                bitangents[indices[i]] += bitangent;
                bitangents[indices[i + 1]] += bitangent;
                bitangents[indices[i + 2]] += bitangent;
            }

            // Account for mirrored normal maps.
            for (int i = 0; i < bitangents.Length; i++)
            {
                bitangents[i] = VectorUtils.Orthogonalize(bitangents[i], normals[i]);
                bitangents[i] *= -1;
            }
        }
    }
}
