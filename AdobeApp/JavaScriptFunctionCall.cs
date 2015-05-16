using System;
using System.Dynamic;

namespace AdobeApp
{
    /// <summary>
    /// allow calling a javaScript function via a dynamic object
    /// </summary>
    public class JavaScriptFunctionCall : DynamicObject
    {
        public string FunctionName { get; private set; }
        public object Arg { get; private set; }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            FunctionName = binder.Name;
            if (args.Length > 0)
                Arg = args[0];

            // satisfy compiler -- we do not need a result
            result = null;

            return true;
        }
    }
}
