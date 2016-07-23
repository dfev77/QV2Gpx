﻿using System;
using System.IO;
using QV2Gpx.Model;

namespace QV2Gpx.Gpx
{
    internal class GpxContentWriter : IContentWriter
    {
        private IGpxWriterFactory _writerFactory;

        internal GpxContentWriter() : this(new GpxWriterFactory()) { }

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
            var filePath = track.GetFileName();
            Console.WriteLine($"Export track #{track.Id} '{track.Name}' to file '{filePath}'");

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
