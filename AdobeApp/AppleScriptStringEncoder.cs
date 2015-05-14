using System;
using System.Collections.Generic;
using System.Linq;

namespace AdobeApp
{
    /// <summary>
    /// Contains static helper methods for transforming strings into
    /// strange looking Unicode strings usable inside AppleScript
    /// </summary>
    /// <example>
    /// var ecoded = AppleScriptStringEncoder.ToUtxt("huhu");
    /// var chunks = AppleScriptStringEncoder.SplitIntoChunks("huhu");
    /// </example>
    public static class AppleScriptStringEncoder
    {
        /// <summary>
        /// Returns a single «data ...» AppleScript expression for the given string
        /// </summary>
        /// <returns>AppleScript expression</returns>
        /// <param name="content">String to transform</param>
        public static string ToUtxt(string text)
        {
            string encodedContent =
                String.Join(
                    "", 
                    text.ToCharArray().Select(c => ((int)c).ToString("X4"))
                );

            // Encoding to Macroman is neede before executing
            return String.Format("«data utxt{0}» as Unicode text", encodedContent);
        }

        /// <summary>
        /// Helper that splits a possibly large text into smaller junks
        /// </summary>
        /// <returns>Junks of maximum the requested junk size</returns>
        /// <param name="text">The text to split up.</param>
        /// <param name="chunkSize">Chunk size. Defaults to 40</param>
        public static IEnumerable<string> SplitIntoChunks(string text, int chunkSize = 40)
        {
            for (int i = 0; i < text.Length; i += chunkSize)
            {
                var length = i + chunkSize > text.Length
                    ? text.Length - i
                    : chunkSize;

                yield return ToUtxt(text.Substring(i, length));
            }
        }
    }
}
