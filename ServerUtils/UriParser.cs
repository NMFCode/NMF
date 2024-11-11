using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public class UriParser
    {
        public static bool TryGetFilePath(Uri uri, out string absoluteFilePath)
        {
            var unescapedUri = Uri.UnescapeDataString(uri.AbsolutePath);
            if (!File.Exists(unescapedUri))
            {
                unescapedUri = RemoveLeadingSlash(unescapedUri);
            }

            absoluteFilePath = unescapedUri;
            return File.Exists(unescapedUri);
        }

        private static string RemoveLeadingSlash(string unescapedUri)
        {
            if (unescapedUri.StartsWith("/") || unescapedUri.StartsWith("\\"))
            {
                return unescapedUri.Substring(1);
            }
            return unescapedUri;
        }
    }
}
