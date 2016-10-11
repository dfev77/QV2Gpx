using System;
using System.IO;
using QV2Gpx.Model;

namespace QV2Gpx.Gpx
{
    internal class GpxContentWriter : IContentWriter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly string _outputFolder;
        private IGpxWriterFactory _writerFactory;

        internal GpxContentWriter(string outputFolder) : this(new GpxWriterFactory()) {
            _outputFolder = outputFolder;
        }

        internal GpxContentWriter(IGpxWriterFactory writerFactory)
        {
            _writerFactory = writerFactory;
        }

        public void ExportAllTracks(IDatabase db)
        {
            foreach (Track track in db.GetTracks())
            {
                WriteTrackToFile(db, track);
            }
        }
        
        private void WriteTrackToFile(IDatabase db, Track track)
        {
            var folder = Path.Combine(_outputFolder, db.Name);
            Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, track.GetFileName());
            logger.Info($"Export track {db.Name} - '{track.Name}' (#{track.Id}) to file '{filePath}'");

            using (IGpxWriter writer = _writerFactory.Create(filePath))
            {
                var points = db.GetPointsOfInterest(track.Id);
                writer.WritePoints(points);
                writer.StartTrack(track);
                writer.WriteTrackPoints(db.GetTrackPoints(track.Id));
                writer.EndTrack();
            }
        }
    }
}
