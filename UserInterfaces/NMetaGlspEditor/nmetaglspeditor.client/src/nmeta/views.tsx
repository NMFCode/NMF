/** @jsx svg */
import {
    GCompartment,
    GCompartmentView,
    GEdge,
    IViewArgs,
    Point,
    PolylineEdgeViewWithGapsOnIntersections,
    RenderingContext,
    angleOfPoint,
    isBoundsAware,
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    svg,
    toDegrees,
} from '@eclipse-glsp/client';
import { VNode } from 'snabbdom';

export class ReferenceEdgeView extends PolylineEdgeViewWithGapsOnIntersections {
    protected override renderAdditionals(edge: GEdge, segments: Point[], context: RenderingContext): VNode[] {
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
    protected shouldRenderComposition(edge: any) {
        return edge.renderComposition === true;
    }

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    protected shouldRenderEndArrow(edge: any) {
        return edge.renderEndArrow === true;
    }

    protected renderArrow(p1: Point, p2: Point): VNode {
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const arrow = (
            <path
                class-sprotty-edge={true}
                class-arrow={true}
                d='M 1,0 L 10,-4 L 4,0 L 10,4 Z'
                transform={`rotate(${toDegrees(angleOfPoint({ x: p1.x - p2.x, y: p1.y - p2.y }))} ${p2.x} ${p2.y}) translate(${p2.x} ${
                    p2.y
                })`}
            />
        );
        return arrow;
    }

    protected renderComposition(p1: Point, p2: Point): VNode {
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const arrow = (
            <path
                class-sprotty-edge={true}
                class-arrow={true}
                d='M 1,0 L 10,-4 L 19,0 L 10,4 L 1,0 Z'
                transform={`rotate(${toDegrees(angleOfPoint({ x: p1.x - p2.x, y: p1.y - p2.y }))} ${p2.x} ${p2.y}) translate(${p2.x} ${
                    p2.y
                })`}
            />
        );
        return arrow;
    }
}

export class InheritanceEdgeView extends PolylineEdgeViewWithGapsOnIntersections {
    
    protected override renderAdditionals(edge: GEdge, segments: Point[], context: RenderingContext): VNode[] {
        const additionals = super.renderAdditionals(edge, segments, context);
        const p1 = segments[segments.length - 2];
        const p2 = segments[segments.length - 1];
        additionals.push(<path 
            class-sprotty-edge={true}
            d='M 1,0 L 10,-4 L 10,4 Z'
            transform={`rotate(${toDegrees(angleOfPoint({ x: p1.x - p2.x, y: p1.y - p2.y }))} ${p2.x} ${p2.y}) translate(${p2.x} ${
                p2.y
            })`}/>);
        return additionals;
    }
}

export class DividerView extends GCompartmentView {
    override render(compartment: Readonly<GCompartment>, context: RenderingContext, args?: IViewArgs | undefined): VNode | undefined {
        const parent = compartment.parent;
        const width = isBoundsAware(parent) ? parent.bounds.width : compartment.size.width;
        return <path d={`M 0,0 L ${width},0`} class-sprotty-node></path>;
    }
}
