using System.IO;

namespace QV2Gpx
{
    [CliParse.ParsableClass("QV2Gpx", "Convert QuoVadis3 database into gpx files", ExampleText = "QV2Gpx <filePath>", ShowHelpWhenNoArgumentsProvided = true)]
    internal class CliArguments : CliParse.Parsable
    {
        private string _outputPath;
        [CliParse.ParsableArgument("file", Required = true, ShortName = 'f', ImpliedPosition = 1, Description = "Input file (.qu3)")]
        public string InputPath { get; set; }

        [CliParse.ParsableArgument("action", Required = false, ShortName = 'a', ImpliedPosition = 2, DefaultValue = Commands.Export, Description = "Action to perform, list tracks or export them")]
        public Commands Command { get; set; }

        [CliParse.ParsableArgument("out", Required = false, ShortName = 'o', ImpliedPosition = 3, Description = "Output folder")]
        public string OutputPath { get
            {
                return _outputPath ?? (File.Exists(InputPath) ? Path.GetDirectoryName(InputPath) : InputPath);
            }

            set { _outputPath = OutputPath; }
        }
    }
}
