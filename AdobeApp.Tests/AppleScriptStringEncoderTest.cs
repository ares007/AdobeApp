using AdobeApp;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdobeApp.Tests
{
    [TestFixture]
    public class AppleScriptStringEncoderTest
    {
        private const string UTXT_PREFIX = "«data utxt";
        private const string UTXT_SUFFIX = "» as Unicode text";

        private const string HEX4 = "[0-9A-F][0-9A-F][0-9A-F][0-9A-F]";
        private readonly string UTXT_REGEX = $"\\A{UTXT_PREFIX}({HEX4})*{UTXT_SUFFIX}\\z";
        
        [Test]
        public void TuUtxt_FromString_ReturnsHexEncoded()
        {
            // Act
            var encoded = AppleScriptStringEncoder.ToUtxt("abcö");

            // Assert
            Assert.AreEqual($"{UTXT_PREFIX}00610062006300F6{UTXT_SUFFIX}", encoded);
        }

        [TestCase("abc", 1)]
        [TestCase("abcdefghij", 1)]
        [TestCase("abcdefghijk", 2)]
        [TestCase("abcdefghijöäüÖÄÜßñëf", 2)]
        [TestCase("abcdefghijöäüÖÄÜßñëfg", 3)]
        [Test]
        public void SplitIntoChunks_FromShortString_ReturnsOneChunk(string text, int nrChunks)
        {
            // Act
            var chunks = AppleScriptStringEncoder.SplitIntoChunks(text, 10);

            // Assert
            Assert.AreEqual(nrChunks, chunks.Count());
        }

        [TestCase("abc")]
        [TestCase("abcdefghij0123456789ABCDEFGHIJ")]
        [Test]
        public void SplitIntoChunks_FromString_ReturnsUtxtString(string text)
        {
            // Act
            var chunks = AppleScriptStringEncoder.SplitIntoChunks(text, 5);

            // Assert
            chunks.All(c => Regex.IsMatch(c, UTXT_REGEX));
        }
    }
}
