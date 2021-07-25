using System;
using System.IO;
using System.Text;

namespace RayTracer.Tests.Smoke
{
    public class TestBase
    {
        protected StringBuilder imagePath = new StringBuilder();
        protected StringBuilder modelPath = new StringBuilder();

        public TestBase()
        {
            int index = 0;
            string currpath = Directory.GetCurrentDirectory();
            string[] pathParts = currpath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in pathParts)
            {
                imagePath.Append("/" + pathParts[index++]);
                if (part == "RayTracer.Tests.Smoke") break;
            }
            modelPath.Append(imagePath.ToString());
            imagePath.Append("/images/");
            modelPath.Append("/models/");

            if (!Directory.Exists(imagePath.ToString()))
                Directory.CreateDirectory(imagePath.ToString());

            if (!Directory.Exists(modelPath.ToString()))
                Directory.CreateDirectory(modelPath.ToString());
        }
    }
}
