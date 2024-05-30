"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.DividerView = exports.InheritanceEdgeView = exports.ReferenceEdgeView = void 0;
/** @jsx svg */
const client_1 = require("@eclipse-glsp/client");
class ReferenceEdgeView extends client_1.PolylineEdgeViewWithGapsOnIntersections {
    renderAdditionals(edge, segments, context) {
        const additionals = super.renderAdditionals(edge, segments, context);
        if (this.shouldRenderEndArrow(edge)) {
            const p1 = segments[segments.length - 2];
            const p2 = segments[segments.length - 1];
            additionals.push(this.renderArrow(p1, p2));
        }
        if (this.shouldRenderComposition(edge)) {
            const p1 = segments[1];
            const p2 = segments[0];
            additionals.push(this.renderComposition(p1, p2));
        }
        return additionals;
    }
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    shouldRenderComposition(edge) {
        return edge.renderComposition === true;
    }
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    shouldRenderEndArrow(edge) {
        return edge.renderEndArrow === true;
    }
    renderArrow(p1, p2) {
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const arrow = ((0, client_1.svg)("path", { "class-sprotty-edge": true, "class-arrow": true, d: 'M 1,0 L 10,-4 L 4,0 L 10,4 Z', transform: `rotate(${(0, client_1.toDegrees)((0, client_1.angleOfPoint)({ x: p1.x - p2.x, y: p1.y - p2.y }))} ${p2.x} ${p2.y}) translate(${p2.x} ${p2.y})` }));
        return arrow;
    }
    renderComposition(p1, p2) {
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const arrow = ((0, client_1.svg)("path", { "class-sprotty-edge": true, "class-arrow": true, d: 'M 1,0 L 10,-4 L 19,0 L 10,4 L 1,0 Z', transform: `rotate(${(0, client_1.toDegrees)((0, client_1.angleOfPoint)({ x: p1.x - p2.x, y: p1.y - p2.y }))} ${p2.x} ${p2.y}) translate(${p2.x} ${p2.y})` }));
        return arrow;
    }
}
exports.ReferenceEdgeView = ReferenceEdgeView;
class InheritanceEdgeView extends client_1.PolylineEdgeViewWithGapsOnIntersections {
    renderAdditionals(edge, segments, context) {
        const additionals = super.renderAdditionals(edge, segments, context);
        const p1 = segments[segments.length - 2];
        const p2 = segments[segments.length - 1];
        additionals.push((0, client_1.svg)("path", { "class-sprotty-edge": true, d: 'M 1,0 L 10,-4 L 10,4 Z', transform: `rotate(${(0, client_1.toDegrees)((0, client_1.angleOfPoint)({ x: p1.x - p2.x, y: p1.y - p2.y }))} ${p2.x} ${p2.y}) translate(${p2.x} ${p2.y})` }));
        return additionals;
    }
}
exports.InheritanceEdgeView = InheritanceEdgeView;
class DividerView extends client_1.GCompartmentView {
    render(compartment, context, args) {
        const parent = compartment.parent;
        const width = (0, client_1.isBoundsAware)(parent) ? parent.bounds.width : compartment.size.width;
        return (0, client_1.svg)("path", { d: `M 0,0 L ${width},0`, "class-sprotty-node": true });
    }
}
exports.DividerView = DividerView;
//# sourceMappingURL=views.js.map