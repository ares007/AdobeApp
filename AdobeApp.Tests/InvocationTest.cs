using AdobeApp;
using NUnit.Framework;
using System;

namespace AdobeApp.Tests
{
    [TestFixture]
    public class InvocationTest
    {
        private Invocation invocation;

        [SetUp]
        public void Setup()
        {
            invocation = new Invocation();
        }

        [Test]
        public void Invocation_Initially_HasAmptyFunctionList()
        {
            // Assert
            Assert.AreEqual(0, invocation.FunctionCalls.Count);
        }
    }
}
