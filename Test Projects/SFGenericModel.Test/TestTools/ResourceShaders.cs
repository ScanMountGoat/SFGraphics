using System.IO;
using System.Reflection;

namespace TestTools
{
    class ResourceShaders
    {
        public static string GetShader(string resourceName)
        {
            string fullName = $"SFGenericModel.Test.Shaders.{resourceName}";
            return GetResourceText(fullName);
        }

        private static string GetResourceText(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            string result;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }
    }
}
