using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QV2Gpx
{
    internal class AppInfo
    {
        private AppInfo()
        {

        }
        public string Name = "QV2Gpx";
        public string Version = "0.1";
        public static AppInfo Instance = new AppInfo();

        public override string ToString()
        {
            return $"{Name}#{Version}";
        }
    }
}
