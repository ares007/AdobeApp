using AdobeApp;
using NUnit.Framework;
using System;

namespace AdobeApp.Tests
{
    [TestFixture]
    public class ApplicationTest
    {
        private Application application;
        [SetUp]
        public void SetUp()
        {
            application = new Application("app");
        }

        [Test]
        public void Constructor_Name_Builds()
        {
            // Act
            application = new Application("xname");

            // Assert
            Assert.AreEqual("xname", application.AppName);
            Assert.AreEqual(1800, application.AppleScriptTimeout);
            Assert.IsNull(application.JavaScriptFilename);
        }

        [Test]
        public void Constructor_NameAndTimeout_Builds()
        {
            // Act
            application = new Application("aname", 888);

            // Assert
            Assert.AreEqual("aname", application.AppName);
            Assert.AreEqual(888, application.AppleScriptTimeout);
            Assert.IsNull(application.JavaScriptFilename);
        }

        [Test]
        public void NameFactory_Name_Builds()
        {
            // Act
            application = Application.Name("ggg");

            // Assert
            Assert.AreEqual("ggg", application.AppName);
            Assert.AreEqual(1800, application.AppleScriptTimeout);
            Assert.IsNull(application.JavaScriptFilename);
        }

        [Test]
        public void Modifier_Timeout_ReturnsApplication()
        {
            // Act
            var result = application.Timeout(777);

            // Assert
            Assert.AreSame(result, application);
        }

        [Test]
        public void Modifier_Timeout_ChangesTimeout()
        {
            // Act
            application.Timeout(888);

            // Assert
            Assert.AreEqual("app", application.AppName);
            Assert.AreEqual(888, application.AppleScriptTimeout);
            Assert.IsNull(application.JavaScriptFilename);
        }

        [Test]
        public void Modifier_JavaScript_ReturnsApplication()
        {
            // Act
            var result = application.JavaScript("x.js");

            // Assert
            Assert.AreSame(result, application);
        }

        [Test]
        public void Modifier_JavaScript_ChangesJavaScriptFilename()
        {
            // Act
            application.JavaScript("xx.js");

            // Assert
            Assert.AreEqual("app", application.AppName);
            Assert.AreEqual(1800, application.AppleScriptTimeout);
            Assert.AreEqual("xx.js", application.JavaScriptFilename);
        }

        // For testing Execute and Run, we would need some
        // kind of Indirection for building dependencies
    }
}
