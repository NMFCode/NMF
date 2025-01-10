using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace NMF.AnyText.Grammars
{
    public partial class AnyTextGrammar
    {
        /// <summary>
        ///     List of Possible Code Actions
        /// </summary>
        public override IEnumerable<CodeActionInfo> SupportedCodeActions { get; } = new List<CodeActionInfo>
        {
            new()
            {
                Title = "Generate comment header",
                Kind = "refactor.extract",
                CommandTitle = "Insert Comment Header",
                WorkspaceEdit = null,
                Diagnostics = new[] { "" },
                Command = "editor.action.addCommentHeader",
                Arguments = new[] { "a" }
            }
        };

        /// <summary>
        ///     Dictionary of Code Action Identifier and the Executable Action
        /// </summary>
        public override Dictionary<string, Func<object[], object>> ExecutableCodeActions { get; } = new()
        {
            {
                "editor.action.addCommentHeader", obj =>
                {
                    var arguments = obj;
                    if (arguments != null && arguments.Length > 0)
                    {
                        var documentUri = (string)arguments[0];
                        var startRange = ParsePositionFromJson(arguments[1].ToString());
                        var endRange = ParsePositionFromJson(arguments[2].ToString());

                        if (documentUri != null)
                        {
                            InsertCommentHeader(documentUri);
                            return "Comment header generated.";
                        }

                        return "Invalid document URI.";
                    }

                    return "No arguments provided.";
                }
            }
        };


        private static void InsertCommentHeader(string filePath)
        {
            var uri = new Uri(Uri.UnescapeDataString(filePath), UriKind.RelativeOrAbsolute);
            var localPath = uri.LocalPath;
            var content = File.ReadAllLines(localPath);
            var commentHeader = GenerateCommentHeader();

            var updatedContent = new string[content.Length + commentHeader.Length];
            Array.Copy(commentHeader, updatedContent, commentHeader.Length);
            Array.Copy(content, 0, updatedContent, commentHeader.Length, content.Length);

            SaveDocument(localPath, updatedContent);
        }

        private static string[] GenerateCommentHeader()
        {
            var builder = new StringBuilder();
            builder.AppendLine("/*");
            builder.AppendLine(" * Description: <description>");
            builder.AppendLine(" * Author: <author>");
            builder.AppendLine(" * Date: " + DateTime.Now.ToString("yyyy-MM-dd"));
            builder.AppendLine(" * Version: 1.0");
            builder.AppendLine(" */");

            return builder.ToString().Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        private static void SaveDocument(string filePath, string[] updatedContent)
        {
            File.WriteAllLines(filePath, updatedContent);
        }

        private static ParsePosition ParsePositionFromJson(string jsonString)
        {
            var jsonDocument = JsonDocument.Parse(jsonString);

            var line = jsonDocument.RootElement.GetProperty("line").GetInt32();
            var character = jsonDocument.RootElement.GetProperty("character").GetInt32();

            return new ParsePosition(line, character);
        }
    }
}