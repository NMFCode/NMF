/** @jsx svg */
import { GCompartment, GCompartmentView, GEdge, IViewArgs, Point, PolylineEdgeViewWithGapsOnIntersections, RenderingContext } from '@eclipse-glsp/client';
import { VNode } from 'snabbdom';
export declare class ReferenceEdgeView extends PolylineEdgeViewWithGapsOnIntersections {
    protected renderAdditionals(edge: GEdge, segments: Point[], context: RenderingContext): VNode[];
    protected shouldRenderComposition(edge: any): boolean;
    protected shouldRenderEndArrow(edge: any): boolean;
    protected renderArrow(p1: Point, p2: Point): VNode;
    protected renderComposition(p1: Point, p2: Point): VNode;
}
export declare class InheritanceEdgeView extends PolylineEdgeViewWithGapsOnIntersections {
    protected renderAdditionals(edge: GEdge, segments: Point[], context: RenderingContext): VNode[];
}
export declare class DividerView extends GCompartmentView {
    render(compartment: Readonly<GCompartment>, context: RenderingContext, args?: IViewArgs | undefined): VNode | undefined;
}
//# sourceMappingURL=views.d.ts.map