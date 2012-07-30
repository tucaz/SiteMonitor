using System;
using CommandLine;
using CommandLine.Text;

namespace SiteMonitor
{
    public class Arguments : CommandLineOptionsBase
    {
        [Option(null, "web", HelpText = "Specifies whether or not this instance should expose the web interface")]
        public bool WebInterface { get; set; }        

        [Option(null, "port", HelpText = "Port where the web interface will be hosted. Works together with /web switch")]
        public int? Port { get; set; }

        [Option(null, "monitor", HelpText = "Specifies whether or not this instance should run the monitoring tests")]
        public bool Monitor { get; set; }

        [Option(null, "interval", HelpText = "Interval between test runs. Specified in minutes")]
        public int? Interval { get; set; }

        [OptionArray(null, "assembly", HelpText = "List of assemblies containing tests to run")]
        public string[] Assembly { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("SiteMonitor", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()),                
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            
            this.HandleParsingErrorsInHelp(help);            
            
            help.AddPreOptionsLine("Usage: SiteMonitor.exe [web] [port] [monitor] [interval] [assembly]");
            help.AddPreOptionsLine(@"Example: SiteMonitor.exe --web --port=12345 --monitor --interval=5 --assembly C:\myAssembly1.dll C:\myAssembly2.dll");
            help.AddOptions(this);

            return help;
        }

        void HandleParsingErrorsInHelp(HelpText help)
        {
            if (this.LastPostParsingState.Errors.Count > 0)
            {
                var errors = help.RenderParsingErrorsText(this, 2); // indent with two spaces
                if (!string.IsNullOrEmpty(errors))
                {
                    help.AddPreOptionsLine(string.Concat(Environment.NewLine, "ERROR(S):"));
                    help.AddPreOptionsLine(errors);
                }
            }
        }
    }
}
