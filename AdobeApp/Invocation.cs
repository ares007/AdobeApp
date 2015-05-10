using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;

/* internal detail and ToDo:
 * create a data structure reflecting the sequence
 * 
 * --> basic structure for a sequence
 *     { execute: [ ... ] }
 * 
 * --> a "regular" sequential part adds to an array (FunctionCall)
 *     { function: "DoSomething", arguments: [ ... ] }
 * 
 * --> a try/catch execution generates: (ExceptionHandling)
 *     { try: [ ... ], catch: [ ... ], finaly: [ ... ] }
 * 
 * --> condition, loop
 *     { if: [ ... ], then: [ ... ], else: [ ... ] }
 *     { while: [ ... ], do: [ ... ] }
 * 
 */

namespace AdobeApp
{
    /// <summary>
    /// Encapsulates a complete JavaScript invocation chain
    /// </summary>
    /// <example>>
    /// invocation
    ///     .Open("/path/to/file.indd")
    ///     .DoSomething()
    ///     .SaveAs("/path/to/another.indd")
    ///     .Close();
    /// 
    /// // Experimental: exception handling in running JavaScript
    /// invocation
    ///     .Try(i => i.DoSomething())
    ///     .Catch(i => i.DoSomethingOnFailure())
    ///     .Finally(i => i.DoSomethingAlways());
    /// 
    /// // Idea: condition, loop: based on last function's return value
    /// invocation
    ///     .If(i => i.DoSomething())
    ///     .Then(t => t.DoThen())
    ///     .Else(e => e.DoElse());
    /// 
    /// invocation
    ///     .While(w => w.DoSomething())
    ///     .Do(d => d.DoWhile());
    /// 
    /// 
    /// ----- more complete examples
    /// 
    /// // conditional execution, stop on error
    /// invocation
    ///     .Open("/path/to/file.indd")
    ///     .CheckFonts()
    ///     .CheckLinks()
    ///     .If(i => i.FontsAndLinksOk())
    ///     .Then(t => t.CreateProofPdf().CreateThumbnail())
    ///     .Else(e => e.CreateErrorThumbnail())
    ///     .Close();
    /// 
    /// // surround dangerous part with a try, close always happens
    /// invocation
    ///     .Open("/path/to/file.indd")
    ///     .Try(try => try.CreateProofPdf().CreateThumbnail())
    ///     .Close();
    /// 
    /// // both mixed
    /// invocation
    ///     .Open("/path/to/file.indd")
    ///     .Try(try =>
    ///         try.CheckFonts()
    ///             .CheckLinks()
    ///             .If(i => i.FontsAndLinksOk())
    ///             .Then(t => t.CreateProofPdf().CreateThumbnail())
    ///             .Else(e => e.CreateErrorThumbnail())
    ///     )
    ///     .Close();
    /// 
    /// </example>
    public class Invocation : DynamicObject, ICallable
    {
        /// <summary>
        /// All functions to call in sequence
        /// </summary>
        /// <value>A List of FunctionCall objects representing all functions to call</value>
        public List<ICallable> FunctionCalls { get; set; }

        /// <summary>
        /// Instantiates an invocation
        /// </summary>
        public Invocation()
        {
            FunctionCalls = new List<ICallable>();
        }

        /// <summary>
        /// Reserved for future use
        /// </summary>
        /// <returns>Invocation to allow chaining</returns>
        /// <param name="invocation">Invocation.</param>
        public Invocation Try(Func<Invocation, Invocation> invocation)
        {
            FunctionCalls.Add(
                new ExceptionHandling
                {
                    Try = invocation(new Invocation())
                }
            );

            return this;
        }

        /// <summary>
        /// Reserved for future use
        /// </summary>
        /// <returns>Invocation to allow chaining</returns>
        /// <param name="invocation">Invocation.</param>
        public Invocation Catch(Func<Invocation, Invocation> invocation)
        {
            var exceptionHandling = FunctionCalls.Last() as ExceptionHandling;
            if (exceptionHandling == null)
                throw new InvocationException("Try statement is missing");

            if (exceptionHandling.Catch != null)
                throw new InvocationException("Catch already seen");
            
            exceptionHandling.Catch = invocation(new Invocation());

            return this;
        }

        /// <summary>
        /// Reserved for future use
        /// </summary>
        /// <returns>Invocation to allow chaining</returns>
        /// <param name="invocation">Invocation.</param>
        public Invocation Finally(Func<Invocation, Invocation> invocation)
        {
            var exceptionHandling = FunctionCalls.Last() as ExceptionHandling;
            if (exceptionHandling == null)
                throw new InvocationException("Try statement is missing");

            if (exceptionHandling.Finally != null)
                throw new InvocationException("Finally already seen");

            exceptionHandling.Finally = invocation(new Invocation());

            return this;
        }

        /// <summary>
        /// Opens the file given
        /// </summary>
        /// <param name="path">Full path to file to open</param>
        /// <returns>Invocation to allow chaining</returns>
        public dynamic Open(string path)
        {
            FunctionCalls.Add(
                new FunctionCall
                {
                    Function = "Open",
                    Arguments = new object[]
                        {
                            new { Path = path }
                        },
                }
            );

            return this;
        }

        /// <summary>
        /// Saves the open document
        /// </summary>
        /// <returns>Invocation to allow chaining</returns>
        /// <param name="path">Path to save the file to</param>
        public dynamic SaveAs(string path)
        {
            FunctionCalls.Add(
                new FunctionCall
                {
                    Function = "Save",
                    Arguments = new object[]
                        {
                            new { Path = path }
                        },
                }
            );

            return this;
        }

        /// <summary>
        /// Closes the currently open document without saving
        /// </summary>
        /// <returns>Invocation to allow chaining</returns>
        public dynamic Close()
        {
            FunctionCalls.Add(
                new FunctionCall
                {
                    Function = "Close",
                    Arguments = new object[] {},
                }
            );

            return this;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out dynamic result)
        {
            FunctionCalls.Add(
                new FunctionCall
                { 
                    Function = binder.Name,
                    Arguments = args
                }
            );

            result = this;
            return true;
        }

        public object ToCallable()
        {
            return FunctionCalls.Select(f => f.ToCallable()).ToArray();
        }
    }
}
