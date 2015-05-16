using AdobeApp;
using NUnit.Framework;
using System;

namespace AdobeApp.Tests
{
    [TestFixture]
    public class AppleScriptRunnerTest
    {
        [Test]
        public void RunScript_WithReturnStatement_ReturnsResult()
        {
            // Act
            var result = AppleScriptRunner.Run("return 42");

            // Assert
            Assert.AreEqual("42\n", result);
        }
    }
}