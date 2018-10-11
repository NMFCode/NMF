using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace NMF.Expressions.Debug
{
    public static class DebugExtensions
    {
        [Conditional("DEBUG")]
        public static void Visualize(this INotifiable node)
        {
            var file = Path.GetTempFileName();
            File.Delete(file);
            file = Path.ChangeExtension(file, ".dgml");
            File.WriteAllText(file, DgmlExporter.Export(node));
            var process = new Process();
            process.StartInfo.FileName = file;
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }

        public static void ExportToDgml(this INotifiable node, string path)
        {
            File.WriteAllText(path, DgmlExporter.Export(node));
        }
    }
}
