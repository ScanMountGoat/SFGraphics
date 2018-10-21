using OpenTK;
using System.Collections.Generic;

namespace BoundingSphereTests
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
    }
}
