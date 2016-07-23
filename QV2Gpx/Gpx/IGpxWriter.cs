using System.Collections.Generic;
using QV2Gpx.Model;

namespace QV2Gpx
{
    internal interface IGpxWriter : System.IDisposable
    {
        void Close();
        void EndTrack();
        void StartTrack(Track track);
        void WritePoint(PointOfInterest point);
        void WritePoints(IEnumerable<PointOfInterest> points);
        void WriteTrackPoint(TrackPoint point);
        void WriteTrackPoints(IEnumerable<TrackPoint> points);
    }
}