using System;
using CommandLine;
using CommandLine.Text;

namespace AdobeApp.Starter
{
    public class CommandLineOptions
    {
        [Option('v', "verbose", DefaultValue = false, HelpText = "generate more output")]
        public bool Verbose { get; set; }

        [Option('d', "debug", DefaultValue = false, HelpText = "debug output")]
        public bool Debug { get; set; }

        [Option('l', "list", DefaultValue = false, HelpText = "list available JavaScript resources")]
        public bool ListJavaScriptResources { get; set; }

        [Option('s', "show", DefaultValue = false, HelpText = "show content of a JavaScript resource")]
        public bool ShowJavaScriptResource { get; set; }

        [Option('r', "run", DefaultValue = false, HelpText = "run (high level) instead of execute (low level)")]
        public bool Run { get; set; }

        [Option('j', "javascript", HelpText = "JavaScript to show or execute")]
        public string JavaScript { get; set; }

        [Option('f', "function", HelpText = "JavaScript function to execute or run")]
        public string FunctionName { get; set; }

        [Option('a', "args", HelpText = "Arguments for function (JSON)")]
        public string Args { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                helptext => HelpText.DefaultParsingErrorsHandler(this, helptext)
            );
        }
    }
}
