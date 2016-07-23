using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text;
using System.Xml;
using QV2Gpx.Model;
using System.IO;

namespace QV2Gpx
{
    internal class Database : IDisposable, IDatabase
    {
        private readonly string _filePath;
        private OleDbConnection _connection;
        private DateBuilder _dateBuider;

        public Database(string filePath)
        {
            _filePath = filePath;
            _dateBuider = new DateBuilder();
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
                command.CommandText = $"select trp_idx, lat, lon, alt, [date], [time] from Tracks_trp where tr_idx = {trackId} order by trp_idx ASC";
                command.CommandType = System.Data.CommandType.Text;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new TrackPoint()
                        {
                            Id = (int)reader["trp_idx"],
                            Latitude = (float)reader["lat"],
                            Longitude = (float)reader["lon"],
                            Elevation = (float)reader["alt"],
                            Time = _dateBuider.Build((DateTime)reader["date"], (DateTime)reader["time"]),
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
        private string GetPointsTableName(int trackId)
        {
            return $"{Path.GetFileNameWithoutExtension(_filePath)}_WPseria{trackId:00}";
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterest(int trackId)
        {            
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $"select wp_idx, name, lat, lon, description, [date], [time], alt from {GetPointsTableName(trackId)} order by wp_idx asc";
                command.CommandType = System.Data.CommandType.Text;
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        yield break;
                    }

                    while (reader.Read())
                    {
                        yield return new PointOfInterest
                        {
                            Id = (int)reader["wp_idx"],
                            Name = (string)reader["name"],
                            Description = (string)reader["description"],
                            Latitude = (float)reader["lat"],
                            Longitude = (float)reader["lon"],
                            Elevation = (float)reader["alt"],
                            Time = _dateBuider.Build((DateTime)reader["date"], (DateTime)reader["time"])
                        };
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

    internal class DateBuilder
    {
        public DateTime Build(DateTime date, DateTime time)
        {
            return date.AddHours(time.Hour).AddMinutes(time.Minute).AddSeconds(time.Second).AddMilliseconds(time.Millisecond);
        }
    }
}
