import { createNMetaDiagramContainer } from './nmeta/diagramContainer';
import {
    accessibilityModule,
    bindOrRebind,
    ConsoleLogger,
    createDiagramOptionsModule,
    IDiagramOptions,
    LogLevel,
    STANDALONE_MODULE_CONFIG,
    TYPES,
    toolPaletteModule
} from '@eclipse-glsp/client';
import { Container } from 'inversify';
import './css/diagram.css';
export function createContainer(options: IDiagramOptions): Container {
    const container = createNMetaDiagramContainer(
        createDiagramOptionsModule(options),
        {
            add: accessibilityModule,
            remove: toolPaletteModule
        },
        STANDALONE_MODULE_CONFIG
    );
    bindOrRebind(container, TYPES.ILogger).to(ConsoleLogger).inSingletonScope();
    bindOrRebind(container, TYPES.LogLevel).toConstantValue(LogLevel.warn);
    container.bind(TYPES.IMarqueeBehavior).toConstantValue({ entireEdge: true, entireElement: true });
    return container;
}
