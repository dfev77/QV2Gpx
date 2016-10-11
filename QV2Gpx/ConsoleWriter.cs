using System;
using QV2Gpx.Model;

namespace QV2Gpx
{
    internal class ConsoleWriter : IContentWriter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void ExportAllTracks(IDatabase db)
        {
            logger.Info($"Tracks for '{db.Name}':");
            
            foreach (Track track in db.GetTracks())
            {
                logger.Info($"\t{track.Id}: {track.Name}");
            }
        }
    }
}
