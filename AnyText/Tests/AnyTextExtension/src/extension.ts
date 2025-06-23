import * as vscode from 'vscode';
import type { Executable, LanguageClientOptions, Position, ServerOptions} from 'vscode-languageclient/node.js';
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

    client.onNotification('custom/showReferences', async (definitionPosition: Position) => {
        const uri = vscode.window.activeTextEditor?.document.uri;
        const vscodePosition = new vscode.Position(definitionPosition.line, definitionPosition.character)

        const references: vscode.Location[] = await vscode.commands.executeCommand(
            'vscode.executeReferenceProvider',
            uri,
            vscodePosition
        );

        if (references) {
            await vscode.commands.executeCommand('editor.action.showReferences', uri, vscodePosition, references);
        }
    });
    context.subscriptions.push(
        vscode.commands.registerCommand("anytext.createModel", async () => {
            const languages = context.extension.packageJSON?.contributes.languages;

            const selectedLang = await vscode.window.showQuickPick(
              languages.map((lang: any) => lang.id),
              {
                  placeHolder: "Select a language",
              }
            );
            if (!selectedLang) {
                return;
            }
            const sourceUri = await vscode.window.showOpenDialog({
                canSelectMany: false,
                filters: { Models: ["nmeta"] },
                openLabel: "Select source model",
            });
            if (!sourceUri || sourceUri.length === 0) return;
            const targetUri = await vscode.window.showSaveDialog({
                filters: { Models: [selectedLang] },
                saveLabel: "Save generated model as",
            });
            if (!targetUri) return;

            await client.sendRequest("workspace/executeCommand", {
                command: "anytext.createModel",
                arguments: [sourceUri.toString(), targetUri.toString(), selectedLang],
            });
        })
    );

    context.subscriptions.push(
        vscode.commands.registerCommand("anytext.syncModel", async () => {
            const editor = vscode.window.activeTextEditor;
            if (!editor) return;

            const firstFile = await vscode.window.showOpenDialog({
                canSelectMany: false,

                openLabel: "Select source model",
            });
            if (!firstFile  || firstFile.length === 0) return;

            const secondFile = await vscode.window.showOpenDialog({
                canSelectMany: false,

                openLabel: "Select source model",
            });
            if (!secondFile  || secondFile.length === 0) return;
            await client.sendRequest("workspace/executeCommand", {
                command: "anytext.syncModel",
                arguments: [firstFile.toString(), secondFile.toString()]
            });
        })
    );
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
