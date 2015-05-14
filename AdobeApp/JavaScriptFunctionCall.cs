using System;
using System.Dynamic;

namespace AdobeApp
{
    public class JavaScriptFunctionCall : DynamicObject
    {
        public string FunctionName { get; private set; }
        public object Arg { get; private set; }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            FunctionName = binder.Name;
            Arg = args.Length >= 1 ? args[0] : null;

            // satisfy compiler -- we do not need a result
            result = null;

            return true;
        }
    }
}

