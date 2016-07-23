using System;
using System.IO;
using NUnit.Framework;
using QV2Gpx;
using Org.XmlUnit.Constraints;
using QV2Gpx.Model;

namespace QV2GpxTests
{    
    [TestFixture]
    public class GpxWriterTests
    {
        static readonly Track track = new Track() { Id = 1, Name = "test" };
        static TrackPoint trackPoint = new TrackPoint() { Id = 1, Elevation = 12.5f, Latitude = 12.22f, Longitude = 41.32f, Time = new DateTime(1989, 12, 16, 17, 32, 15) };
        static PointOfInterest poi1 = new PointOfInterest()
        {
            Id = 2,
            Description = "beautifull point",
            Elevation = 12.5f,
            Latitude = 12.22f,
            Longitude = 41.32f,
            Name = "point name",
            Time = new DateTime(1966, 9, 8, 19, 0, 0),
        };

        static StringWriter outputStream;
        static GpxWriter sut;

        [SetUp]
        public void Setup()
        {
            outputStream = new StringWriterWithEncoding();
            sut = new GpxWriter(outputStream);
        }

        [Test]
        public void ForAnEmptySetOfData_WritesCorrectGpxNodeAttributesAndNamespaces()
        {
            //arrange

            //act
            sut.StartTrack(track);
            sut.EndTrack();
            sut.Close();
            
            //assert
            Assert.That(outputStream.ToString(),
                        CompareConstraint.IsIdenticalTo(Text.XmlForCurrentTest()),
                        "incorect xml");
        }

        [Test]
        public void ForATrackWithATrackPoint_WritesCorrectXml()
        {
            //arrange
            
            //act
            sut.StartTrack(track);
            sut.WriteTrackPoint(trackPoint);
            sut.EndTrack();
            sut.Close();

            //assert
            Assert.That(outputStream.ToString(),
                        CompareConstraint.IsIdenticalTo(Text.XmlForCurrentTest()),
                        "incorect xml");
        }

        [Test]
        public void ForAnEmptyTrackAndAPointOfInterest_WritesCorrectXml()
        {
            //arrange

            //act
            sut.StartTrack(track);
            sut.EndTrack();
            sut.WritePoint(poi1);
            sut.Close();

            //assert
            Assert.That(outputStream.ToString(),
                        CompareConstraint.IsSimilarTo(Text.XmlForCurrentTest()),
                        "incorect xml");
        }

        [Test]
        public void ForASetOfPointOfInterest_WritesCorrectXml()
        {
            //arrange
            var points = new []{
                     poi1,
                    new PointOfInterest()
                    {
                        Id = 2,
                        Description = "beautifull point",
                        Elevation = 12.5f,
                        Latitude = 12.22f,
                        Longitude = 41.32f,
                        Name = "point 2",
                        Time = new DateTime(1966, 9, 8, 19, 0, 0),
                    }};

            //act
            sut.StartTrack(track);
            sut.EndTrack();
            sut.WritePoints(points);
            sut.Close();

            //assert
            Assert.That(outputStream.ToString(),
                        CompareConstraint.IsSimilarTo(Text.XmlForCurrentTest()),
                        "incorect xml");
        }

        [Test]
        public void ForATrackAndAPointOfInterest_WritesCorrectXml()
        {
            //arrange
            var points = new[]{ poi1 };

            //act
            sut.StartTrack(track);
            sut.WriteTrackPoint(trackPoint);
            sut.EndTrack();
            sut.WritePoints(points);
            sut.Close();

            //assert
            Assert.That(outputStream.ToString(),
                        CompareConstraint.IsSimilarTo(Text.XmlForCurrentTest()),
                        "incorect xml");
        }
    }
}
