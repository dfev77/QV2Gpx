using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QV2Gpx
{
    public interface IProcessor
    {
        void Process(Commands action);
    }
}
