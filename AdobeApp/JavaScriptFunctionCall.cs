using System;
using System.Dynamic;

namespace AdobeApp
{
    public class JavaScriptFunctionCall : DynamicObject
    {
        public string FunctionName { get; private set; }

        public JavaScriptFunctionCall()
        {
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            FunctionName = binder.Name;

            // TODO: call function. result is string. Convert to JavaScriptResponse

            result = "hello"; // satisfy compiler

            return true;
        }
    }
}

