import * as vscode from 'vscode';
import type { LanguageClientOptions, Position,  StreamInfo} from 'vscode-languageclient/node.js';
import { LanguageClient } from 'vscode-languageclient/node.js';
import * as path from 'node:path';
import * as os from 'os';
import { WebSocketServerLauncher } from './WebSocketServerLauncher';
import { WebSocketStream } from './WebSocketStream';
import { getContributedLanguages } from './extensionMetadata';
import {
    GlspVscodeConnector,
    SocketGlspVscodeServer,
    configureDefaultCommands
} from '@eclipse-glsp/vscode-integration/node';
import NMetaEditorProvider from './nmeta-editor-providor';
let client: LanguageClient;

export async function activate(context: vscode.ExtensionContext)
{
    const isWindows = os.platform() === 'win32';
    const serverModule = context.asAbsolutePath(path.join('srv', `AnyTextGrammarServer`));
    const executablePath = isWindows ? `${serverModule}.exe` : serverModule;
  /*const server: Executable =
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
    }*/

    const serverLauncher = new WebSocketServerLauncher({
        executable: executablePath,
        useDotnet: false,
        logging: true
    });

    await serverLauncher.start();
    const serverOptions = (path = '/lsp'): Promise<StreamInfo> => {
    return new Promise((resolve, reject) => {
        const wsUrl = serverLauncher.getWebSocketUrl(path);
        const socket = new WebSocket(wsUrl);

        socket.onopen = () => {
            const stream = new WebSocketStream(socket);
            resolve({
                reader: stream,
                writer: stream
            });
        };
        socket.onerror = (err) =>{
        reject(err);
        }
    });
};


    const fileSystemWatcher = vscode.workspace.createFileSystemWatcher('**/*.any*');
    context.subscriptions.push(fileSystemWatcher);
    const documentSelector = getContributedLanguages().map(lang => ({ language: lang.id }));

    let clientOptions: LanguageClientOptions =
    {
        // Register the server for plain text documents
        documentSelector,
        synchronize: {
            fileEvents: fileSystemWatcher
        }
    };

    client = new LanguageClient('AnyText', ()=> serverOptions("/lsp"), clientOptions);
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
        vscode.commands.registerCommand("anytext.createModelSync", async () => {
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
                command: "anytext.createModelSync",
                arguments: [sourceUri.toString(), targetUri.toString(), selectedLang],
            });
        })
    );

    context.subscriptions.push(
        vscode.commands.registerCommand("anytext.syncModel", async (path) => {
            const editor = vscode.window.activeTextEditor;
            if (!editor) return;

            await client.sendRequest("workspace/executeCommand", {
                command: "anytext.syncModel",
                arguments: [path]
            });
        })
    );
    console.log('LSP server example is now active!');




    const nmetaServer = new SocketGlspVscodeServer({
		clientId: "vscode",
		clientName: "vscode",
		connectionOptions: {
			port: serverLauncher.getResolvedPort() ?? 0,
			path: "glsp",
		},
    });

    // Initialize GLSP-VSCode connector with server wrapper
    const glspVscodeConnector = new GlspVscodeConnector({
		server: nmetaServer,
		logging: true,
    });

    const customEditorProvider = vscode.window.registerCustomEditorProvider(
		"nmeta.glspDiagram",
		new NMetaEditorProvider(context, glspVscodeConnector),
		{
			webviewOptions: { retainContextWhenHidden: true },
			supportsMultipleEditorsPerDocument: false,
		}
    );

    context.subscriptions.push(
		nmetaServer,
		glspVscodeConnector,
		customEditorProvider
    );
    nmetaServer.start();

    configureDefaultCommands({
		extensionContext: context,
		connector: glspVscodeConnector,
		diagramPrefix: "nmeta",
    });




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
