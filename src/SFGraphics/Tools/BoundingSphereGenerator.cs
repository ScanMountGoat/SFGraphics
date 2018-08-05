﻿using System;
using System.Collections.Generic;
using OpenTK;


namespace SFGraphics.Tools
{
    /// <summary>
    /// Generates bounding spheres. 
    /// This can be used with <see cref="SFGraphics.Cameras.Camera.FrameBoundingSphere(Vector3, float, float)"/> to make models visible in the viewport.
    /// </summary>
    public static class BoundingSphereGenerator
    {
        /// <summary>
        /// Returns Vector4(center.Xyz, radius).
        /// </summary>
        /// <param name="vertexPositions"></param>
        /// <returns>Vector4(center.Xyz, radius)</returns>
        public static Vector4 GenerateBoundingSphere(List<Vector3> vertexPositions)
        {
            if (vertexPositions.Count == 0)
                return new Vector4(0);
            
            // Compares the vertexPositions to the first vertex.
            float minX = vertexPositions[0].X;
            float maxX = vertexPositions[0].X;

            float minY = vertexPositions[0].Y;
            float maxY = vertexPositions[0].Y;

            float minZ = vertexPositions[0].Z;
            float maxZ = vertexPositions[0].Z;

            // Finds the corners of the bounding box.
            // This will be relatively slow for denser models.
            foreach (var vertex in vertexPositions)
            {
                minX = Math.Min(minX, vertex.X);
                maxX = Math.Max(maxX, vertex.X);

                minY = Math.Min(minY, vertex.Y);
                maxY = Math.Max(maxY, vertex.Y);

                minZ = Math.Min(minZ, vertex.Z);
                maxZ = Math.Max(maxZ, vertex.Z);
            }

            // Finds the smallest cube that will hold the entire model.
            float xLength = maxX - minX;
            float yLength = maxY - minY;
            float zLength = maxZ - minZ;
            float maxLength = Math.Max(Math.Max(xLength, yLength), zLength);

            // The center is the average in each direction.
            Vector3 center = new Vector3(0);
            center.X = (maxX + minX) / 2.0f;
            center.Y = (maxY + minY) / 2.0f;
            center.Z = (maxZ + minZ) / 2.0f;

            // The radius is half the diameter.
            float radius = maxLength / 2.0f;

            return new Vector4(center, radius);
        }
    }
}
