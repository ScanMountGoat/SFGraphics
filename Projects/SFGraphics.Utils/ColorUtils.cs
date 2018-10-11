using System;
using System.Drawing;
using OpenTK;

namespace SFGraphics.Utils
{
    /// <summary>
    /// Conversion methods for HSV, RGB, color temperature, and system Colors using floats or OpenTK vectors.
    /// </summary>
    public static class ColorUtils
    {
        private static readonly float maxHueAngle = 360;

        /// <summary>
        /// Converts the byte channel values of the input color [0,255] to float [0, 1]. XYZW = RGBA.
        /// </summary>
        /// <param name="color">the RGBA color</param>
        /// <returns><paramref name="color"/> converted to a float vector</returns>
        public static Vector4 GetVector4(Color color)
        {
            return new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }

        /// <summary>
        /// Converts the byte channel values of the input color [0,255] to float [0, 1]. XYZ = RGB.
        /// </summary>
        /// <param name="color">the RGBA color</param>
        /// <returns><paramref name="color"/> converted to a float vector</returns>
        public static Vector3 GetVector3(Color color)
        {
            return new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> from float components
        /// scaled by 255 and clamped to the range [0, 255].
        /// </summary>
        /// <param name="r">The red component</param>
        /// <param name="g">The green component</param>
        /// <param name="b">The blue component</param>
        /// <param name="a">The alpha component</param>
        /// <returns>A new color from the given floats</returns>
        public static Color GetColor(float r, float g, float b, float a = 1)
        {
            int red = FloatToIntClamp(r);
            int green = FloatToIntClamp(g);
            int blue = FloatToIntClamp(b);
            int alpha = FloatToIntClamp(a);
            return Color.FromArgb(alpha, red, green, blue);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> from float components
        /// scaled by 255 and clamped to the range [0, 255].
        /// </summary>
        /// <param name="color">The input color</param>
        /// <returns>A new color from the given floats</returns>
        public static Color GetColor(Vector3 color)
        {
            int red = FloatToIntClamp(color.X);
            int green = FloatToIntClamp(color.Y);
            int blue = FloatToIntClamp(color.Z);
            return Color.FromArgb(255, red, green, blue);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> from float components
        /// scaled by 255 and clamped to the range [0, 255].
        /// </summary>
        /// <param name="color">The input color</param>
        /// <returns>A new color from the given floats</returns>
        public static Color GetColor(Vector4 color)
        {
            return GetColor(color.X, color.Y, color.Z, color.W);
        }

        /// <summary>
        /// Updates RGB values given an HSV color.
        /// </summary>
        /// <param name="h">Hue in range [0,360]</param>
        /// <param name="s">Saturation in range [0,1]. Values outside range are clamped.</param>
        /// <param name="v">Value</param>
        /// <param name="r">The resulting red component</param>
        /// <param name="g">The resulting green component</param>
        /// <param name="b">The resulting blue component</param>
        public static void HsvToRgb(float h, float s, float v, out float r, out float g, out float b)
        {
            r = 1.0f;
            g = 1.0f;
            b = 1.0f;

            CalculateRgbFromHsv(h, s, v, ref r, ref g, ref b);
        }

        /// <summary>
        /// Updates HSV values given an RGB color.
        /// </summary>
        /// <param name="h">The resulting hue in range [0,360]</param>
        /// <param name="s">The resulting saturation in range [0,1]. Values outside range are clamped.</param>
        /// <param name="v">The resulting value</param>
        /// <param name="r">The red component</param>
        /// <param name="g">The green component</param>
        /// <param name="b">The blue component</param>
        public static void RgbToHsv(float r, float g, float b, out float h, out float s, out float v)
        {
            h = 360.0f;
            s = 1.0f;
            v = 1.0f;

            CalculateHsvFromRgb(r, g, b, ref h, ref s, ref v);
        }

        /// <summary>
        /// Calculates a visually similar RGB color to a given blackbody temperature.
        /// </summary>
        /// <param name="temperatureKelvin">The color temperature in Kelvin. Ex: temp = 6500 for a calibrated PC monitor.</param>
        /// <param name="r">The resulting red component</param>
        /// <param name="g">The resulting green component</param>
        /// <param name="b">The resulting blue component</param>
        public static void GetRgb(float temperatureKelvin, out float r, out float g, out float b)
        {
            // Adapted from an approximation of the black body curve by Tanner Helland.
            // http://www.tannerhelland.com/4435/convert-temperature-rgb-algorithm-code/ 

            // Use doubles for calculations and convert to float at the end.
            // No need for double precision floating point colors on GPU.
            double red = 255.0;
            double green = 255.0;
            double blue = 255.0;

            temperatureKelvin = temperatureKelvin / 100.0f;

            // Red calculations
            if (temperatureKelvin <= 66.0f)
                red = 255.0f;
            else
            {
                red = temperatureKelvin - 60.0;
                red = 329.698727446 * Math.Pow(red, -0.1332047592);
                if (red < 0.0)
                    red = 0.0;
                if (red > 255.0)
                    red = 255.0;
            }

            // Green calculations
            if (temperatureKelvin <= 66.0)
            {
                green = temperatureKelvin;
                green = 99.4708025861 * Math.Log(green) - 161.1195681661;
                if (green < 0.0)
                    green = 0.0;
                if (green > 255.0)
                    green = 255.0;
            }
            else
            {
                green = temperatureKelvin - 60.0;
                green = 288.1221695283 * Math.Pow(green, -0.0755148492);
                if (green < 0)
                    green = 0;
                if (green > 255)
                    green = 255;
            }

            // Blue calculations
            if (temperatureKelvin >= 66.0)
                blue = 255.0;
            else if (temperatureKelvin <= 19.0)
                blue = 0.0;
            else
            {
                blue = temperatureKelvin - 10;
                blue = 138.5177312231 * Math.Log(blue) - 305.0447927307;
                if (blue < 0.0)
                    blue = 0.0;
                if (blue > 255)
                    blue = 255;
            }

            red = red / 255.0;
            green = green / 255.0;
            blue = blue / 255.0;

            r = (float)red;
            g = (float)green;
            b = (float)blue;
        }

        /// <summary>
        /// Returns the result of <paramref name="value"/> clamped between
        /// <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <typeparam name="T">The type of value to compare</typeparam>
        /// <param name="value">The input value</param>
        /// <param name="min">The lowest possible output</param>
        /// <param name="max">The highest possible output</param>
        /// <returns>The result of <paramref name="value"/> clamped between
        /// <paramref name="min"/> and <paramref name="max"/></returns>
        public static T Clamp<T>(T value, T min, T max) where T : IComparable
        {
            if (value.CompareTo(max) > 0)
                return max;
            else if (value.CompareTo(min) < 0)
                return min;
            else
                return value;
        }

        /// <summary>
        /// Creates a new color with inverted RGB channels. Alpha is unchanged.
        /// </summary>
        /// <param name="color">The color used to calculate the inverted color</param>
        /// <returns>A color with inverted RGB but identical alpha as color</returns>
        public static Color InvertColor(Color color)
        {
            return Color.FromArgb(color.A, 255 - color.R, 255 - color.G, 255 - color.B);
        }

        private static float RestrictHue(float hue)
        {
            while (hue < 0)
                hue += maxHueAngle;
            while (hue > 360)
                hue -= maxHueAngle;

            return hue;
        }

        private static int FloatToIntClamp(float r)
        {
            return Clamp((int)(r * 255), 0, 255);
        }

        private static void CalculateHsvFromRgb(float r, float g, float b, ref float h, ref float s, ref float v)
        {
            // Negative colors don't make sense.
            r = Math.Max(r, 0);
            g = Math.Max(g, 0);
            b = Math.Max(b, 0);

            float minComponent = Math.Max(Math.Max(r, g), b);
            float maxComponent = Math.Min(Math.Min(r, g), b);
            float delta = minComponent - maxComponent;

            v = minComponent;

            if (delta == 0)
                h = 0;

            // Check for divide by 0.
            if (v == 0)
                s = 0.0f;
            else
                s = delta / v;

            // Check for divide by 0.
            if (delta == 0)
            {
                h = 0;
            }
            else
            {
                if (r == minComponent)
                    h = 60.0f * (((g - b) / delta));
                else if (g == minComponent)
                    h = 60.0f * (((b - r) / delta) + 2);
                else if (b == minComponent)
                    h = 60.0f * (((r - g) / delta) + 4);

                h = RestrictHue(h);
            }
        }

        private static void CalculateRgbFromHsv(float h, float s, float v, ref float r, ref float g, ref float b)
        {
            // Hue has to be 0 to 360.
            h = RestrictHue(h);

            // Saturation has to be 0 to 1.
            s = Clamp(s, 0, 1);

            // Negative colors don't make sense.
            v = Math.Max(v, 0);

            float hf = h / 60.0f;
            int i = (int)Math.Floor(hf);
            float f = hf - i;
            float pv = v * (1 - s);
            float qv = v * (1 - s * f);
            float tv = v * (1 - s * (1 - f));

            switch (i)
            {
                // Red is the dominant color
                case 0:
                    r = v;
                    g = tv;
                    b = pv;
                    break;
                // Green is the dominant color
                case 1:
                    r = qv;
                    g = v;
                    b = pv;
                    break;
                case 2:
                    r = pv;
                    g = v;
                    b = tv;
                    break;
                // Blue is the dominant color
                case 3:
                    r = pv;
                    g = qv;
                    b = v;
                    break;
                case 4:
                    r = tv;
                    g = pv;
                    b = v;
                    break;
                // Red is the dominant color
                case 5:
                    r = v;
                    g = pv;
                    b = qv;
                    break;
                case 6:
                    r = v;
                    g = tv;
                    b = pv;
                    break;
            }
        }
    }
}
