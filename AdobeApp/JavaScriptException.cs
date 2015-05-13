using System;
using Common.Logging;

namespace AdobeApp
{
	public class JavaScriptException : Exception
	{
        public JavaScriptException(string message) : base(message)
        {
        }
	}
}
