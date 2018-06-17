using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
