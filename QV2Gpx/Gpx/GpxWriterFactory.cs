using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QV2Gpx.Gpx
{
    internal interface IGpxWriterFactory
    {
        IGpxWriter Create(string filePath);
    }

    internal class GpxWriterFactory : IGpxWriterFactory
    {
        public IGpxWriter Create(string filePath)
        {
            return new GpxWriter(filePath);
        }
    }
}
