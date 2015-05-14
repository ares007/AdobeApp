using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace AdobeApp
{
    /// <summary>
    /// abstraction to an Adobe Application
    /// </summary>
    /// <example>
    /// SomeClass result =
    ///     Application
    ///         .Name("Adobe InDesign CC2014")
    ///         .JavaScript("xxx.js")
    ///         .Run<SomeResultClass>(js => js.DoSomething(args));
    /// 
    /// </example>
    public class Application
    {
        private const int DEFAULT_TIMEOUT = 1800;
        private ILog Log = LogManager.GetLogger<Application>();

        private JsonSerializerSettings settings;

        public string AppName { get; private set; }
        public int AppleScriptTimeout { get; private set; }
        public string JavaScriptFilename { get; private set; }

        public Application(string name) : this(name, DEFAULT_TIMEOUT)
        {
        }

        public Application (string name, int timeout)
        {
            AppName = name;
            AppleScriptTimeout = timeout;
            settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
        }

        public static Application Name(string name)
        {
            return new Application(name);
        }

        public Application Timeout(int timeout)
        {
            AppleScriptTimeout = timeout;

            return this;
        }

        public Application JavaScript(string name)
        {
            JavaScriptFilename = name;

            return this;
        }

        #region Low Level Execution
        public string Execute(string functionName, object args)
        {
            var jsonArgs = JsonConvert.SerializeObject(args, Formatting.None, settings);
            return Execute(functionName, jsonArgs);
        }

        public string Execute(string functionName, string jsonArgs)
        {
            Log.Debug(m => m("Start Exectute {0}", functionName));

            using (var scriptDir = BuildScriptDir())
            {
                var appleScript = BuildAppleScript(scriptDir, functionName, jsonArgs);

                return AppleScriptRunner.Run(appleScript);
            }
        }

        private ScriptDir BuildScriptDir()
        {
            var javaScripts = new JavaScriptCollection();

            var scriptDir = new ScriptDir();
            scriptDir.Populate(javaScripts);

            return scriptDir;
        }

        private AppleScriptBuilder BuildAppleScript(ScriptDir scriptDir, string functionName, string jsonArgs)
        {
            return
                new AppleScriptBuilder()
                    .Tell(AppName)
                    .Timeout(AppleScriptTimeout)
                    .Assign("functionName", functionName)
                    .Assign("scriptArgs", jsonArgs)
                    .Assign("scriptLogger", "array")
                    .RunJavaScriptFile(scriptDir.Script(JavaScriptFilename), "functionName", "scriptArgs", "scriptLogger");
        }
        #endregion

        #region High level Run
        /// <summary>
        /// runs a given javaScript function with an argument
        /// </summary>
        /// <returns>
        /// an object constructed from the JSON result
        /// </returns>
        /// <param name="functionName">Function name.</param>
        /// <param name="arg">Argument.</param>
        public object Run(string functionName, object arg)
        {
            return Run<object>(functionName, arg);
        }

        /// <summary>
        /// runs a given javaScript function with an argument
        /// </summary>
        /// <returns>
        /// an object of the requested type built from the JSON result
        /// </returns>
        /// <param name="functionName">Function name.</param>
        /// <param name="arg">Argument.</param>
        /// <typeparam name="TResult">The type of the result to generate</typeparam>
        public TResult Run<TResult>(string functionName, object arg)
            where TResult: class
        {
            var responseText = Execute(functionName, arg);
            var response = JsonConvert.DeserializeObject<JavaScriptResponse>(responseText, settings);

            response.Log.ForEach(WriteLogLine);
            if (!response.Success)
                throw new JavaScriptException(response.Exception.ToString());

            Log.Debug(m => m("Result: {0}", JsonConvert.SerializeObject(response.Result, Formatting.None)));

            // poor man's conversion from object to TResult
            return JsonConvert.DeserializeObject<TResult>(
                JsonConvert.SerializeObject(response.Result)
            );
        }

        /// <summary>
        /// runs a given javaScript function with an argument
        /// </summary>
        /// <returns>
        /// an object of the requested type built from the JSON result
        /// </returns>
        /// <param name="functionCall">a simulated call with an arg</param>
        /// <typeparam name="TResult">The type of the result to generate</typeparam>
        public TResult Run<TResult>(Action<JavaScriptFunctionCall> functionCall)
                where TResult: class 
        {
            var javaScriptCall = new JavaScriptFunctionCall();
            functionCall(new JavaScriptFunctionCall());

            return Run<TResult>(javaScriptCall.FunctionName, javaScriptCall.Arg);
        }

        private void WriteLogLine(LogLine logLine)
        {
            switch (logLine.Severity)
            {
                case LogSeverity.Debug:
                    Log.Debug(logLine.Message);
                    break;

                case LogSeverity.Info:
                    Log.Info(logLine.Message);
                    break;

                case LogSeverity.Warn:
                    Log.Warn(logLine.Message);
                    break;

                case LogSeverity.Error:
                    Log.Error(logLine.Message);
                    break;
            }
        }
        #endregion
    }
}
