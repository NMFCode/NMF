"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
require("reflect-metadata");
const client_1 = require("@eclipse-glsp/client");
const di_config_1 = require("./di.config");
const port = 7052;
const id = 'glsp';
const diagramType = 'nmeta';
const clientId = 'sprotty';
const webSocketUrl = `wss://localhost:${port}/${id}`;
let glspClient;
let container;
const wsProvider = new client_1.GLSPWebSocketProvider(webSocketUrl);
wsProvider.listen({ onConnection: initialize, onReconnect: reconnect, logger: console });
async function initialize(connectionProvider, isReconnecting = false) {
    glspClient = new client_1.BaseJsonrpcGLSPClient({ id, connectionProvider });
    container = (0, di_config_1.createContainer)({ clientId, diagramType, glspClientProvider: async () => glspClient, sourceUri: 'about:nmeta' });
    const actionDispatcher = container.get(client_1.GLSPActionDispatcher);
    const diagramLoader = container.get(client_1.DiagramLoader);
    await diagramLoader.load({ requestModelOptions: { isReconnecting } });
    if (isReconnecting) {
        const message = `Connection to the ${id} glsp server got closed. Connection was successfully re-established.`;
        const timeout = 5000;
        const severity = 'WARNING';
        actionDispatcher.dispatchAll([client_1.StatusAction.create(message, { severity, timeout }), client_1.MessageAction.create(message, { severity })]);
        return;
    }
}
async function reconnect(connectionProvider) {
    glspClient.stop();
    initialize(connectionProvider, true /* isReconnecting */);
}
//# sourceMappingURL=main.js.map