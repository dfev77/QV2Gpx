using QV2Gpx.Processor;
using System;

namespace QV2Gpx
{
    static class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                if (args.Length == 0)
                {
                    var form = new MainForm();
                    InitializeLoggers(form.LoggingControl);
                    System.Windows.Forms.Application.Run(form);
                    return 0;
                }

                InitializeLoggers();
                logger = NLog.LogManager.GetCurrentClassLogger();

                var settings = new CliArguments();
                var parseResult = CliParse.ParsableExtensions.CliParse(settings, args);
                if (!parseResult.Successful || parseResult.ShowHelp)
                {
                    ShowHelp(settings, parseResult);
                    return -1;
                }

                Process(settings);
                return 0;
            } catch(Exception ex)
            {
                logger.Error(ex);
                return -2;
            }
            finally
            {
                Console.WriteLine("Press Enter to exit");
                Console.ReadLine();
            }
        }
        
        private static void InitializeLoggers(System.Windows.Forms.RichTextBox control = null)
        {
            NLog.Config.LoggingConfiguration config = new NLog.Config.LoggingConfiguration();
            NLog.Targets.ColoredConsoleTarget consoleTarget = new NLog.Targets.ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);
            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", NLog.LogLevel.Debug, consoleTarget));

            if (control != null)
            {
                NLog.Windows.Forms.RichTextBoxTarget uiTarget = new NLog.Windows.Forms.RichTextBoxTarget();
                uiTarget.TargetRichTextBox = control;
                uiTarget.Layout = " - ${message}";
                config.AddTarget("control", uiTarget);

                config.LoggingRules.Add(new NLog.Config.LoggingRule("*", NLog.LogLevel.Debug, uiTarget));
            }

            NLog.LogManager.Configuration = config;            
        }

        public static void Process(CliArguments settings)
        {
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
