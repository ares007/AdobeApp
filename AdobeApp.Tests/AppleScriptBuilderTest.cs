using AdobeApp;
using NUnit.Framework;
using System;

namespace AdobeApp.Tests
{
    [TestFixture]
    public class AppleScriptBuilderTest
    {
        private AppleScriptBuilder appleScript;

        [SetUp]
        public void SetUp()
        {
            appleScript = new AppleScriptBuilder();
        }

        [Test]
        public void ToString_Initially_CreatesEmptyScript()
        {
            // Assert
            Assert.AreEqual("", appleScript.ToString());
        }

        [Test]
        public void Tell_Only_CreatesTellBlock()
        {
            // Act
            appleScript.Tell("Xxx");

            // Assert
            Assert.AreEqual(
                "tell application \"Xxx\"\n" + 
                "end tell\n", 
                appleScript.ToString()
            );
        }

        [Test]
        public void TellTimeout_Both_CreateNestedBlocks()
        {
            // Act
            appleScript.Tell("Xxx").Timeout(42);

            // Assert
            Assert.AreEqual(
                "tell application \"Xxx\"\n" + 
                "with timeout of 42 seconds\n" +
                "end timeout\n" +
                "end tell\n", 
                appleScript.ToString()
            );
        }


        [Test]
        public void TellAssign_Both_CreateStatementInsideBlock()
        {
            // Act
            appleScript.Tell("Xxx").Assign("result", "42");

            // Assert
            Assert.AreEqual(
                "tell application \"Xxx\"\n" + 
                "set result to 42\n" +
                "end tell\n", 
                appleScript.ToString()
            );
        }
    }
}

