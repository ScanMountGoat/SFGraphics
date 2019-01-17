using System.Collections.Generic;
using OpenTK;

namespace SFGraphics.Utils
{
    /// <summary>
    /// Contains methods for calculating vertex properties for PrimitiveType.Triangles.
    /// </summary>
    public static class TriangleListUtils
    {
        /// <summary>
        /// Calculates normalized, smooth tangents and bitangents for the given vertex data. Bitangents are adjusted to account for mirrored UVs.
        /// </summary>
        /// <param name="positions">The vertex positions</param>
        /// <param name="normals">The vertex normals</param>
        /// <param name="uvs">The vertex texture coordinates</param>
        /// <param name="indices">The indices used to define the triangle faces</param>
        /// <param name="tangents">The newly generated tangents</param>
        /// <param name="bitangents">The newly generated bitangents</param>
        public static void CalculateTangentsBitangents(IList<Vector3> positions, IList<Vector3> normals, IList<Vector2> uvs, IList<int> indices, out Vector3[] tangents, out Vector3[] bitangents)
        {
            if (normals.Count != positions.Count)
                throw new System.ArgumentOutOfRangeException("normals", "Vector source lengths do not match.");

            if (uvs.Count != positions.Count)
                throw new System.ArgumentOutOfRangeException("uvs", "Vector source lengths do not match.");

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
