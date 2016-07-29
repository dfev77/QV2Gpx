using System;

namespace QV2Gpx.Processor
{
    public class FileProcessor : IProcessor
    {
        private readonly string _inputFile;
        private string _outputFolder;

        public FileProcessor(string inputFile, string outputFolder)
        {
            _inputFile = inputFile;
            _outputFolder = outputFolder;
        }

        public void Process(Commands action)
        {
            using (var con = new Database(_inputFile))
            {
                con.Open();
                IContentWriter writer;

                switch (action)
                {
                    case Commands.List:
                        writer = new ConsoleWriter();
                        break;
                    case Commands.Export:
                        writer = new Gpx.GpxContentWriter(_outputFolder);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Unknown command: {action}");
                }

                writer.ExportAllTracks(con);
            }
        }
    }
}
