using System;
using System.Linq;
using System.IO;

namespace AdobeApp
{
    /// <summary>
    /// A temporary directory holding all JavaScript files
    /// </summary>
    /// <example>
    /// var dir = new ScriptDir();
    /// dir.Populate(javaSctiptCollection);
    /// dir.Save("/path/to/file.js");
    /// 
    /// </example>
    public class ScriptDir: IDisposable
    {
        /// <summary>
        /// Directory
        /// </summary>
        /// <value>Full path to the temporary directory</value>
        public string Dir { get; private set; }
        private bool isRandomDir;

        /// <summary>
        /// Initializes a new JavaScriptDir instance, creates a temporary directory and collects JavaScript files
        /// </summary>
        public ScriptDir()
        {
            Dir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(Dir);
            isRandomDir = true;
        }

        /// <summary>
        /// Initializes a new JavaScriptDir instance using an existing directory and collects JavaScript files
        /// </summary>
        /// <param name="dir">Full path to an existing directory</param>
        public ScriptDir(string dir)
        {
            Dir = dir;
            isRandomDir = false;
        }

        /// <summary>
        /// Fills the directory with all files from a given collector
        /// </summary>
        /// <param name="collection">JavaScript collection to populate from</param>
        public void Populate(JavaScriptCollection collection)
        {
            collection.Files().ToList()
                .ForEach(f => PutFile(f, collection[f]));
        }

        /// <summary>
        /// allow accessing the files using [] notation
        /// </summary>
        /// <param name="fileName">File name.</param>
        public string this[string fileName]
        {
            get { return GetFile(fileName); }
            set { PutFile(fileName, value); }
        }

        /// <summary>
        /// finds out if a given file is in the collection
        /// </summary>
        /// <returns><c>true</c> if the file is contained in the collection</returns>
        /// <param name="fileName">File name.</param>
        public Boolean HasFile(string fileName)
        {
            return File.Exists(Script(fileName));
        }

        /// <summary>
        /// Get the contents of a named file inside script Directory
        /// </summary>
        /// <param name="fileName">File name.</param>
        public string GetFile(string fileName)
        {
            return File.ReadAllText(Script(fileName));
        }

        /// <summary>
        /// Put some content into the Directory and save it under the given filename
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="content">Content.</param>
        public void PutFile(string fileName, string content)
        {
            File.WriteAllText(Script(fileName), content);
        }

        /// <summary>
        /// Generates a path to a file inside dir.
        /// </summary>
        /// <returns>Full path to the file inside the directory</returns>
        /// <param name="fileName">File name.</param>
        public string Script(string fileName)
        {
            return Path.Combine(Dir, fileName);
        }

        public void Dispose()
        {
            if (isRandomDir && Directory.Exists(Dir))
            {
                Directory.Delete(Dir, true);
            }
        }
    }
}
