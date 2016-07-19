using System;

namespace QV2Gpx
{
    [CliParse.ParsableClass("QV2Gpx", "Convert QuoVadis3 database into gpx files", ExampleText = "QV2Gpx <filePath>", ShowHelpWhenNoArgumentsProvided = true)]
    internal class CliArguments : CliParse.Parsable
    {
        [CliParse.ParsableArgument("file", Required = true, ShortName = 'f', ImpliedPosition = 1, Description = "Input file (.qu3)")]
        public string InputFile { get; set; }

        [CliParse.ParsableArgument("action", Required = false, ShortName = 'a', DefaultValue = Commands.Export, Description = "Action to perform, list tracks or export them")]
        public Commands Command { get; set; }
    }
}
