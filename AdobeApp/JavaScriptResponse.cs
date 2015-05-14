using System;
using System.Collections.Generic;

namespace AdobeApp
{
    /// <summary>
    /// holds the response from the JavaScript
    /// </summary>
    public class JavaScriptResponse
    {
        public bool Success { get; set; }
        public object Result { get; set; }
        public List<LogLine> Log { get; set; }
        public JavaScriptExceptionInfo Exception { get; set; }
    }
}
