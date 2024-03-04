import {
    ConsoleLogger,
    ContainerConfiguration,
    DeleteElementContextMenuItemProvider,
    GEdge,
    GridSnapper,
    LogLevel,
    RevealNamedElementActionProvider,
    GCompartment,
    GCompartmentView,
    GLabelView,
    TYPES,
    bindAsService,
    bindOrRebind,
    configureDefaultModelElements,
    configureModelElement,
    initializeDiagramContainer,
    RectangularNodeView,
} from '@eclipse-glsp/client';
import 'balloon-css/balloon.min.css';
import { Container, ContainerModule } from 'inversify';
import 'sprotty/css/edit-label.css';
import '../css/diagram.css';
import { AttributeLabel, DefaultNode, ElementLabel } from './model';
import { DividerView, InheritanceEdgeView, ReferenceEdgeView } from './views';
import { ContextMenuService } from './menu';

export const nmetaDiagramModule = new ContainerModule((bind, unbind, isBound, rebind) => {
    const context = { bind, unbind, isBound, rebind };

    bindOrRebind(context, TYPES.ILogger).to(ConsoleLogger).inSingletonScope();
    bindOrRebind(context, TYPES.LogLevel).toConstantValue(LogLevel.warn);
    bind(TYPES.ISnapper).to(GridSnapper);
    bindAsService(context, TYPES.ICommandPaletteActionProvider, RevealNamedElementActionProvider);
    bindAsService(context, TYPES.IContextMenuItemProvider, DeleteElementContextMenuItemProvider);
    bindOrRebind(context, TYPES.IContextMenuService).to(ContextMenuService);

    configureDefaultModelElements(context);
    configureModelElement(context, 'label', AttributeLabel, GLabelView);
    configureModelElement(context, 'comp:header', GCompartment, GCompartmentView);
    configureModelElement(context, 'comp:attributes', GCompartment, GCompartmentView);
    configureModelElement(context, 'comp:operations', GCompartment, GCompartmentView);
    configureModelElement(context, 'comp:literals', GCompartment, GCompartmentView);
    configureModelElement(context, 'Class', DefaultNode, RectangularNodeView);
    configureModelElement(context, 'Enumeration', DefaultNode, RectangularNodeView);
    configureModelElement(context, 'Namespace', DefaultNode, RectangularNodeView);
    configureModelElement(context, 'Literal', ElementLabel, GLabelView);
    configureModelElement(context, 'Attribute', ElementLabel, GLabelView);
    configureModelElement(context, 'Operation', ElementLabel, GLabelView);
    configureModelElement(context, 'Reference', GEdge, ReferenceEdgeView);
    configureModelElement(context, 'comp:divider', GCompartment, DividerView);

    configureModelElement(context, 'edge:inheritance', GEdge, InheritanceEdgeView);
});

export function createNMetaDiagramContainer(...containerConfiguration: ContainerConfiguration): Container {
    return initializeDiagramContainer(new Container(), nmetaDiagramModule, ...containerConfiguration);
}
