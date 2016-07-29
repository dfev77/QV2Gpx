using QV2Gpx.Processor;
using System;

namespace QV2Gpx
{
    static class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            try
            {
                var settings = new CliArguments();
                var parseResult = CliParse.ParsableExtensions.CliParse(settings, args);
                if (!parseResult.Successful || parseResult.ShowHelp)
                {
                    ShowHelp(settings, parseResult);
                    return -1;
                }

                IProcessor processor = null;
                if (System.IO.File.Exists(settings.InputPath))
                {
                    processor = new FileProcessor(settings.InputPath, settings.OutputPath);
                }
                else if (System.IO.Directory.Exists(settings.InputPath))
                {
                    processor = new FolderProcessor(settings.InputPath, settings.OutputPath);
                }

                processor?.Process(settings.Command);
                return 0;

            } catch(Exception ex)
            {
                Console.Error.WriteLine(ex);
                return -2;
            }
            finally
            {
                Console.WriteLine("Press Enter to exit");
                Console.ReadLine();
            }
        }

        private static void ShowHelp(CliArguments settings, CliParse.CliParseResult parseResult)
        {
            Console.WriteLine(CliParse.ParsableExtensions.GetHelpInfo(settings, "{title}\r\n{description}"));

            if (!parseResult.Successful)
            {
                Console.WriteLine(string.Join(Environment.NewLine, parseResult.CliParseMessages));
                Console.WriteLine();
            }

            Console.WriteLine(CliParse.ParsableExtensions.GetHelpInfo(settings, 
                                                                      "{syntax}\r\n{footer}",
                                                                      "-{shortname}, --{name} - {description} {required}, {defaultvalue}, {example}"));
        }
    }
}
