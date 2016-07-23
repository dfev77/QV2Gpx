﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;

namespace QV2Gpx
{
    internal class GpxWriter : IDisposable
    {
        private readonly XmlWriter _writer;
        private bool _rootNodeWritten;
        private bool _segmentStarted;

        public GpxWriter(TextWriter baseStream)
        {
            this._writer = XmlWriter.Create(baseStream, GetWriterSettings());
        }

        public GpxWriter(Stream baseStream)
        {
            this._writer = XmlWriter.Create(baseStream, GetWriterSettings());
        }

        private XmlWriterSettings GetWriterSettings()
        {
            return new XmlWriterSettings()
            {
                Encoding = System.Text.Encoding.UTF8,
                Indent = true,
            };
        }

        private void InitializeXml()
        {
            if (_rootNodeWritten)
            {
                return;
            }

            const string defaultNamespace = "http://www.topografix.com/GPX/1/1";
            const string xsiNameSpace = "http://www.w3.org/2001/XMLSchema-instance";
            const string schemaLocation = "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd";
            const string gpxVersion = "1.1";

            _writer.WriteStartDocument();
            _writer.WriteStartElement("gpx", defaultNamespace);
            _writer.WriteAttributeString("xmlns", defaultNamespace);
            _writer.WriteAttributeString("xmlns", "xsi", "http://www.w3.org/2000/xmlns/", xsiNameSpace);
            _writer.WriteAttributeString("xsi", "schemaLocation", xsiNameSpace, schemaLocation);

            _writer.WriteAttributeString("creator", AppInfo.Instance.ToString());
            _writer.WriteAttributeString("version", gpxVersion);

            _rootNodeWritten = true;
        }

        public void StartTrack(Model.Track track)
        {
            InitializeXml();
            
            _writer.WriteStartElement("trk");
            _writer.WriteElementString("name", track.Name);
        }

        public void WritePoint(Model.TrackSegment point)
        {
            StartSegment();
            _writer.WriteStartElement("trkpt");
            _writer.WriteAttributeString("lat", point.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture));
            _writer.WriteAttributeString("lon", point.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture));
            _writer.WriteElementString("ele", point.Elevation.ToString(System.Globalization.CultureInfo.InvariantCulture));
            _writer.WriteElementString("time", point.Time.ToString("s"));
            _writer.WriteEndElement();
        }

        private void StartSegment()
        {
            if (!_segmentStarted)
            {
                _writer.WriteStartElement("trkseg");
                _segmentStarted = true;
            }
        }

        private void EndSegment()
        {
            if (_segmentStarted)
            {
                _writer.WriteEndElement();
                _segmentStarted = false;
            }
        }

        public void EndTrack()
        {
            EndSegment();
            _writer.WriteEndElement();
            _writer.Flush();
        }

        public void Close()
        {
            //close gpx node
            _writer.WriteEndElement();
            _writer.Flush();
            _writer.Close();
        }



        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Close();
                    this._writer.Dispose();
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
