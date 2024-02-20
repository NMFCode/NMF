import {
    ConsoleLogger,
    ContainerConfiguration,
    DefaultTypes,
    DeleteElementContextMenuItemProvider,
    GEdge,
    GGraph,
    GLSPProjectionView,
    GridSnapper,
    LogLevel,
    RevealNamedElementActionProvider,
    RoundedCornerNodeView,
    GCompartment,
    GCompartmentView,
    GLabel,
    GLabelView,
    TYPES,
    bindAsService,
    bindOrRebind,
    configureDefaultModelElements,
    configureModelElement,
    editLabelFeature,
    initializeDiagramContainer,
    GEdgeView,
    configureViewerOptions,
    selectFeature,
    deletableFeature,
    moveFeature,
    RectangularNodeView
} from '@eclipse-glsp/client';
import 'balloon-css/balloon.min.css';
import { Container, ContainerModule } from 'inversify';
import 'sprotty/css/edit-label.css';
import '../css/diagram.css';
import { DefaultNode } from './model';
import { InheritanceEdgeView, ReferenceEdgeView } from './views';

export const nmetaDiagramModule = new ContainerModule((bind, unbind, isBound, rebind) => {
    const context = { bind, unbind, isBound, rebind };

    bindOrRebind(context, TYPES.ILogger).to(ConsoleLogger).inSingletonScope();
    bindOrRebind(context, TYPES.LogLevel).toConstantValue(LogLevel.warn);
    bind(TYPES.ISnapper).to(GridSnapper);
    bindAsService(context, TYPES.ICommandPaletteActionProvider, RevealNamedElementActionProvider);
    bindAsService(context, TYPES.IContextMenuItemProvider, DeleteElementContextMenuItemProvider);

    configureDefaultModelElements(context);
    configureModelElement(context, 'label', GLabel, GLabelView, { enable: [moveFeature, editLabelFeature, selectFeature] });
    configureModelElement(context, 'comp:header', GCompartment, GCompartmentView);
    configureModelElement(context, 'comp:attributes', GCompartment, GCompartmentView);
    configureModelElement(context, 'comp:literals', GCompartment, GCompartmentView);
    configureModelElement(context, 'Class', DefaultNode, RectangularNodeView, {enable: [selectFeature]});
    configureModelElement(context, 'Enumeration', DefaultNode, RectangularNodeView, {enable: [selectFeature]});
    configureModelElement(context, 'ChildNamespace', DefaultNode, RoundedCornerNodeView, {enable: [selectFeature]});
    configureModelElement(context, 'Literal', GLabel, GLabelView, { enable: [editLabelFeature, deletableFeature, selectFeature] });
    configureModelElement(context, 'Attribute', GLabel, GLabelView, { enable: [editLabelFeature, deletableFeature, selectFeature] });
    configureModelElement(context, 'Reference', GEdge, ReferenceEdgeView);

    configureModelElement(context, 'edge:inheritance', GEdge, InheritanceEdgeView);
});

export function createNMetaDiagramContainer(...containerConfiguration: ContainerConfiguration): Container {
    return initializeDiagramContainer(new Container(), nmetaDiagramModule, ...containerConfiguration);
}
