using System.Collections.Generic;
using System.Threading.Tasks;
using NMF.AnyText.Rules;

namespace NMF.AnyText
{
    /// <summary>
    /// Represents the arguments for executing a command on a document.
    /// </summary>
    public abstract class ExecuteCommandArguments 
    {
        /// <summary>
        /// RuleApplication of the Action
        /// </summary>
        public RuleApplication RuleApplication { get; set; }
        
        /// <summary>
        /// ParseContext of the Document
        /// </summary>
        public ParseContext Context { get; set; }
        
        /// <summary>
        /// URI of the document.
        /// </summary>
        public string DocumentUri { get; set; }
        
        /// <summary>
        /// Starting position of the Range.
        /// </summary>
        public ParsePosition Start { get; set; }
        
        /// <summary>
        /// Ending position of the Range.
        /// </summary>
        public ParsePosition End { get; set; }
        
        /// <summary>
        /// Additional options for the command execution.
        /// </summary>
        public Dictionary<string, object> OtherOptions { get; set; }

        /// <summary>
        /// Sends a log to the client
        /// </summary>
        /// <param name="message">the message to log</param>
        /// <param name="messageType">the type of the message</param>
        public abstract Task SendLog(string message, MessageType messageType = MessageType.Info);

        /// <summary>
        /// Shows a message to the client
        /// </summary>
        /// <param name="message">the message to show</param>
        /// <param name="messageType">the type of the message</param>
        /// <param name="buttons">the available buttons for the user</param>
        public abstract Task<string> ShowRequest(string message, MessageType messageType = MessageType.Info, params string[] buttons);

        /// <summary>
        /// Shows a notification to the client
        /// </summary>
        /// <param name="message">the notification</param>
        /// <param name="messageType">the type of the message</param>
        public abstract Task ShowNotification(string message, MessageType messageType = MessageType.Info);

        /// <summary>
        /// Requests a client to open a document
        /// </summary>
        /// <param name="uri">The URI of the document to show.</param>
        /// <param name="selection">The optional selection range in the document.</param>
        /// <param name="external">If true, requests to open the document externally.</param>
        /// <param name="takeFocus">If true, requests the client to take focus.</param>
        public abstract Task ShowDocument(string uri, ParseRange? selection = null, bool external = false,
            bool takeFocus = false);

        /// <summary>
        /// Requests a client to show references of a symbol
        /// </summary>
        /// <param name="position">The position of the symbol in the document</param>
        public abstract Task ShowReferences(ParsePosition position);
    }
}