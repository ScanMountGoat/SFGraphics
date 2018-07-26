using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;


namespace SFGraphics.Tools
{
    /// <summary>
    /// This class contains methods for checking the availability of OpenGL extensions.
    /// Extensions that aren't core for the current OpenGL version may not be available on all graphics processors.
    /// For example, an application can target OpenGL 3.30 but still use newer features if available.
    /// </summary>
    public static class OpenTKExtensionTools
    {
        private static HashSet<string> supportedExtensions = new HashSet<string>();

        /// <summary>
        /// Returns true if an extension is available. 
        /// There is no need to check core extensions for the current OpenGL version.
        /// </summary>
        /// <param name="extensionName">The name of the OpenGL extension. 
        /// Names are case sensitive Ex: GL_KHR_debug</param>
        /// <returns></returns>
        public static bool IsAvailable(string extensionName)
        {
            if (supportedExtensions.Count == 0)
                InitializeExtensions();

            return supportedExtensions.Contains(extensionName);
        }

        private static void InitializeExtensions()
        {
            string[] extensions = GetAllExtensions();

            supportedExtensions.Clear();
            foreach (string extension in extensions)
            {
                if (!supportedExtensions.Contains(extension))
                    supportedExtensions.Add(extension);
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
