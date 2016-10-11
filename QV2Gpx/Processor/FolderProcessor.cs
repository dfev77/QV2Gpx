using System;
using System.IO;

namespace QV2Gpx.Processor
{
    public class FolderProcessor : IProcessor
    {
        public const string QuoVadisExtension = "*.qu3";
        private readonly string _inputFolder;
        private string _outputFolder;

        public FolderProcessor(string inputFolder, string outputFolder)
        {
            _inputFolder = Path.GetFullPath(inputFolder);
            _outputFolder = outputFolder;
        }

        public void Process(Commands command)
        {
            foreach (string filePath in Directory.GetFiles(_inputFolder, QuoVadisExtension, SearchOption.AllDirectories))
            {
                var subfolder = Path.GetDirectoryName(filePath);
                var intermediateOutputFolder = (subfolder == _inputFolder) ? 
                    _outputFolder 
                    : Path.Combine(_outputFolder, subfolder.Substring(_inputFolder.Length + 1));
                    
                new FileProcessor(filePath, intermediateOutputFolder).Process(command);
            }
        }
    }

}
