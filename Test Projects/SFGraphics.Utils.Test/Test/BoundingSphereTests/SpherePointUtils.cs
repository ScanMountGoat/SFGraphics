using OpenTK;
using System.Collections.Generic;

namespace SFGraphics.Utils.Test.BoundingSphereTests
{
    internal static class SpherePointUtils
    {
        public static bool SphereContainsPoints(Vector4 sphere, List<Vector3> points)
        {
            foreach (var point in points)
            {
                if ((point - sphere.Xyz).LengthSquared > (sphere.W * sphere.W))
                    return false;
            }
            return true;
        }

        public static bool SphereContainsSpheres(Vector4 outer, List<Vector4> inner)
        {
            foreach (var sphere in inner)
            {
                // Check if distance between centers is less than sum of radii.
                if (Vector3.Distance(sphere.Xyz, outer.Xyz) > (outer.W + sphere.W))
                    return false;
            }
            return true;
        }
    }
}
