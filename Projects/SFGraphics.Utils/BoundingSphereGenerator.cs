using OpenTK;
using System;
using System.Collections.Generic;

namespace SFGraphics.Utils
{
    /// <summary>
    /// Contains methods to generate a bounding sphere for a collection of vertices.
    /// </summary>
    public static class BoundingSphereGenerator
    {
        /// <summary>
        /// The center is the average of the min and max values for X, Y, Z.
        /// The radius is calculated based on the smallest sphere that will contain all of <paramref name="vertexPositions"/>.
        /// Returns Vector4(center.Xyz, radius).
        /// </summary>
        /// <param name="vertexPositions"></param>
        /// <returns>Vector4(center.Xyz, radius)</returns>
        public static Vector4 GenerateBoundingSphere(List<Vector3> vertexPositions)
        {
            if (vertexPositions.Count == 0)
                return new Vector4(0);

            // Find the corners of the bounding region.
            Vector3 min = new Vector3(vertexPositions[0]);
            Vector3 max = new Vector3(vertexPositions[0]);

            foreach (var vertex in vertexPositions)
            {
                min = Vector3.ComponentMin(min, vertex);
                max = Vector3.ComponentMax(max, vertex);
            }

            return GetBoundingSphereFromRegion(min, max);
        }

        /// <summary>
        /// Generates a single bounding sphere that contains all the given
        /// bounding spheres.
        /// </summary>
        /// <param name="boundingSpheres">The list of bounding sphere centers (xyz) and radii (w)</param>
        /// <returns>A single bounding sphere that contains <paramref name="boundingSpheres"/></returns>
        public static Vector4 GenerateBoundingSphere(List<Vector4> boundingSpheres)
        {
            if (boundingSpheres.Count == 0)
                return new Vector4(0);

            // Calculate the end points using the center and radius
            Vector3 min = boundingSpheres[0].Xyz - new Vector3(boundingSpheres[0].W);
            Vector3 max = boundingSpheres[0].Xyz + new Vector3(boundingSpheres[0].W);

            foreach (var sphere in boundingSpheres)
            {
                min = Vector3.ComponentMin(min, sphere.Xyz - new Vector3(sphere.W));
                max = Vector3.ComponentMax(max, sphere.Xyz + new Vector3(sphere.W));
            }

            return GetBoundingSphereFromSpheres(min, max);
        }

        private static Vector4 GetBoundingSphereFromSpheres(Vector3 min, Vector3 max)
        {
            Vector3 lengths = max - min;
            float maxLength = Math.Max(lengths.X, Math.Max(lengths.Y, lengths.Z));
            Vector3 center = (max + min) / 2.0f;
            float radius = maxLength / 2.0f;
            return new Vector4(center, radius);
        }

        private static float CalculateRadius(float horizontalLeg, float verticalLeg)
        {
            return (float)Math.Sqrt((horizontalLeg * horizontalLeg) + (verticalLeg * verticalLeg));
        }

        private static Vector4 GetBoundingSphereFromRegion(Vector3 min, Vector3 max)
        {
            // The radius should be the hypotenuse of the triangle.
            // This ensures the sphere contains all points.
            Vector3 lengths = max - min;
            float horizontalLeg = lengths.X / 2.0f;
            float verticalLeg = lengths.Y / 2.0f;

            Vector3 center = (max + min) / 2.0f;
            float radius = CalculateRadius(horizontalLeg, verticalLeg);

            return new Vector4(center, radius);
        }
    }
}
