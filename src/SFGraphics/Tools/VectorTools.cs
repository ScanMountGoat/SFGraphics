using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// <para>
        /// Ex: <c>Vector3 tanOrthoToNrm = Orthogonalize(tan, nrm);</c>
        /// </para>
        /// </summary>
        /// <param name="target">The vector to normalize</param>
        /// <param name="source">The vector to normalize against</param>
        /// <returns></returns>
        public static Vector3 Orthogonalize(Vector3 target, Vector3 source)
        {
            return Vector3.Normalize(target - source * Vector3.Dot(source, target));
        }

        /// <summary>
        /// Generates a tangent vector <paramref name="s"/> and a bitangent vector
        /// <paramref name="t"/> for a triangle face. 
        /// <para>
        /// <para></para>
        /// <paramref name="s"/> and <paramref name="t"/> should be added to the existing tangent
        /// and bitangent value for each vertex in the triangle. Normalizing the final sum 
        /// averages the tangents and bitangents for smoother results.
        /// </para>
        /// </summary>
        /// <param name="v1">The position of the first vertex</param>
        /// <param name="v2">The position of the second vertex</param>
        /// <param name="v3">The position of the third vertex</param>
        /// <param name="uv1">The UV coordinates of the first vertex</param>
        /// <param name="uv2">The UV coordinates of the second vertex</param>
        /// <param name="uv3">The UV coordinates of the third vertex</param>
        /// <param name="s">The generated tangent vector</param>
        /// <param name="t">The generated bitangent vector</param>
        public static void GenerateTangentBitangent(Vector3 v1, Vector3 v2, Vector3 v3, 
                                                    Vector2 uv1, Vector2 uv2, Vector2 uv3, 
                                                    out Vector3 s, out Vector3 t)
        {
            float x1 = v2.X - v1.X;
            float x2 = v3.X - v1.X;
            float y1 = v2.Y - v1.Y;
            float y2 = v3.Y - v1.Y;
            float z1 = v2.Z - v1.Z;
            float z2 = v3.Z - v1.Z;

            float s1 = uv2.X - uv1.X;
            float s2 = uv3.X - uv1.X;
            float t1 = uv2.Y - uv1.Y;
            float t2 = uv3.Y - uv1.Y;

            float div = (s1 * t2 - s2 * t1);
            float r = 1.0f / div;

            // Fix +/- infinity from division by 0.
            if (r == float.PositiveInfinity || r == float.NegativeInfinity)
                r = 1.0f;

            float sX = t2 * x1 - t1 * x2;
            float sY = t2 * y1 - t1 * y2;
            float sZ = t2 * z1 - t1 * z2;
            s = new Vector3(sX, sY, sZ) * r;

            float tX = s1 * x2 - s2 * x1;
            float tY = s1 * y2 - s2 * y1;
            float tZ = s1 * z2 - s2 * z1;
            t = new Vector3(tX, tY, tZ) * r;

            // Prevents black tangents or bitangents due to having vertices with the same UV coordinates. 
            float delta = 0.00075f;
            bool sameU = (Math.Abs(uv1.X - uv2.X) < delta) && (Math.Abs(uv2.X - uv3.X) < delta);
            bool sameV = (Math.Abs(uv1.Y - uv2.Y) < delta) && (Math.Abs(uv2.Y - uv3.Y) < delta);
            if (sameU || sameV)
            {
                // Let's pick some arbitrary tangent vectors.
                s = new Vector3(1, 0, 0);
                t = new Vector3(0, 1, 0);
            }
        }
    }
}
