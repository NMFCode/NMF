"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Icon = exports.EdgeLabel = exports.AttributeLabel = exports.ElementLabel = exports.DefaultNode = void 0;
const client_1 = require("@eclipse-glsp/client");
class DefaultNode extends client_1.RectangularNode {
    get editableLabel() {
        const headerComp = this.children.find(element => element.type === 'comp:header');
        if (headerComp) {
            const label = headerComp.children.find(element => element.type === 'label');
            if (label && (0, client_1.isEditableLabel)(label)) {
                return label;
            }
        }
        return undefined;
    }
    get name() {
        var _a;
        const labelText = (_a = this.editableLabel) === null || _a === void 0 ? void 0 : _a.text;
        return labelText ? labelText : '<unknown>';
    }
}
exports.DefaultNode = DefaultNode;
DefaultNode.DEFAULT_FEATURES = [
    client_1.connectableFeature,
    client_1.deletableFeature,
    client_1.selectFeature,
    client_1.boundsFeature,
    client_1.moveFeature,
    client_1.layoutContainerFeature,
    client_1.fadeFeature,
    client_1.hoverFeedbackFeature,
    client_1.popupFeature,
    client_1.nameFeature,
    client_1.withEditLabelFeature
];
class ElementLabel extends client_1.GLabel {
}
exports.ElementLabel = ElementLabel;
ElementLabel.DEFAULT_FEATURES = [
    ...client_1.GLabel.DEFAULT_FEATURES,
    client_1.editLabelFeature,
    client_1.selectFeature,
    client_1.deletableFeature
];
class AttributeLabel extends client_1.GLabel {
}
exports.AttributeLabel = AttributeLabel;
AttributeLabel.DEFAULT_FEATURES = [
    ...client_1.GLabel.DEFAULT_FEATURES,
    client_1.editLabelFeature
];
class EdgeLabel extends client_1.GLabel {
}
exports.EdgeLabel = EdgeLabel;
EdgeLabel.DEFAULT_FEATURES = [
    ...client_1.GLabel.DEFAULT_FEATURES,
    client_1.editLabelFeature,
    client_1.moveFeature,
];
class Icon extends client_1.GShapeElement {
    constructor() {
        super(...arguments);
        this.size = {
            width: 32,
            height: 32
        };
    }
}
exports.Icon = Icon;
Icon.DEFAULT_FEATURES = [client_1.boundsFeature, client_1.layoutContainerFeature, client_1.layoutableChildFeature, client_1.fadeFeature];
//# sourceMappingURL=model.js.map