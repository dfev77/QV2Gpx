using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text;
using System.Xml;
using QV2Gpx.Model;
using System.IO;

namespace QV2Gpx
{
    internal class Database : IDisposable
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

        public IEnumerable<TrackPoint> GetTrackPoints(int trackId)
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
                        yield return new TrackPoint()
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

        public IEnumerable<Track> GetTracks()
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
        
        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this._connection.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

}
