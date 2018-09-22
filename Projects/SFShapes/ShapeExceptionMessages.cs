using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFShapes
{
    internal static class ShapeExceptionMessages
    {
        public static readonly string invalidSphereRadius = "Radius must be positive and non zero.";

        public static readonly string invalidSpherePrecision = "Precision must be greater than the required minimum.";
    }
}
