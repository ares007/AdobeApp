using System;

namespace AdobeApp
{
    /// <summary>
    /// represents an exeception handling inside JavaScript
    /// </summary>
    public class ExceptionHandling : ICallable
    {
        public Invocation Try { get; set; }
        public Invocation Catch { get; set; }
        public Invocation Finally { get; set; }

        public object ToCallable()
        {
            return new { Try = Try, Catch = Catch, Finally = Finally };
        }
    }
}
