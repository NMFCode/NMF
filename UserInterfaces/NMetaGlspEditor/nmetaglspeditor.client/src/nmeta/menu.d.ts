import { Anchor, IContextMenuService, IActionDispatcher, MenuItem, ViewerOptions } from "@eclipse-glsp/client";
export declare class ContextMenuService implements IContextMenuService {
    readonly actionDispatcher: IActionDispatcher;
    protected viewerOptions: ViewerOptions;
    show(items: MenuItem[], anchor: Anchor, onHide?: (() => void) | undefined): void;
    protected createMenu(items: MenuItem[], closeCallback: () => void): HTMLDivElement;
    protected createMenuItems(items: MenuItem[], menuNode: HTMLDivElement, closeCallback: () => void): void;
}
//# sourceMappingURL=menu.d.ts.map