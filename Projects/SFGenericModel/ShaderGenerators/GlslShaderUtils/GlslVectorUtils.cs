﻿using SFGenericModel.VertexAttributes;
using System;

namespace SFGenericModel.ShaderGenerators.GlslShaderUtils
{
    internal static class GlslVectorUtils
    {
        public static readonly string[] vectorComponents = new string[] { "x", "y", "z", "w" };

        public static string ConstructVector(ValueCount targetValueCount, ValueCount sourceCount, string sourceName)
        {
            // TODO: What happens when target and source count are both 1?
            int targetCount = (int)targetValueCount;
            if (sourceCount == ValueCount.One)
                return $"vec{targetCount}({sourceName})";

            string components = GetMaxSharedComponents(sourceCount, targetValueCount);

            // Add 1's for the remaining parts of the constructor.
            string paddingValues = "";
            for (int i = components.Length; i < targetCount; i++)
            {
                paddingValues += ", 1";
            }

            return $"vec{targetCount}({sourceName}.{components}{paddingValues})";
        }

        public static string ConstructVector(ValueCount targetValueCount, VertexAttribute source)
        {
            return ConstructVector(targetValueCount, source.ValueCount, source.Name);
        }

        private static string GetMaxSharedComponents(ValueCount sourceCount, ValueCount targetCount)
        {
            string resultingComponents = "";

            int count = Math.Min((int)sourceCount, (int)targetCount);
            for (int i = 0; i < count; i++)
            {
                resultingComponents += vectorComponents[i];
            }

            return resultingComponents;
        }

        public static string GetSwizzle(TextureSwizzle swizzle)
        {
            switch (swizzle)
            {
                case TextureSwizzle.Rgb:
                    return "rgb";
                case TextureSwizzle.R:
                    return "rrr";
                case TextureSwizzle.G:
                    return "ggg";
                case TextureSwizzle.B:
                    return "bbb";
                case TextureSwizzle.A:
                    return "aaa";
                default:
                    return "rgb";
            }
        }
    }
}
