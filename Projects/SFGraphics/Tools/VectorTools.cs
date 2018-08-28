using System;
using OpenTK;

namespace SFGraphics.Tools
{
    /// <summary>
    /// Utility methods for normals, tangents, bitangents, and angle conversions
    /// </summary>
    public static class VectorTools
    {
        /// <summary>
        /// Converts <paramref name="radians"/> to degrees.
        /// </summary>
        /// <param name="radians">The number of radians</param>
        /// <returns>The angle converted to degrees</returns>
        public static double GetDegrees(double radians)
        {
            return radians / Math.PI * 180.0;
        }

        /// <summary>
        /// Converts <paramref name="degrees"/> to radians.
        /// </summary>
        /// <param name="degrees">The number of degrees</param>
        /// <returns>The angle converted to radians</returns>
        public static double GetRadians(double degrees)
        {
            return degrees / 180.0 * Math.PI;
        }

        /// <summary>
        /// Uses the Gran-Schmidt method for orthogonalizing a vector to another vector.
        /// The resulting vector is normalized.    
        /// <para></para>
        /// <para>
        /// Ex: <c>Vector3 tanOrthoToNrm = Orthogonalize(tan, nrm);</c>
        /// </para>
        /// </summary>
        /// <param name="target">The vector to normalize</param>
        /// <param name="source">The vector to normalize against</param>
        /// <returns><paramref name="target"/> orthogonalized to <paramref name="source"/></returns>
        public static Vector3 Orthogonalize(Vector3 target, Vector3 source)
        {
            return Vector3.Normalize(target - source * Vector3.Dot(source, target));
        }

        /// <summary>
        /// Generates a tangent vector <paramref name="tangent"/> and a bitangent vector
        /// <paramref name="bitangent"/> for a triangle face. 
        /// If the three vertices have the same UVs or position, <paramref name="tangent"/> is set to (1, 0, 0)
        /// and <paramref name="bitangent"/> is set to (0, 1, 0). This prevents black shading artifacts.
        /// <para></para><para></para>
        /// <paramref name="tangent"/> and <paramref name="bitangent"/> should be added to the existing tangent
        /// and bitangent value for each vertex in the triangle. Normalizing the final sum 
        /// averages the tangents and bitangents for smoother results.
        /// </summary>
        /// <param name="v1">The position of the first vertex</param>
        /// <param name="v2">The position of the second vertex</param>
        /// <param name="v3">The position of the third vertex</param>
        /// <param name="uv1">The UV coordinates of the first vertex</param>
        /// <param name="uv2">The UV coordinates of the second vertex</param>
        /// <param name="uv3">The UV coordinates of the third vertex</param>
        /// <param name="tangent">The generated tangent vector</param>
        /// <param name="bitangent">The generated bitangent vector</param>
        public static void GenerateTangentBitangent(Vector3 v1, Vector3 v2, Vector3 v3, 
                                                    Vector2 uv1, Vector2 uv2, Vector2 uv3, 
                                                    out Vector3 tangent, out Vector3 bitangent)
        {
            Vector3 posA = v2 - v1;
            Vector3 posB = v3 - v1;

            Vector2 uvA = uv2 - uv1;
            Vector2 uvB = uv3 - uv1;

            // Prevent black tangents/bitangents for vertices with 
            // the same UV coordinates or position. 
            float delta = 0.00075f;
            bool sameU = (Math.Abs(uvA.X) < delta) && (Math.Abs(uvB.X) < delta);
            bool sameV = (Math.Abs(uvA.Y) < delta) && (Math.Abs(uvB.Y) < delta);
            bool sameX = (Math.Abs(posA.X) < delta) && (Math.Abs(posB.X) < delta);
            bool sameY = (Math.Abs(posA.Y) < delta) && (Math.Abs(posB.Y) < delta);
            bool sameZ = (Math.Abs(posA.Z) < delta) && (Math.Abs(posB.Z) < delta);

            if (sameU || sameV || sameX || sameY || sameZ)
            {
                // HACK: Let's pick some arbitrary tangent vectors.
                tangent = new Vector3(1, 0, 0);
                bitangent = new Vector3(0, 1, 0);
                return;
            }

            float div = (uvA.X * uvB.Y - uvB.X * uvA.Y);
            float r = 1.0f / div;

            // Fix +/- infinity from division by 0.
            if (div == 0)
                r = 1.0f;

            float sX = uvB.Y * posA.X - uvA.Y * posB.X;
            float sY = uvB.Y * posA.Y - uvA.Y * posB.Y;
            float sZ = uvB.Y * posA.Z - uvA.Y * posB.Z;
            tangent = new Vector3(sX, sY, sZ) * r;
            
            float tX = uvA.X * posB.X - uvB.X * posA.X;
            float tY = uvA.X * posB.Y - uvB.X * posA.Y;
            float tZ = uvA.X * posB.Z - uvB.X * posA.Z;
            bitangent = new Vector3(tX, tY, tZ) * r;
        }

        /// <summary>
        /// Calculates the face normal of a triangle. The result is not normalized.
        /// A triangle facing the camera will have a positive normal when 
        /// the verts are ordered counter-clockwise.
        /// </summary>
        /// <param name="v1">The position of the first vertex</param>
        /// <param name="v2">The position of the second vertex</param>
        /// <param name="v3">The position of the third vertex</param>
        /// <returns>The calculated face normal</returns>
        public static  Vector3 CalculateNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Vector3 U = v2 - v1;
            Vector3 V = v3 - v1;

            // Don't normalize here, so surface area can be calculated. 
            return Vector3.Cross(U, V);
        }
    }
}
