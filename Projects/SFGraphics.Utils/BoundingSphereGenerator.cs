﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFGraphics.Utils
{
    /// <summary>
    /// Contains methods to generate a bounding sphere for a collection of vertices.
    /// </summary>
    public static class BoundingSphereGenerator
    {
        /// <summary>
        /// Generates a close to optimal sphere that contains all of the given points
        /// within the sphere.
        /// Returns Vector4(center.Xyz, radius).
        /// </summary>
        /// <param name="points">The points that should be contained within the bounding sphere</param>
        /// <returns>Vector4(center.Xyz, radius)</returns>
        public static Vector4 GenerateBoundingSphere(IEnumerable<Vector3> points)
        {
            // The initial max/min should be the first point.
            Vector3 min = points.FirstOrDefault();
            Vector3 max = points.FirstOrDefault();

            // Find the corners of the bounding region.
            foreach (var point in points)
            {
                min = Vector3.ComponentMin(min, point);
                max = Vector3.ComponentMax(max, point);
            }

            Vector4 sphere = GetBoundingSphereFromRegion(min, max);
            sphere = AdjustBoundingSphere(points, sphere);
            return sphere;
        }

        // Optimization adapted from efficient bounding sphere algorithm by Jack Ritter
        // Example c implementation:
        // https://github.com/erich666/GraphicsGems/blob/master/gems/BoundSphere.c
        private static Vector4 AdjustBoundingSphere(IEnumerable<Vector3> points, Vector4 sphere)
        {
            foreach (var point in points)
            {
                // Avoid calculating square root for comparison.
                Vector3 delta = point - sphere.Xyz;
                float deltaLengthSquared = delta.LengthSquared;

                // If the point isn't in the bounding sphere, extend the sphere.
                if (deltaLengthSquared > (sphere.W * sphere.W))
                {
                    float newRadius = (float)Math.Sqrt(deltaLengthSquared);
                    sphere.W = (sphere.W + newRadius) / 2.0f;
                    float offset = newRadius - sphere.W;
                    sphere.Xyz = ((sphere.W * sphere.Xyz) + (offset * point)) / newRadius;
                }
            }

            return sphere;
        }

        /// <summary>
        /// Generates a single bounding sphere that contains all the given
        /// bounding spheres.
        /// </summary>
        /// <param name="boundingSpheres">The list of bounding sphere centers (xyz) and radii (w)</param>
        /// <returns>A single bounding sphere that contains <paramref name="boundingSpheres"/></returns>
        public static Vector4 GenerateBoundingSphere(IEnumerable<Vector4> boundingSpheres)
        {
            // The initial max/min should be the first point.
            Vector3 min = boundingSpheres.FirstOrDefault().Xyz - new Vector3(boundingSpheres.FirstOrDefault().W);
            Vector3 max = boundingSpheres.FirstOrDefault().Xyz + new Vector3(boundingSpheres.FirstOrDefault().W);

            // Calculate the end points using the center and radius
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
