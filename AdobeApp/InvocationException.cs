using System;

namespace AdobeApp
{
    public class InvocationException : Exception
    {
        public InvocationException(string message) : base(message)
        {
        }
    }
}
