using System;
using Common.Logging;

namespace AdobeApp
{
    /// <summary>
    /// Indicates an exception raised inside the JavaScript
    /// </summary>
	public class JavaScriptException : Exception
	{
        public JavaScriptException(string message) : base(message)
        {
        }
	}
}
