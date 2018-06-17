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
    }
}
