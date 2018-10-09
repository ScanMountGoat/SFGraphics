using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GlUtils
{
    /// <summary>
    /// This class contains methods for checking the availability of OpenGL extensions.
    /// Extensions that aren't core for the current OpenGL version may not be available on all graphics processors.
    /// For example, an application can target OpenGL 3.30 but still use newer features if available.
    /// </summary>
    public static class OpenGLExtensions
    {
        private static HashSet<string> supportedExtensions = new HashSet<string>();

        /// <summary>
        /// Returns true if an extension is available. 
        /// There is no need to check core extensions for the current OpenGL version.
        /// Initializes the list of current extensions again if empty.
        /// </summary>
        /// <param name="extensionName">The name of the OpenGL extension. 
        /// Ex: GL_KHR_debug. Names are not case sensitive. </param>
        /// <returns>True if the extension is available</returns>
        public static bool IsAvailable(string extensionName)
        {
            if (supportedExtensions.Count == 0)
                InitializeCurrentExtensions();

            // Ignore case.
            return supportedExtensions.Contains(extensionName.ToLower());
        }

        /// <summary>
        /// Clears and initializes the available extensions for searching with <see cref="IsAvailable(string)"/>.
        /// </summary>
        public static void InitializeCurrentExtensions()
        {
            string[] extensions = GetAllExtensions();

            supportedExtensions.Clear();
            foreach (string extension in extensions)
            {
                string extensionName = extension.ToLower();
                if (!supportedExtensions.Contains(extensionName))
                    supportedExtensions.Add(extensionName);
            }
        }

        private static string[] GetAllExtensions()
        {
            string allExtensions = GL.GetString(StringName.Extensions);
            string[] extensions = allExtensions.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return extensions;
        }
    }
}
