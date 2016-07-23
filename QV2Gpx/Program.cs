using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                    Console.WriteLine(CliParse.ParsableExtensions.GetHelpInfo(settings, "{title}\r\n{description}\r\n{syntax}\r\n{footer}",
                "-{shortname}, --{name} - {description} {required}, {defaultvalue}, {example}"));
                    return -1;
                }

                using (var con = new Database(settings.InputFile))
                {
                    con.Open();
                    IContentWriter writer;

                    switch (settings.Command)
                    {
                        case Commands.List:
                            writer = new ConsoleWriter();
                            break;
                        case Commands.Export:
                            writer = new Gpx.GpxContentWriter();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException($"Unknown command: {settings.Command}");
                    }

                    writer.ExportAllTracks(con);
                }

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
    }
}
