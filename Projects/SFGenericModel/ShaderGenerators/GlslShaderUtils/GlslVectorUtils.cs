﻿using SFGenericModel.VertexAttributes;
using System;

namespace SFGenericModel.ShaderGenerators.GlslShaderUtils
{
    internal static class GlslVectorUtils
    {
        public static readonly string[] vectorComponents = new string[] { "x", "y", "z", "w" };

        public static string ConstructVector(ValueCount targetCount, ValueCount sourceCount, string sourceName)
        {
            if (sourceCount == ValueCount.One)
                return $"vec{(int)targetCount}({sourceName})";

            string components = GetMaxSharedComponents(sourceCount, targetCount);

            // Add 1's for the remaining parts of the constructor.
            string paddingValues = "";
            for (int i = components.Length; i < (int)targetCount; i++)
            {
                paddingValues += ", 1";
            }

            return $"vec{(int)targetCount}({sourceName}.{components}{paddingValues})";
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
    }
}