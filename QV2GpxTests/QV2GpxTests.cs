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
        [Test]
        public void ForAnEmptySetOfData_WritesCorrectGpxNodeAttributesAndNamespaces()
        {
            //arrange
            var outputStream = new StringWriterWithEncoding();
            var sut = new GpxWriter(outputStream);
            var track = new Track() { Id = 1, Name = "test" };

            //act
            sut.StartTrack(track);
            sut.EndTrack();
            sut.Close();
            
            //assert
            Assert.That<string>(outputStream.ToString(),
                                CompareConstraint.IsIdenticalTo(Text.XmlForCurrentTest()),
                                "incorect xml");
        }

        [Test]
        public void ForAnGivenTrackAndPoint_WritesCorrectGpxNodes()
        {
            //arrange
            var outputStream = new StringWriterWithEncoding();
            var sut = new GpxWriter(outputStream);
            var track = new Track() { Id = 1, Name = "test" };
            var point = new TrackSegment() { Id = 1, Elevation = 12.5f, Latitude = 12.22f, Longitude = 41.32f, Time = new DateTime(1989, 12, 16, 17, 32, 15)};

            //act
            sut.StartTrack(track);
            sut.WritePoint(point);
            sut.EndTrack();
            sut.Close();

            //assert
            Assert.That<string>(outputStream.ToString(),
                                CompareConstraint.IsIdenticalTo(Text.XmlForCurrentTest()),
                                "incorect xml");
        }
    }
}
