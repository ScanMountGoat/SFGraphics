using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace SFShapes
{
    /// <summary>
    /// Contains methods to generate vertex positions for drawing 
    /// geometric primitives as triangle lists.
    /// </summary>
    public static class ShapeGenerator
    {
        /// <summary>
        /// The minimum precision that can generate a valid sphere.
        /// </summary>
        public static readonly int minSpherePrecision = 8;

        /// <summary>
        /// Creates a triangle list cube with side length <paramref name="scale"/>.
        /// </summary>
        /// <param name="center">The center of the shape</param>
        /// <param name="scale">The side length of each cube face</param>
        /// <returns></returns>
        public static Tuple<List<Vector3>, PrimitiveType> GetCubePositions(Vector3 center, float scale)
        {
            return GetRectangularPrismPositions(center, scale, scale, scale);
        }

        /// <summary>
        /// Creates a triangle list rectangular prism of dimensions <paramref name="scaleX"/>
        /// * <paramref name="scaleY"/> * <paramref name="scaleZ"/>.
        /// </summary>
        /// <param name="center">The center of the shape</param>
        /// <param name="scaleX">The total width of the shape</param>
        /// <param name="scaleY">The total height of the shape</param>
        /// <param name="scaleZ">The total depth of the shape</param>
        /// <returns></returns>
        public static Tuple<List<Vector3>, PrimitiveType> GetRectangularPrismPositions(Vector3 center, float scaleX, float scaleY, float scaleZ)
        {
            scaleX = scaleX * 0.5f;
            scaleY = scaleY * 0.5f;
            scaleZ = scaleZ * 0.5f;

            List<Vector3> positions = new List<Vector3>()
            {
                new Vector3(center.X + -scaleX, center.Y + -scaleY, center.Z + -scaleZ),
                new Vector3(center.X + -scaleX, center.Y + -scaleY, center.Z +  scaleZ),
                new Vector3(center.X + -scaleX, center.Y +  scaleY, center.Z +  scaleZ),

                new Vector3(center.X +  scaleX, center.Y +  scaleY, center.Z + -scaleZ),
                new Vector3(center.X + -scaleX, center.Y + -scaleY, center.Z + -scaleZ),
                new Vector3(center.X + -scaleX, center.Y +  scaleY, center.Z + -scaleZ),

                new Vector3(center.X +  scaleX, center.Y + -scaleY, center.Z +  scaleZ),
                new Vector3(center.X + -scaleX, center.Y + -scaleY, center.Z + -scaleZ),
                new Vector3(center.X +  scaleX, center.Y + -scaleY, center.Z + -scaleZ),

                new Vector3(center.X +  scaleX, center.Y +  scaleY, center.Z + -scaleZ),
                new Vector3(center.X +  scaleX, center.Y + -scaleY, center.Z + -scaleZ),
                new Vector3(center.X + -scaleX, center.Y + -scaleY, center.Z + -scaleZ),

                new Vector3(center.X + -scaleX, center.Y + -scaleY, center.Z + -scaleZ),
                new Vector3(center.X + -scaleX, center.Y +  scaleY, center.Z +  scaleZ),
                new Vector3(center.X + -scaleX, center.Y +  scaleY, center.Z + -scaleZ),

                new Vector3(center.X +  scaleX, center.Y + -scaleY, center.Z +  scaleZ),
                new Vector3(center.X + -scaleX, center.Y + -scaleY, center.Z +  scaleZ),
                new Vector3(center.X + -scaleX, center.Y + -scaleY, center.Z + -scaleZ),

                new Vector3(center.X + -scaleX, center.Y +  scaleY, center.Z +  scaleZ),
                new Vector3(center.X + -scaleX, center.Y + -scaleY, center.Z +  scaleZ),
                new Vector3(center.X +  scaleX, center.Y + -scaleY, center.Z +  scaleZ),

                new Vector3(center.X +  scaleX, center.Y +  scaleY, center.Z +  scaleZ),
                new Vector3(center.X +  scaleX, center.Y + -scaleY, center.Z + -scaleZ),
                new Vector3(center.X +  scaleX, center.Y +  scaleY, center.Z + -scaleZ),

                new Vector3(center.X +  scaleX, center.Y + -scaleY, center.Z + -scaleZ),
                new Vector3(center.X +  scaleX, center.Y +  scaleY, center.Z +  scaleZ),
                new Vector3(center.X +  scaleX, center.Y + -scaleY, center.Z +  scaleZ),

                new Vector3(center.X +  scaleX, center.Y +  scaleY, center.Z +  scaleZ),
                new Vector3(center.X +  scaleX, center.Y +  scaleY, center.Z + -scaleZ),
                new Vector3(center.X + -scaleX, center.Y +  scaleY, center.Z + -scaleZ),

                new Vector3(center.X +  scaleX, center.Y +  scaleY, center.Z +  scaleZ),
                new Vector3(center.X + -scaleX, center.Y +  scaleY, center.Z + -scaleZ),
                new Vector3(center.X + -scaleX, center.Y +  scaleY, center.Z +  scaleZ),

                new Vector3(center.X +  scaleX, center.Y +  scaleY, center.Z +  scaleZ),
                new Vector3(center.X + -scaleX, center.Y +  scaleY, center.Z +  scaleZ),
                new Vector3(center.X +  scaleX, center.Y + -scaleY, center.Z +  scaleZ)
            };

            return new Tuple<List<Vector3>, PrimitiveType>(positions, PrimitiveType.Triangles);
        }

        /// <summary>
        /// Creates a subdivided triangle strip sphere.
        /// </summary>
        /// <param name="center">The center of the sphere</param>
        /// <param name="radius">The radius of the sphere</param>
        /// <param name="precision">The amount of subdivisions</param>
        /// <returns>Vertices for a triangle strip sphere</returns>
        /// <exception cref="ArgumentOutOfRangeException">Radius is <c>0</c> or negative. 
        /// Precision is less than <see cref="minSpherePrecision"/>.</exception>
        public static Tuple<List<Vector3>, PrimitiveType> GetSpherePositions(Vector3 center, float radius, int precision)
        {
            // Adapted to modern OpenGL from method in OpenTKContext.cs
            // https://github.com/libertyernie/brawltools/blob/ba2e029a51224f83e77fd9332c969c99fe092f33/BrawlLib/OpenGL/TKContext.cs#L487-L539

            if (radius <= 0)
                throw new ArgumentOutOfRangeException(ShapeExceptionMessages.invalidSphereRadius);

            if (precision < minSpherePrecision)
                throw new ArgumentOutOfRangeException(ShapeExceptionMessages.invalidSpherePrecision);

            List<Vector3> positions = new List<Vector3>();

            float halfPI = (float)(Math.PI * 0.5);
            float oneOverPrecision = 1.0f / precision;
            float twoPIOverPrecision = (float)(Math.PI * 2.0 * oneOverPrecision);

            float theta1 = 0;
            float theta2 = 0;
            float theta3 = 0;

            for (int j = 0; j < precision / 2; j++)
            {
                theta1 = (j * twoPIOverPrecision) - halfPI;
                theta2 = ((j + 1) * twoPIOverPrecision) - halfPI;

                for (int i = 0; i <= precision; i++)
                {
                    theta3 = i * twoPIOverPrecision;
                    CreateFirstSphereVertex(center, radius, positions, theta2, theta3);
                    CreateSecondSphereVertex(center, radius, positions, theta1, theta3);
                }
            }

            return new Tuple<List<Vector3>, PrimitiveType>(positions, PrimitiveType.TriangleStrip);
        }

        private static void CreateSecondSphereVertex(Vector3 center, float radius, List<Vector3> positions, float theta1, float theta3)
        {
            Vector3 normal2;
            normal2.X = (float)(Math.Cos(theta1) * Math.Cos(theta3));
            normal2.Y = (float)Math.Sin(theta1);
            normal2.Z = (float)(Math.Cos(theta1) * Math.Sin(theta3));

            Vector3 position2;
            position2.X = center.X + radius * normal2.X;
            position2.Y = center.Y + radius * normal2.Y;
            position2.Z = center.Z + radius * normal2.Z;
            positions.Add(position2);
        }

        private static void CreateFirstSphereVertex(Vector3 center, float radius, List<Vector3> positions, float theta2, float theta3)
        {
            Vector3 normal;
            normal.X = (float)(Math.Cos(theta2) * Math.Cos(theta3));
            normal.Y = (float)Math.Sin(theta2);
            normal.Z = (float)(Math.Cos(theta2) * Math.Sin(theta3));
            Vector3 position;
            position.X = center.X + radius * normal.X;
            position.Y = center.Y + radius * normal.Y;
            position.Z = center.Z + radius * normal.Z;
            positions.Add(position);
        }
    }
}
