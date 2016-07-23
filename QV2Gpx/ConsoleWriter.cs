using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QV2Gpx.Model;

namespace QV2Gpx
{
    internal class ConsoleWriter : IContentWriter
    {
        public void ExportAllTracks(IDatabase db)
        {
            Console.WriteLine("Tracks:");
            foreach (Track track in db.GetTracks())
            {
                Console.WriteLine($"\t{track.Id}: {track.Name}");
            }
        }
    }
}
