using AdobeApp;
using NUnit.Framework;
using System;

namespace AdobeApp.Tests
{
    [TestFixture]
    public class JavaScriptFunctionCallTest
    {
        JavaScriptFunctionCall functionCall;
        #pragma warning disable 0414
        dynamic js;
        #pragma warning restore 0414

        [SetUp]
        public void SetUp()
        {
            functionCall = new JavaScriptFunctionCall();
            js = functionCall;
        }

        [Test]
        public void Constructor_WithoutArgs_SetsAttributesNull()
        {
            // Assert
            Assert.IsNull(functionCall.FunctionName);
            Assert.IsNull(functionCall.Arg);
        }

        [Test]
        public void Invocation_WithoutArgs_SavesFunctionName()
        {
            // Act
            js.DoSomething();

            // Assert
            Assert.AreEqual("DoSomething", functionCall.FunctionName);
            Assert.IsNull(functionCall.Arg);
        }

        [Test]
        public void Invocation_WithArg_SavesFunctionNameAndArg()
        {
            // Act
            js.Write("hello");

            // Assert
            Assert.AreEqual("Write", functionCall.FunctionName);
            Assert.AreEqual("hello", functionCall.Arg);
        }
    }
}
