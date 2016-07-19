using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text;
using System.Xml;
using QV2Gpx.Model;

namespace QV2Gpx
{
    public class Database : IDisposable
    {
        private readonly string _filePath;
        private OleDbConnection _connection;
        public Database(string filePath)
        {
            _filePath = filePath;
        }

        public void Open()
        {
            _connection = new OleDbConnection($"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={_filePath};Persist Security Info = False;");
            _connection.Open();
        }
        
        public void DumpTracks()
        {
            Console.WriteLine("Tracks:");
            foreach (Track track in GetTracks())
            {
                Console.WriteLine($"\t{track.Id}: {track.Name}");
            }
        }

        private IEnumerable<TrackSegment> GetTrackPoints(int trackId)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $"select trp_idx, lat, lon, alt, date, time, speed, course from Tracks_trp where tr_idx = {trackId} order by trp_idx ASC";
                command.CommandType = System.Data.CommandType.Text;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = (DateTime)reader["date"];
                        DateTime time = (DateTime)reader["time"];
                        yield return new TrackSegment()
                        {
                            Id = (int)reader["trp_idx"],
                            Latitude = (float)reader["lat"],
                            Longitude = (float)reader["lon"],
                            Elevation = (float)reader["alt"],
                            Time = date.AddHours(time.Hour).AddMinutes(time.Minute).AddSeconds(time.Second).AddMilliseconds(time.Millisecond),
                        };
                    }
                }
            }
        }

        private IEnumerable<Track> GetTracks()
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "select tr_idx as id, name from Tracks";
                command.CommandType = System.Data.CommandType.Text;
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        yield break;
                    }

                    while (reader.Read())
                    {
                        yield return new Track { Id = (int)reader["id"], Name = (string)reader["name"] };
                    }
                }
            }
        }

        public void ExportAllTracksToFile()
        {
            foreach(Track track in GetTracks())
            {
                ExportTrackToFile(track);
            }
        }

        private void ExportTrackToFile(Track track)
        {
            string filePath = System.IO.Path.GetFullPath(track.GetFileName() + ".gpx");
            Console.WriteLine($"Export track #{track.Id} '{track.Name}' to file '{filePath}'");

            const string defaultNamespace = "http://www.topografix.com/GPX/1/1";
            const string xsiNameSpace = "http://www.w3.org/2001/XMLSchema-instance";
            const string schemaLocation = "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd";
            const string gpxVersion = "1.1";

            XmlWriterSettings writerSettings = new XmlWriterSettings()
                                                    {
                                                        Encoding = Encoding.UTF8,
                                                        Indent = true,
                                                    };

            using (XmlWriter writer = XmlWriter.Create(filePath, writerSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("gpx", defaultNamespace);
                writer.WriteAttributeString("xmlns", defaultNamespace);
                writer.WriteAttributeString("xmlns", "xsi", "http://www.w3.org/2000/xmlns/", xsiNameSpace);
                writer.WriteAttributeString("xsi", "schemaLocation", xsiNameSpace, schemaLocation);

                writer.WriteAttributeString("creator", AppInfo.Instance.ToString());
                writer.WriteAttributeString("version", gpxVersion);

                writer.WriteStartElement("trk");
                writer.WriteElementString("name", track.Name);

                writer.WriteStartElement("trkseg");
                
                foreach (TrackSegment point in GetTrackPoints(track.Id))
                {
                    writer.WriteStartElement("trkpt");
                    writer.WriteAttributeString("lat", point.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    writer.WriteAttributeString("lon", point.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    writer.WriteElementString("ele", point.Elevation.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    writer.WriteElementString("time", point.Time.ToString("s"));
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndDocument();
            }
        }
        
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this._connection.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Database() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

}
