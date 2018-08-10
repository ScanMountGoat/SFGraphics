﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Textures
{
    internal static class CompressedBlockSize
    {
        /// <summary>
        /// A string is used instead of the enum because there are Ext versions of 
        /// some enum values with identical associated integers.
        /// </summary>
        public static readonly Dictionary<string, int> blockSizeByFormat = new Dictionary<string, int>
        {
            { "CompressedR11Eac",                       8},
            { "CompressedRed",                          8}, // generic compressed format
            { "CompressedRedRgtc1",                     8},
            { "CompressedRedRgtc1Ext",                  8},
            { "CompressedRg",                           8}, // generic compressed format
            { "CompressedRg11Eac",                     16},
            { "CompressedRgRgtc2",                     16},
            { "CompressedRgb",                          8},
            { "CompressedRgb8Etc2",                     8},
            { "CompressedRgb8PunchthroughAlpha1Etc2",   8},
            { "CompressedRgbBptcSignedFloat",          16},
            { "CompressedRgbBptcUnsignedFloat",        16},
            { "CompressedRgbS3tcDxt1Ext",               8},
            { "CompressedRgba",                         8},
            { "CompressedRgba8Etc2Eac",                16},
            { "CompressedRgbaBptcUnorm",               16},
            { "CompressedRgbaS3tcDxt1Ext",              8},
            { "CompressedRgbaS3tcDxt3Ext",             16},
            { "CompressedRgbaS3tcDxt5Ext",             16},
            { "CompressedSignedR11Eac",                 8},
            { "CompressedSignedRedRgtc1",               8},
            { "CompressedSignedRedRgtc1Ext",            8},
            { "CompressedSignedRg11Eac",               16},
            { "CompressedSignedRgRgtc2",               16},
            { "CompressedSrgb",                         8}, // generic compressed format
            { "CompressedSrgb8Alpha8Etc2Eac",          16},
            { "CompressedSrgb8Etc2",                    8},
            { "CompressedSrgb8PunchthroughAlpha1Etc2",  8},
            { "CompressedSrgbAlpha",                    8}, // generic compressed format
            { "CompressedSrgbAlphaBptcUnorm",          16},
            { "CompressedSrgbAlphaS3tcDxt1Ext",         8},
            { "CompressedSrgbAlphaS3tcDxt3Ext",        16},
            { "CompressedSrgbAlphaS3tcDxt5Ext",        16},
            { "CompressedSrgbS3tcDxt1Ext",              8}
        };
    }
}