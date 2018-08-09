using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace SFGraphicsRenderTests.TestTools
{
    class ResourceShaders
    {
        public static string GetShader(string resourceName)
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
