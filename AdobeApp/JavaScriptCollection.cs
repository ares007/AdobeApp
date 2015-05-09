using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace AdobeApp
{
    /// <summary>
    /// collect JavaScript files by introspecting loaded assemblies
    /// </summary>
    /// <example>
    /// var collection = new JavaScriptCollection();
    /// 
    /// var files = collection.Files();
    /// var init = collection.File("init.js");
    /// var init2 = collection["init.js"];
    /// 
    /// 
    /// </example>
    public class JavaScriptCollection
    {
        // Hint: Resource names are dot-separated.
        private readonly string JAVASCRIPT_FOLDER_NAME = ".javascript.";

        private Dictionary<string, string> javaScriptFiles;

        public JavaScriptCollection()
        {
            javaScriptFiles = new Dictionary<string, string>();

            CollectJavaScriptFiles();
        }

        /// <summary>
        /// list all file names currently in the collection
        /// </summary>
        public IEnumerable<string> Files()
        {
            return javaScriptFiles.Keys.AsEnumerable();
        }

        /// <summary>
        /// allow accessing the files using [] notation
        /// </summary>
        /// <param name="fileName">File name.</param>
        public string this[string fileName]
        {
            get { return GetFile(fileName); }
            set { javaScriptFiles[fileName] = value; }
        }

        /// <summary>
        /// return the content of a given file
        /// </summary>
        /// <param name="fileName">File name.</param>
        public string GetFile(string fileName)
        {
            return javaScriptFiles[fileName];
        }

        /// <summary>
        /// finds out if a given file is in the collection
        /// </summary>
        /// <returns><c>true</c> if the file is contained in the collection</returns>
        /// <param name="fileName">File name.</param>
        public Boolean HasFile(string fileName)
        {
            return javaScriptFiles.ContainsKey(fileName);
        }

        private void CollectJavaScriptFiles()
        {
            foreach (var assembly in ListAssemblies())
            {
                foreach (var resource in ListJavaScriptResources(assembly))
                {
                    int folderPosition = resource.LastIndexOf(JAVASCRIPT_FOLDER_NAME, StringComparison.OrdinalIgnoreCase);
                    var fileName = resource.Substring(folderPosition + JAVASCRIPT_FOLDER_NAME.Length);

                    using (var resourceStream = assembly.GetManifestResourceStream(resource))
                    using (var resourceReader = new StreamReader(resourceStream))
                    {
                        javaScriptFiles.Add(fileName, resourceReader.ReadToEnd());
                    }
                }
            }
        }

        // actually: list assemblies in call stack but this is exactly
        // the order in which we want to load JavaScript files
        private IEnumerable<Assembly> ListAssemblies()
        {
            var stack = new System.Diagnostics.StackTrace();

            return stack.GetFrames()
                .Select(f => f.GetMethod().DeclaringType.Assembly)
                .Where(a => !a.GetName().Name.StartsWith("mscorlib"))
                .Distinct();
        }

        private IEnumerable<string> ListJavaScriptResources(Assembly assembly)
        {
            return assembly.GetManifestResourceNames()
                .Where(r => r.IndexOf(JAVASCRIPT_FOLDER_NAME, StringComparison.OrdinalIgnoreCase) > 0);
        }
    }
}
