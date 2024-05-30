import { EditableLabel, LayoutContainer, Nameable, RectangularNode, GChildElement, GShapeElement, WithEditableLabel, GLabel } from '@eclipse-glsp/client';
export declare class DefaultNode extends RectangularNode implements Nameable, WithEditableLabel {
    static readonly DEFAULT_FEATURES: symbol[];
    get editableLabel(): (GChildElement & EditableLabel) | undefined;
    get name(): string;
}
export declare class ElementLabel extends GLabel {
    static readonly DEFAULT_FEATURES: symbol[];
}
export declare class AttributeLabel extends GLabel {
    static readonly DEFAULT_FEATURES: symbol[];
}
export declare class EdgeLabel extends GLabel {
    static readonly DEFAULT_FEATURES: symbol[];
}
export declare class Icon extends GShapeElement implements LayoutContainer {
    static readonly DEFAULT_FEATURES: symbol[];
    layout: string;
    size: {
        width: number;
        height: number;
    };
}
//# sourceMappingURL=model.d.ts.map