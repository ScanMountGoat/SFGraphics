using System.IO;
using System.Reflection;

namespace RenderTestUtils
{
    public static class ResourceShaders
    {
        public static string GetShaderSource(string shaderName)
        {
            string fullName = $"RenderTestUtils.Shaders.{shaderName}";
            return GetResourceText(fullName);
        }

        private static string GetResourceText(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            string result = "";
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
