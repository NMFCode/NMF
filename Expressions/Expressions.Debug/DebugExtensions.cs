using System.Diagnostics;
using System.IO;

namespace NMF.Expressions.Debug
{
    /// <summary>
    /// Denotes a class with debug helper functions
    /// </summary>
    public static class DebugExtensions
    {
        /// <summary>
        /// Visualizes the provided DDG node
        /// </summary>
        /// <param name="node">The DDG node to visualize</param>
        /// <remarks>This will generate a DGML file and open it. Therefore, a DGML viewer must be installed (e.g. Visual Studio with the required packages).</remarks>
        [Conditional("DEBUG")]
        public static void Visualize(this INotifiable node)
        {
#pragma warning disable S5445 // Insecure temporary file creation methods should not be used
            var file = Path.GetTempFileName();
#pragma warning restore S5445 // Insecure temporary file creation methods should not be used
            File.Delete(file);
            file = Path.ChangeExtension(file, ".dgml");
            File.WriteAllText(file, DgmlExporter.Export(node));
            var process = new Process();
            process.StartInfo.FileName = file;
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }

        /// <summary>
        /// Exports the provided notifiable node to DGML and saves it
        /// </summary>
        /// <param name="node">The DDG node to export</param>
        /// <param name="path">The path where to export the file to</param>
        public static void ExportToDgml(this INotifiable node, string path)
        {
            File.WriteAllText(path, DgmlExporter.Export(node));
        }
    }
}
