using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QV2Gpx
{
    internal interface IContentWriter
    {
        void ExportAllTracks(Database db);
    }
}
