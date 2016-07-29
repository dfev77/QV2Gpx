using System;
using System.Collections.Generic;
using QV2Gpx.Model;

namespace QV2Gpx
{
    internal interface IDatabase : IDisposable
    {
        string Name { get; }

        IEnumerable<PointOfInterest> GetPointsOfInterest(int trackId);
        IEnumerable<TrackPoint> GetTrackPoints(int trackId);
        IEnumerable<Track> GetTracks();
        void Open();
    }
}