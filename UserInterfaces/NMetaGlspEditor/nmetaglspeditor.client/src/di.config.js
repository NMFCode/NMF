"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.createContainer = void 0;
const diagramContainer_1 = require("./nmeta/diagramContainer");
const client_1 = require("@eclipse-glsp/client");
require("./css/diagram.css");
function createContainer(options) {
    const container = (0, diagramContainer_1.createNMetaDiagramContainer)((0, client_1.createDiagramOptionsModule)(options), {
        add: client_1.accessibilityModule,
        remove: client_1.toolPaletteModule
    }, client_1.STANDALONE_MODULE_CONFIG);
    (0, client_1.bindOrRebind)(container, client_1.TYPES.ILogger).to(client_1.ConsoleLogger).inSingletonScope();
    (0, client_1.bindOrRebind)(container, client_1.TYPES.LogLevel).toConstantValue(client_1.LogLevel.warn);
    container.bind(client_1.TYPES.IMarqueeBehavior).toConstantValue({ entireEdge: true, entireElement: true });
    return container;
}
exports.createContainer = createContainer;
//# sourceMappingURL=di.config.js.map