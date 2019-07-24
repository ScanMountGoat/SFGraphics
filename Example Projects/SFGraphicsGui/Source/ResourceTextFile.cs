using System.IO;
using System.Reflection;

namespace SFGraphicsGui
{
    public static class ResourceTextFile
    {
        /// <summary>
        /// Reads all the text of the specified resource file into a string.
        /// </summary>
        /// <param name="resourceName">The name of the resource, including the namespace and sub directories.
        /// Ex: "SFGraphicsGui.Shaders.screenShader.vert"</param>
        /// <returns></returns>
        public static string GetFileText(string resourceName)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
