using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace SFGraphics.GlUtils
{
    /// <summary>
    /// This class contains methods for checking the availability of OpenGL extensions.
    /// Extensions that aren't core for the current OpenGL version may not be available on all graphics processors.
    /// For example, an application can target OpenGL 3.30 but still use newer features if available.
    /// </summary>
    public static class OpenGLExtensions
    {
        private static readonly HashSet<string> supportedExtensions = new HashSet<string>();

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
            // Using GL.GetString to retrieve all extensions is deprecated. 
            int extensionCount = GL.GetInteger(GetPName.NumExtensions);
            string[] extensions = new string[extensionCount];
            for (int i = 0; i < extensionCount; i++)
            {
                extensions[i] = GL.GetString(StringNameIndexed.Extensions, i);
            }
            return extensions;
        }
    }
}
