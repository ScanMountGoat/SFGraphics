using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace SFGraphicsGui
{
    static class ResourceTextFile
    {
        /// <summary>
        /// Reads all the text of the specified resource file into a string.
        /// </summary>
        /// <param name="resourceName">The name of the resource, including the namespace and sub directories.
        /// Ex: "SFGraphicsGui.Shaders.screenShader.vert"</param>
        /// <returns></returns>
        public static string GetFileText(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string result;
            string[] resources = assembly.GetManifestResourceNames();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}
