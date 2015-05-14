using System;

namespace AdobeApp
{
	public class JavaScriptExceptionInfo
	{
        public string Name { get; set; }
        public string Description { get; set; }
        public int LineNo { get; set; }
        public string FileName { get; set; }

        public override string ToString()
        {
            return string.Format("[JavaScriptExceptionInfo: Name={0}, Description={1}, LineNo={2}, FileName={3}]", Name, Description, LineNo, FileName);
        }
	}
}