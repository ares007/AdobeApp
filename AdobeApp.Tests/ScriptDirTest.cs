using AdobeApp;
using NUnit.Framework;
using System;
using System.IO;

namespace AdobeApp.Tests
{
    [TestFixture]
    public class ScriptDirTest
    {
        private ScriptDir scriptDir;

        #region Setup Teardown
        [SetUp]
        public void SetUp()
        {
            scriptDir = new ScriptDir();
        }

        [TearDown]
        public void TearDown()
        {
            if (scriptDir != null)
            {
                scriptDir.Dispose();
                scriptDir = null;
            }
        }
        #endregion

        #region Constuctor Tests
        [Test]
        public void Constructor_WithGivenDir_SavesDir()
        {
            // Arrange
            var tempDir = CreateTempDir();
            var dir = new ScriptDir(tempDir);

            // Assert
            Assert.AreEqual(tempDir, dir.Dir);

            // Away
            RemoveTempDir(tempDir);
        }

        [Test]
        public void Constructor_WithDivenDir_DoesNotRemoveDir()
        {
            // Arrange
            var tempDir = CreateTempDir();

            // Act
            var insideRan = false;
            using (var dir = new ScriptDir(tempDir))
            {
                insideRan = true;
            }
                
            // Assert
            Assert.IsTrue(insideRan);
            Assert.IsTrue(Directory.Exists(tempDir));

            // Away
            RemoveTempDir(tempDir);
        }

        [Test]
        public void Constructor_WithoutGivenDir_CreatesTempDir()
        {
            // Assert
            Assert.IsTrue(Directory.Exists(scriptDir.Dir));
        }

        [Test]
        public void Constructor_WithoutGivenDir_RemovesTempDir()
        {
            // Arrange
            var tempDir = scriptDir.Dir;

            // Act
            scriptDir.Dispose();

            // Assert
            Assert.IsFalse(Directory.Exists(tempDir));

            // Away
            scriptDir = null;
        }
        #endregion

        #region File Tests
        [Test]
        public void Script_ForFile_ReturnsPath()
        {
            // Act
            var path = scriptDir.Script("huhu.js");

            // Assert
            Assert.AreEqual(
                Path.Combine(scriptDir.Dir, "huhu.js"),
                path
            );
        }

        [Test]
        public void HasFile_ForMissingFile_ReportsFalse()
        {
            // Assert
            Assert.IsFalse(scriptDir.HasFile("missing.js"));
        }

        [Test]
        public void HasFile_ForExistingFile_ReportsTrue()
        {
            // Arrange
            var filePath = Path.Combine(scriptDir.Dir, "some.js");
            File.WriteAllText(filePath, "huhu");

            // Assert
            Assert.IsTrue(scriptDir.HasFile("some.js"));
        }

        [Test]
        public void GetFile_ForMissingFile_Dies()
        {
            // Assert
            Assert.Throws(
                typeof(FileNotFoundException),
                () => scriptDir.GetFile("missing.js")
            );
        }

        [Test]
        public void GetFile_ForExistingFile_ReturnsContent()
        {
            // Arrange
            File.WriteAllText(
                Path.Combine(scriptDir.Dir, "thing.js"),
                "nothing inside"
            );

            // Assert
            Assert.AreEqual("nothing inside", scriptDir.GetFile("thing.js"));
        }

        [Test]
        public void PutFile_WithContent_WritesFile()
        {
            // Act
            scriptDir.PutFile("huhu.js", "something inside");

            // Assert
            Assert.AreEqual(
                "something inside",
                File.ReadAllText(Path.Combine(scriptDir.Dir, "huhu.js"))
            );
        }

        [Test]
        public void IndexRead_ForMissingFile_Dies()
        {
            // Assert
            Assert.Throws(
                typeof(FileNotFoundException),
                () => { var x = scriptDir["missing.js"]; }
            );
        }

        [Test]
        public void IndexRead_ForExistingFile_ReturnsContent()
        {
            // Arrange
            File.WriteAllText(
                Path.Combine(scriptDir.Dir, "file.js"),
                "file in it"
            );

            // Assert
            Assert.AreEqual("file in it", scriptDir["file.js"]);
        }

        [Test]
        public void IndexWrite_WithContent_CreatesFile()
        {
            // Act
            scriptDir["something.js"] = "huhu";

            // Assert
            Assert.AreEqual(
                "huhu",
                File.ReadAllText(Path.Combine(scriptDir.Dir, "something.js"))
            );
        }
        #endregion

        #region helpers
        private string CreateTempDir()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);

            return tempDir;
        }

        private void RemoveTempDir(string dir)
        {
            Directory.Delete(dir, true);
        }
        #endregion
    }
}
