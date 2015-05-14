using System;
using TwoPS.Processes;
using System.Text;
using System.Threading.Tasks;

namespace AdobeApp
{
    /// <summary>
    /// Execute an applescript from various sources:
    ///  * given as a string
    ///  * given from an AppleScriptBuilder
    ///  * from a file saved inside a script dir
    /// </summary>
    /// <example>
    /// AppleScriptRunner.RunScript("return 42");
    /// </example>
    public static class AppleScriptRunner
    {
        private const string OSASCRIPT = "/usr/bin/osascript";

        /// <summary>
        /// Runs the script.
        /// </summary>
        /// <returns>result of the script</returns>
        /// <param name="script">AppleScript source</param>
        public static string Run(string script)
        {
            var options = new ProcessOptions(OSASCRIPT, "-");
            options.StandardInputEncoding = Encoding.GetEncoding("macintosh");
            // stdout is UTF8, because we pass-thru JavaScript's output

            var process = new Process(options);
            var task = Task.Run(() => process.Run());
            process.StandardInput.Write(script);
            process.StandardInput.Close();
            var result = task.Result;

            if (result.ExitCode != 0)
            {
                throw new AppleScriptException(result.StandardError);
            }

            return result.StandardOutput;
        }

        /// <summary>
        /// runs a script constructed by the builder
        /// </summary>
        /// <returns>result of the script</returns>
        /// <param name="builder">an AppleScriptBuilder instance</param>
        public static string Run(AppleScriptBuilder builder)
        {
            return Run(builder.ToString());
        }
    }
}
