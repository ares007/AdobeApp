using AdobeApp;
using Common.Logging;
using Common.Logging.Simple;
using System;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AdobeApp.Starter
{
    class MainClass
    {
        private static JavaScriptCollection javaScripts = new JavaScriptCollection();

        public static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                ExecuteCommand(options);
            }
        }

        static void ExecuteCommand(CommandLineOptions options)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            if (options.Debug)
                LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter();


            if (options.ListJavaScriptResources)
                ListJavaScriptResources();
            else if (options.ShowJavaScriptResource)
                ShowJavaScriptResource(options.JavaScript);
            else if (!String.IsNullOrEmpty(options.FunctionName))
            {
                if (options.Run)
                    RunJavaScrtiptFunction(options.JavaScript, options.FunctionName, options.Args);
                else
                    ExecuteJavaScrtiptFunction(options.JavaScript, options.FunctionName, options.Args);
            }
            else if (options.Run)
            {
                Console.WriteLine("Please specify a JavaScript file to run!");
            }

            if (options.MeasureTime)
                Console.WriteLine("Elapsed Time: {0}ms", stopWatch.ElapsedMilliseconds);
        }

        static void ListJavaScriptResources()
        {
            javaScripts.Files()
                .OrderBy(f => f)
                .ToList()
                .ForEach(Console.WriteLine);
        }

        static void ShowJavaScriptResource(string javaScript)
        {
            Console.WriteLine(javaScripts[javaScript]);
        }

        static void ExecuteJavaScrtiptFunction(string javaScript, string functionName, string args)
        {
            var app = new Application("Adobe InDesign CC 2014");
            app.JavaScript(javaScript);

            Console.WriteLine(app.Execute(functionName, args));
        }

        static void RunJavaScrtiptFunction(string javaScript, string functionName, string jsonArgs)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            var args = JsonConvert.DeserializeObject(jsonArgs, settings);

            var app = new Application("Adobe InDesign CC 2014");
            app.JavaScript(javaScript);

            var result = app.Run(functionName, args);
            Console.WriteLine("Result: {0}", result);
        }
    }
}
