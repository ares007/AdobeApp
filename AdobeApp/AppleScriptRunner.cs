using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
            var startInfo = new ProcessStartInfo
                {
                    FileName = OSASCRIPT,
                    Arguments = "-",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                };

            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            var stdInWriter = new StreamWriter(process.StandardInput.BaseStream, Encoding.GetEncoding("macintosh"));
            var stdInTask = stdInWriter.WriteAsync(script)
                .ContinueWith(t => stdInWriter.Close());

            var stdOutTask = process.StandardOutput.ReadToEndAsync();
            var stdErrTask = process.StandardError.ReadToEndAsync();

            process.WaitForExit();
            Task.WaitAll(stdInTask, stdOutTask, stdErrTask);

            // Console.WriteLine("exit code = {0}, result = {1}", process.ExitCode, stdOutTask.Result);

            if (process.ExitCode != 0)
            {
                throw new AppleScriptException(stdErrTask.Result);
            }

            try 
            {
                process.Dispose(); 
            }
            #pragma warning disable 0168
            catch (ObjectDisposedException e) 
            #pragma warning restore 0168
            {
                // ObjectDisposedException thrown
                // because StandardInput is already closed
            }

            return stdOutTask.Result;
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
