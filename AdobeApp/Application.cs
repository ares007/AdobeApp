using System;
using Common.Logging;

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
        }

        public static Application Name(string name) {
            return new Application(name);
        }

        public Application Timeout(int timeout) {
            AppleScriptTimeout = timeout;

            return this;
        }

        public Application JavaScript(string name) {
            JavaScriptFilename = name;

            return this;
        }

        // TODO: Ändern auf Action<JavaScriptFunctionCall>
        // JavaScriptFunctionCall speichert nur FunctionName und Args
        // Aufruf erfolgt hier mit dem AppleScriptRunner

        TResult Run<TResult>(Func<JavaScriptFunctionCall, JavaScriptResponse> functionCall)
                where TResult: class 
        {
            // eigentlich nicht.
            // JavaSCriptCollection zum Suchen nach dem JavaScript
            // AppleScriptBuilder notwendig zum bauen 
            //      -- functionName erst bei JavaScriptFunctionCall bekannt
            // AppleScriptRunner zum starten des ganzen
            // JSON deserialisierung zu TResult

            var response = functionCall(new JavaScriptFunctionCall());
            response.Log.ForEach(WriteLogLine);
            if (!response.Success)
                throw new JavaScriptException(response.Exception.ToString());

            // FIXME: wrong - must be converted to TResult
            return response.Result as TResult;
        }

        private void WriteLogLine(LogLine logLine) {
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

//        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
//        {
//            // return base.TryInvokeMember(binder, args, out result);
//
//        }
    }
}
