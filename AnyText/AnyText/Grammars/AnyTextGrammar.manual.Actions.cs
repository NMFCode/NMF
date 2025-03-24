using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using NMF.AnyText.Metamodel;
using NMF.AnyText.Rules;
using NMF.AnyText.Workspace;
using NMF.Expressions.Linq;
using FileOptions = NMF.AnyText.Workspace.FileOptions;

namespace NMF.AnyText.Grammars
{
    public partial class AnyTextGrammar
    {
        public partial class ModelRuleRule
        {
            /// <inheritdoc />
            protected override IEnumerable<CodeActionInfo<ModelRule>> CodeActions => new List<CodeActionInfo<ModelRule>>
            {
                new()
                {
                    Title = "Copy to new File",
                    Kind = "quickfix",
                    WorkspaceEdit = (m, args) =>
                    {
                        return new WorkspaceEdit
                        {
                            DocumentChanges = new List<DocumentChange>
                            {
                                new()
                                {
                                    CreateFile = new CreateFile
                                    {
                                        Options = new FileOptions
                                        {
                                            IgnoreIfExists = false,
                                            Overwrite = true
                                        },
                                        AnnotationId = "createFile",
                                        Kind = "create",
                                        Uri = $"{m.Name}.anytext"
                                    }
                                },
                                new()
                                {
                                    TextDocumentEdit = new TextDocumentEdit
                                    {
                                        TextDocument = new OptionalVersionedTextDocumentIdentifier
                                        {
                                            Version = 1,
                                            Uri = $"{m.Name}.anytext"
                                        },
                                        Edits = new[]
                                        {
                                            new TextEdit(new ParsePosition(0, 0), new ParsePosition(0, 0),
                                                new[] { Synthesize(m, args.Context) })
                                        }
                                    }
                                }
                            },
                            ChangeAnnotations = new Dictionary<string, ChangeAnnotation>
                            {
                                {
                                    "createFile", new ChangeAnnotation
                                    {
                                        Description = "description",
                                        Label = "label",
                                        NeedsConfirmation = true
                                    }
                                }
                            }
                        };
                    },
                    Diagnostics = new[] { "" },
                    
                }
            };
        }

        public partial class GrammarRule
        {
            /// <inheritdoc />
            protected override IEnumerable<CodeActionInfo<Metamodel.Grammar>> CodeActions => new List<CodeActionInfo<Metamodel.Grammar>>
            {
                new()
                {
                    Title = "Generate comment header",
                    Kind = "refactor.extract",
                    CommandTitle = "Insert Comment Header",
                    WorkspaceEdit = null,
                    Diagnostics = new[] { "" },
                    CommandIdentifier = "editor.action.addCommentHeader",
                    Action = (g, obj) =>
                    {
                        var documentUri = obj.DocumentUri;
                        if (documentUri != null)
                        {
                            InsertCommentHeader(documentUri);
                        }
                    }
                }
            };
        }

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
    }
}