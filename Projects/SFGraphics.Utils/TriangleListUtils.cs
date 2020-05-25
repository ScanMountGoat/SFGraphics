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
                throw new System.ArgumentOutOfRangeException(nameof(normals), "Vector source lengths do not match.");

            if (uvs.Count != positions.Count)
                throw new System.ArgumentOutOfRangeException(nameof(uvs), "Vector source lengths do not match.");

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

            for (int i = 0; i < tangents.Length; i++)
            {
                tangents[i].Normalize();
            }

            // Account for mirrored normal maps.
            for (int i = 0; i < bitangents.Length; i++)
            {
                bitangents[i] = VectorUtils.Orthogonalize(bitangents[i], normals[i]);
                bitangents[i] *= -1;
            }
        }

        /// <summary>
        /// Calculates normalized, smooth tangents.
        /// Bitangents can be generated as <code>cross(N.xyz, T.xyz) * T.w</code>
        /// </summary>
        /// <param name="positions">The vertex positions</param>
        /// <param name="normals">The vertex normals</param>
        /// <param name="uvs">The vertex texture coordinates</param>
        /// <param name="indices">The indices used to define the triangle faces</param>
        /// <param name="tangents">The newly generated tangents</param>
        public static void CalculateTangents(IList<Vector3> positions, IList<Vector3> normals, IList<Vector2> uvs, IList<int> indices, out Vector4[] tangents)
        {
            CalculateTangentsBitangents(positions, normals, uvs, indices, out Vector3[] tangentsVec3, out Vector3[] bitangentsVec3);

            tangents = new Vector4[tangentsVec3.Length];
            // Account for mirrored normal maps.
            // Instead of storing the bitangent, store if the bitangent should be flipped in W.
            for (int i = 0; i < tangentsVec3.Length; i++)
            {
                tangents[i].Xyz = tangentsVec3[i];
                tangents[i].W = VectorUtils.CalculateTangentW(normals[i], tangentsVec3[i], bitangentsVec3[i]);
            }
        }

        /// <summary>
        /// Calculates normalized, smooth normals for the given vertex positions.
        /// </summary>
        /// <param name="positions">The vertex positions</param>
        /// <param name="normals">The vertex normals</param>
        /// <param name="indices">The indices used to define the triangle faces</param>
        public static void CalculateSmoothNormals(IList<Vector3> positions, IList<int> indices, out Vector3[] normals)
        {
            normals = new Vector3[positions.Count];

            // Calculate the vectors.
            for (int i = 0; i < indices.Count; i += 3)
            {
                var normal = VectorUtils.CalculateNormal(positions[indices[i]], positions[indices[i + 1]], positions[indices[i + 2]]);

                normals[indices[i]] += normal;
                normals[indices[i + 1]] += normal;
                normals[indices[i + 2]] += normal;
            }

            // Normalize the result.
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i].Normalize();
            }
        }
    }
}
