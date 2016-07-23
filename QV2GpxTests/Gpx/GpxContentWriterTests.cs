using System;
using System.Collections.Generic;
using QV2Gpx;
using QV2Gpx.Gpx;
using QV2Gpx.Model;
using NSubstitute;
using NUnit.Framework;

namespace QV2GpxTests.Gpx
{
    [TestFixture]
    class GpxContentWriterTests
    {
        [Test]
        public void ExportAllTracks_ForATrackAndASetOfNodes_WritesThemInCorrectOrder()
        {
            // arange
            var tracks = new Track[] { new Track() { Id = 13, Name = "test track" } };
            var trackPoints = new TrackPoint[] { new TrackPoint() };
            var trackPOIs = new PointOfInterest[] { new PointOfInterest() };
            IGpxWriter writer = Substitute.For<IGpxWriter>();
            IGpxWriterFactory writerFactory = Substitute.For<IGpxWriterFactory>();
            writerFactory.Create(Arg.Any<string>()).Returns(writer);
            GpxContentWriter sut = new GpxContentWriter(writerFactory);
            IDatabase db = Substitute.For<IDatabase>();
            db.GetTracks().Returns(tracks);
            db.GetTrackPoints(tracks[0].Id).Returns(trackPoints);
            db.GetPointsOfInterest(tracks[0].Id).Returns(trackPOIs);

            // act
            sut.ExportAllTracks(db);

            // arrange
            db.Received(1).GetTracks();

            writerFactory.Received(1).Create(Arg.Is<string>(x => x.Contains(tracks[0].Name)));
            db.Received(1).GetPointsOfInterest(tracks[0].Id);
            writer.Received(1).WritePoints(Arg.Any<PointOfInterest[]>());

            writer.Received(1).StartTrack(tracks[0]);
            writer.Received(1).WriteTrackPoints(trackPoints);
            writer.Received(1).EndTrack();
        }
    }
}
