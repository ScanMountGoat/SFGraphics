using OpenTK;
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
        /// 
        /// </summary>
        /// <param name="scale">The side length of each cube face</param>
        /// <returns></returns>
        public static List<Vector3> GetCubePositions(float scale)
        {
            return GetRectangularPrismPositions(scale, scale, scale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scaleX">The total width of the shape</param>
        /// <param name="scaleY">The total height of the shape</param>
        /// <param name="scaleZ">The total depth of the shape</param>
        /// <returns></returns>
        public static List<Vector3> GetRectangularPrismPositions(float scaleX, float scaleY, float scaleZ)
        {
            scaleX = scaleX * 0.5f;
            scaleY = scaleY * 0.5f;
            scaleZ = scaleZ * 0.5f;

            return new List<Vector3>()
            {
                new Vector3(-scaleX, -scaleY, -scaleZ),
                new Vector3(-scaleX, -scaleY,  scaleZ),
                new Vector3(-scaleX,  scaleY,  scaleZ),
                new Vector3( scaleX,  scaleY, -scaleZ),
                new Vector3(-scaleX, -scaleY, -scaleZ),
                new Vector3(-scaleX,  scaleY, -scaleZ),
                new Vector3( scaleX, -scaleY,  scaleZ),
                new Vector3(-scaleX, -scaleY, -scaleZ),
                new Vector3( scaleX, -scaleY, -scaleZ),
                new Vector3( scaleX,  scaleY, -scaleZ),
                new Vector3( scaleX, -scaleY, -scaleZ),
                new Vector3(-scaleX, -scaleY, -scaleZ),
                new Vector3(-scaleX, -scaleY, -scaleZ),
                new Vector3(-scaleX,  scaleY,  scaleZ),
                new Vector3(-scaleX,  scaleY, -scaleZ),
                new Vector3 (scaleX, -scaleY,  scaleZ),
                new Vector3(-scaleX, -scaleY,  scaleZ),
                new Vector3(-scaleX, -scaleY, -scaleZ),
                new Vector3(-scaleX,  scaleY,  scaleZ),
                new Vector3(-scaleX, -scaleY,  scaleZ),
                new Vector3( scaleX, -scaleY,  scaleZ),
                new Vector3( scaleX,  scaleY,  scaleZ),
                new Vector3( scaleX, -scaleY, -scaleZ),
                new Vector3( scaleX,  scaleY, -scaleZ),
                new Vector3( scaleX, -scaleY, -scaleZ),
                new Vector3( scaleX,  scaleY,  scaleZ),
                new Vector3( scaleX, -scaleY,  scaleZ),
                new Vector3( scaleX,  scaleY,  scaleZ),
                new Vector3( scaleX,  scaleY, -scaleZ),
                new Vector3(-scaleX,  scaleY, -scaleZ),
                new Vector3( scaleX,  scaleY,  scaleZ),
                new Vector3(-scaleX,  scaleY, -scaleZ),
                new Vector3(-scaleX,  scaleY,  scaleZ),
                new Vector3( scaleX,  scaleY,  scaleZ),
                new Vector3(-scaleX,  scaleY,  scaleZ),
                new Vector3( scaleX, -scaleY,  scaleZ)
            };
        }
    }
}
