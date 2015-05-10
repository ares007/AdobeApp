using System;

namespace AdobeApp
{
    /// <summary>
    /// Contains all neccesary options for a function call
    /// </summary>
    /// <description>
    /// a function call is a part of typically more sequientially running calls
    /// (a <c>Invocation</c>. The exection is transfered to the JavaScript host
    /// using a data structure and dispatched there.
    /// </description>
    public class FunctionCall : ICallable
    {
        /// <summary>
        /// Name of the function to call
        /// </summary>
        /// <value>The name of the function inside the global scope of JavaScript</value>
        public string Function { get; set; }

        /// <summary>
        /// Arguments for the function call
        /// </summary>
        /// <value>List of arguments for the function call.</value>
        public object[] Arguments { get; set; }

        public object ToCallable()
        {
            return new { Function = Function, Arguments = Arguments };
        }
    }
}
