import * as vscode from 'vscode';
import type { Executable, LanguageClientOptions, ServerOptions} from 'vscode-languageclient/node.js';
import { LanguageClient } from 'vscode-languageclient/node.js';
import * as path from 'node:path';

let client: LanguageClient;

export function activate(context: vscode.ExtensionContext)
{
    const serverModule = context.asAbsolutePath(path.join('..', 'AnyTextGrammarServer', 'bin', 'Debug', 'net8.0', 'AnyTextGrammarServer.exe'));
    
    const server: Executable =
    {
        command: serverModule,
        args: [],
        options: { shell: false, detached: false }
    };
    const serverDebug: Executable =
    {
        command: serverModule,
        args: ['debug'],
        options: { shell: false, detached: false }
    };

    const serverOptions: ServerOptions = {
        run: server,
        debug: serverDebug
    }

    const fileSystemWatcher = vscode.workspace.createFileSystemWatcher('**/*.anytext');
    context.subscriptions.push(fileSystemWatcher);

    let clientOptions: LanguageClientOptions =
    {
        // Register the server for plain text documents
        documentSelector: [
            {scheme: 'file', language: 'anytext'},
        ],
        synchronize: {
            fileEvents: fileSystemWatcher
        }
    };

    client = new LanguageClient('anytext', serverOptions, clientOptions);
    
    client.registerProposedFeatures();
    
    console.log('LSP server example is now active!');
    client.start();
}

export function deactivate(): Thenable<void> | undefined
{
    if (!client)
    {
        return undefined;
    }
    return client.stop();
}