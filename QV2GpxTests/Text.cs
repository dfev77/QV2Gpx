using System;
using System.IO;
using NUnit.Framework;

namespace QV2GpxTests
{

    public static class Text
    {
        public static string XmlForCurrentTest()
        {
            return ForCurrentTest(".xml");
        }

        public static string ForCurrentTest(string fileExtension)
        {

            return FromFile(BuildFilePath(fileExtension));
        }

        private static string BuildFilePath(string fileExtension)
        {
            string path = TestContext.CurrentContext.Test.FullName;

            path = "TestData" + path.Substring(path.IndexOf('.'));
            path = path.Replace('.', '\\');
            path = path
                + ((!string.IsNullOrEmpty(fileExtension) && fileExtension[0] == '.') ? "" : ".")
                + fileExtension;

            return path;
        }
        public static string FromFile(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
