using System;
using System.IO;
using QV2Gpx.Model;

namespace QV2Gpx.Gpx
{
    internal class GpxContentWriter : IContentWriter
    {
        public void ExportAllTracks(Database db)
        {
            foreach (Track track in db.GetTracks())
            {
                WriteTrackToFile(db, track);
            }
        }

        private void WriteTrackToFile(Database db, Track track)
        {
            var filePath = track.GetFileName();
            using (FileStream output = new FileStream(filePath, FileMode.Create))
            {
                Console.WriteLine($"Export track #{track.Id} '{track.Name}' to file '{filePath}'");

                using (GpxWriter writer = new GpxWriter(output))
                {
                    writer.StartTrack(track);

                    foreach (TrackSegment point in db.GetTrackPoints(track.Id))
                    {
                        writer.WritePoint(point);
                    }

                    writer.EndTrack();
                }
            }
        }

    }
}
