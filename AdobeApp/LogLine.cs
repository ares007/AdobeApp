using System;
using System.Collections.Generic;

namespace AdobeApp
{
    /// <summary>
    /// a single log message line
    /// </summary>
	public class LogLine
	{
        public LogSeverity Severity { get; set; }
        public string Message { get; set; }
	}
}
