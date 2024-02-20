import {
    GEdge,
    Point,
    PolylineEdgeViewWithGapsOnIntersections,
    RenderingContext,
    angleOfPoint,
    svg,
    toDegrees,
} from '@eclipse-glsp/client';
import { VNode } from 'snabbdom';

// eslint-disable-next-line @typescript-eslint/no-unused-vars, react-refresh/only-export-components
const JSX = { createElement: svg };

export class ReferenceEdgeView extends PolylineEdgeViewWithGapsOnIntersections {
    protected override renderAdditionals(edge: GEdge, segments: Point[], context: RenderingContext): VNode[] {
        const additionals = super.renderAdditionals(edge, segments, context);
        /*if (this.shouldRenderEndArrow(edge)) {
            const p1 = segments[segments.length - 2];
            const p2 = segments[segments.length - 1];
            additionals.push(this.renderArrow(p1, p2));
        }
        if (this.shouldRenderComposition(edge)) {
            const p1 = segments[1];
            const p2 = segments[0];
            additionals.push(this.renderComposition(p1, p2));
        }*/
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
        const arrow:any = (
            <path
                class-sprotty-edge={true}
                class-arrow={true}
                d='M 1,0 L 10,-4 L 10,4 Z'
                transform={`rotate(${toDegrees(angleOfPoint({ x: p1.x - p2.x, y: p1.y - p2.y }))} ${p2.x} ${p2.y}) translate(${p2.x} ${
                    p2.y
                })`}
            />
        );
        return arrow;
    }

    protected renderComposition(p1: Point, p2: Point): VNode {
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const arrow:any = (
            <path
                class-sprotty-edge={true}
                class-arrow={true}
                d='M 1,0 L 10,-4 M 10,-4 Z'
                transform={`rotate(${toDegrees(angleOfPoint({ x: p1.x - p2.x, y: p1.y - p2.y }))} ${p2.x} ${p2.y}) translate(${p2.x} ${
                    p2.y
                })`}
            />
        );
        return arrow;
    }
}