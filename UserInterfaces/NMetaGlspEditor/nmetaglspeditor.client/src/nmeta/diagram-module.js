"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.createNMetaDiagramContainer = exports.nmetaDiagramModule = void 0;
const client_1 = require("@eclipse-glsp/client");
require("balloon-css/balloon.min.css");
const inversify_1 = require("inversify");
require("sprotty/css/edit-label.css");
require("../css/diagram.css");
const model_1 = require("./model");
const views_1 = require("./views");
const menu_1 = require("./menu");
exports.nmetaDiagramModule = new inversify_1.ContainerModule((bind, unbind, isBound, rebind) => {
    const context = { bind, unbind, isBound, rebind };
    (0, client_1.bindOrRebind)(context, client_1.TYPES.ILogger).to(client_1.ConsoleLogger).inSingletonScope();
    (0, client_1.bindOrRebind)(context, client_1.TYPES.LogLevel).toConstantValue(client_1.LogLevel.warn);
    bind(client_1.TYPES.ISnapper).to(client_1.GridSnapper);
    (0, client_1.bindAsService)(context, client_1.TYPES.ICommandPaletteActionProvider, client_1.RevealNamedElementActionProvider);
    (0, client_1.bindAsService)(context, client_1.TYPES.IContextMenuItemProvider, client_1.DeleteElementContextMenuItemProvider);
    (0, client_1.bindOrRebind)(context, client_1.TYPES.IContextMenuService).to(menu_1.ContextMenuService);
    (0, client_1.configureDefaultModelElements)(context);
    (0, client_1.configureModelElement)(context, 'label', model_1.AttributeLabel, client_1.GLabelView);
    (0, client_1.configureModelElement)(context, 'comp:header', client_1.GCompartment, client_1.GCompartmentView);
    (0, client_1.configureModelElement)(context, 'comp:attributes', client_1.GCompartment, client_1.GCompartmentView);
    (0, client_1.configureModelElement)(context, 'comp:operations', client_1.GCompartment, client_1.GCompartmentView);
    (0, client_1.configureModelElement)(context, 'comp:literals', client_1.GCompartment, client_1.GCompartmentView);
    (0, client_1.configureModelElement)(context, 'Class', model_1.DefaultNode, client_1.RectangularNodeView);
    (0, client_1.configureModelElement)(context, 'Enumeration', model_1.DefaultNode, client_1.RectangularNodeView);
    (0, client_1.configureModelElement)(context, 'Namespace', model_1.DefaultNode, client_1.RectangularNodeView);
    (0, client_1.configureModelElement)(context, 'Literal', model_1.ElementLabel, client_1.GLabelView);
    (0, client_1.configureModelElement)(context, 'Attribute', model_1.ElementLabel, client_1.GLabelView);
    (0, client_1.configureModelElement)(context, 'Operation', model_1.ElementLabel, client_1.GLabelView);
    (0, client_1.configureModelElement)(context, 'Reference', client_1.GEdge, views_1.ReferenceEdgeView);
    (0, client_1.configureModelElement)(context, 'comp:divider', client_1.GCompartment, views_1.DividerView);
    (0, client_1.configureModelElement)(context, 'edge:inheritance', client_1.GEdge, views_1.InheritanceEdgeView);
});
function createNMetaDiagramContainer(...containerConfiguration) {
    return (0, client_1.initializeDiagramContainer)(new inversify_1.Container(), exports.nmetaDiagramModule, ...containerConfiguration);
}
exports.createNMetaDiagramContainer = createNMetaDiagramContainer;
//# sourceMappingURL=diagram-module.js.map