using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;

namespace QV2GpxTests
{
    [SetUpFixture]
    public class TestsInitialization
    {
        [OneTimeSetUp]
        public void SetupWorkingFolder()
        {
            string desiredWorkingFolder =
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Environment.CurrentDirectory = desiredWorkingFolder;
        }
    }
}
