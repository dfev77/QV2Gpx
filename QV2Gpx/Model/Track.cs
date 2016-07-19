using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QV2Gpx.Model
{
    internal class Track
    {
        public int Id;
        public string Name;

        public string GetFileName()
        {
            if (Name.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) < 0)
            {
                return Name;
            }

            const char replacementChar = '_';
            foreach (char invalidChar in System.IO.Path.GetInvalidFileNameChars())
            {
                Name = Name.Replace(invalidChar, replacementChar);
            }

            return Name;
        }
    }
}
