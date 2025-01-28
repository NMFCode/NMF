using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using NMF.AnyText.Metamodel;
using NMF.AnyText.Workspace;
using FileOptions = NMF.AnyText.Workspace.FileOptions;

namespace NMF.AnyText.Grammars
{
    public partial class AnyTextGrammar
    {
        public partial class FragmentRuleRule
        {
            public override IEnumerable<CodeLensInfo> SupportedCodeLenses => new List<CodeLensInfo>()
            {
                new()
                {
                    Title = "Run Test",
                    CommandIdentifier = "codelens.runTest",
                    Arguments = new Dictionary<string, object>()
                    {
                        {"test","test"},
                    },
                    
                    Action = args =>
                    {
                        var ruleApplication = args.RuleApplication;
                        var context = args.Context;
                        var semanticElement = (FragmentRule) args.RuleApplication.ContextElement;
                        var uri = args.DocumentUri;
                        Console.Error.WriteLine("TestRun"); 
                    }
                  
                }
            };
        }

        public partial class ModelRuleRule
        {
            public override IEnumerable<CodeActionInfo> SupportedCodeActions => new List<CodeActionInfo>
            {
                new()
                {
                    Title = "Copy to new File",
                    Kind = "quickfix",
                    WorkspaceEdit = (args) =>
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
                                        Uri = "newDocument.anytext"
                                    }
                                },
                                new()
                                {
                                    TextDocumentEdit = new TextDocumentEdit
                                    {
                                        TextDocument = new OptionalVersionedTextDocumentIdentifier
                                        {
                                            Version = 1,
                                            Uri = "newDocument.anytext"
                                        },
                                        Edits = new[]
                                        {
                                            new TextEdit(new ParsePosition(0, 0), new ParsePosition(0, 0),
                                                new[] { "Text in new File" })
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
            public override IEnumerable<CodeActionInfo> SupportedCodeActions => new List<CodeActionInfo>
            {
                new()
                {
                    Title = "Generate comment header",
                    Kind = "refactor.extract",
                    CommandTitle = "Insert Comment Header",
                    WorkspaceEdit = null,
                    Diagnostics = new[] { "" },
                    CommandIdentifier = "editor.action.addCommentHeader",
                    Action =obj =>
                    {
                        var documentUri = obj.DocumentUri;
                        var start = obj.Start;
                        var end = obj.End;
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