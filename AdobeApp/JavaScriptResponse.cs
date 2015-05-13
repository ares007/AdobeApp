using System;
using System.Collections.Generic;

namespace AdobeApp
{
    public class JavaScriptResponse
    {
        public bool Success { get; set; }
        public object Result { get; set; }
        public List<LogLine> Log { get; set; }
        public JavaScriptExceptionInfo Exception { get; set; }
    }
}
