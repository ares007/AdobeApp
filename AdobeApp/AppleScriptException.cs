using System;

namespace AdobeApp
{
    /// <summary>
    /// Indicates an exception during AppleScript execution
    /// </summary>
    public class AppleScriptException : Exception
    {
        public AppleScriptException(string message) : base(message)
        {
        }
    }
}
