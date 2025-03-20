import * as vscode from 'vscode';
import type { Executable, LanguageClientOptions, ServerOptions} from 'vscode-languageclient/node.js';
import { LanguageClient } from 'vscode-languageclient/node.js';
import * as path from 'node:path';
import * as os from 'os';

let client: LanguageClient;

export function activate(context: vscode.ExtensionContext)
{  
    const isWindows = os.platform() === 'win32';
    const serverModule = context.asAbsolutePath(path.join('srv', `AnyTextGrammarServer`));
    
    const executablePath = isWindows ? `${serverModule}.exe` : serverModule;

    const server: Executable =
    {
        command: executablePath,
        args: [],
        options: { shell: false, detached: false }
    };
    const serverDebug: Executable =
    {
        command: executablePath,
        args: ['debug'],
        options: { shell: false, detached: false }
    };

    const serverOptions: ServerOptions = {
        run: server,
        debug: serverDebug
    }

    const fileSystemWatcher = vscode.workspace.createFileSystemWatcher('**/*.any*');
    context.subscriptions.push(fileSystemWatcher);

    let clientOptions: LanguageClientOptions =
    {
        // Register the server for plain text documents
        documentSelector: [
            {language: 'anytext'},
            {language: 'anymeta'}
        ],
        synchronize: {
            fileEvents: fileSystemWatcher
        }
    };

    client = new LanguageClient('AnyText', serverOptions, clientOptions);
    
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