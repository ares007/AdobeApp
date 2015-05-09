using AdobeApp;
using NUnit.Framework;
using System;
using System.Linq;

namespace AdobeApp.Tests
{
    [TestFixture]
    public class JavaScriptCollectionTest
    {
        JavaScriptCollection javaScripts;

        [SetUp]
        public void SetUp()
        {
            javaScripts = new JavaScriptCollection();
        }

        [Test]
        public void Files_Initially_ContainsFiles()
        {
            // Act
            var files = javaScripts.Files();

            // Assert
            Assert.IsTrue(files.Count() >= 2);
        }

        [Test]
        public void Files_Initially_ContainsTestJs()
        {
            // Assert
            Assert.AreEqual(
                1,
                javaScripts.Files().Count(f => f == "test.js")
            );
        }

        [Test]
        public void Files_Initially_ContainsAdobeJs()
        {
            // Assert
            Assert.AreEqual(
                1,
                javaScripts.Files().Count(f => f == "adobe.js")
            );
        }

        [Test]
        public void HasFile_unknownjs_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(javaScripts.HasFile("unknown.js"));
        }

        [Test]
        public void HasFile_testjs_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(javaScripts.HasFile("test.js"));
        }

        [Test]
        public void HasFile_testJS_ReturnsTrue()
        {
            // Assert
            Assert.IsFalse(javaScripts.HasFile("test.JS"));
        }

    }
}
