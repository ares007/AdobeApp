using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AdobeApp
{
    /// <summary>
    /// little helper for building AppleScript statements
    /// </summary>
    /// <example>
    /// new AppleScriptBuilder()
    ///     .Tell("Adobe InDesign CC14")
    ///     .Timeout(1800)
    ///     .Assign("variable", someValue)
    ///     .RunJavaScriptFile("/path/to/js", "args")
    ///     .ToString()
    /// </example>
    public class AppleScriptBuilder
    {
        private List<string> headCalls;
        private List<string> tailCalls;

        private Boolean isInDesign = false;

        public AppleScriptBuilder()
        {
            headCalls = new List<string>();
            tailCalls = new List<string>();
        }

        public override string ToString()
        {
            var lines = new StringBuilder();

            headCalls.ForEach(l => lines.AppendLine(l));
            tailCalls.ForEach(l => lines.AppendLine(l));

            return lines.ToString();
        }

        #region builders
        public AppleScriptBuilder Tell(string application)
        {
            isInDesign = application.Contains("InDesign");

            headCalls.Add(String.Format("tell application \"{0}\"", application));
            tailCalls.Insert(0, "end tell");

            return this;
        }

        public AppleScriptBuilder Timeout(int timeout)
        {
            headCalls.Add(String.Format("with timeout of {0} seconds", timeout));
            tailCalls.Insert(0, "end timeout");

            return this;
        }

        // TODO: add more overloads (eg. float, boolean)
        public AppleScriptBuilder Assign(string variableName, int content)
        {
            headCalls.Add(String.Format("set {0} to {1}", variableName, content));

            return this;
        }

        public AppleScriptBuilder Assign(string variableName, string content)
        {
            if (NoEscapeNeeded(content) && content.Length < 60)
            {
                headCalls.Add(String.Format("set {0} to \"{1}\"", variableName, content));
            }
            else if (content.Length <= 40)
            {
                headCalls.Add(String.Format("set {0} to {1}", variableName, AppleScriptStringEncoder.ToUtxt(content)));
            }
            else
            {
                var chunks = AppleScriptStringEncoder.SplitIntoChunks(content, 40);
                var isFirst = true;
                var extra = "";

                foreach (var chunk in chunks)
                {
                    headCalls.Add(String.Format("set {0} to {1}{2}", variableName, extra, chunk));
                    if (isFirst) {
                        extra = variableName + " & ";
                        isFirst = false;
                    }
                }
            }    

            return this;
        }

        private bool NoEscapeNeeded(string text)
        {
            return new Regex("\\A[0-~]*\\z").IsMatch(text);
        }

        public AppleScriptBuilder RunJavaScriptFile(string file, params string[] args) 
        {
            string format = isInDesign
                ? "do script (POSIX file \"{0}\") language javascript with arguments {{ {1} }} undo mode fast entire script"
                : "do javascript \"$.evalFile('{0}')\" with arguments {{ {1} }}";

            headCalls.Add(
                String.Format(format, file, String.Join(", ", args))
            );

            return this;
        }
        #endregion
    }
}
