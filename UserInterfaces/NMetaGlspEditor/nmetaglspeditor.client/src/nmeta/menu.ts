import { Anchor, IContextMenuService, SetPopupModelAction, TYPES, IActionDispatcher, MenuItem, ViewerOptions, EMPTY_ROOT } from "@eclipse-glsp/client";
import { inject, injectable } from "inversify";

@injectable()
export class ContextMenuService implements IContextMenuService {

    @inject(TYPES.IActionDispatcher) readonly actionDispatcher: IActionDispatcher;
    @inject(TYPES.ViewerOptions) protected viewerOptions: ViewerOptions;

    show(items: MenuItem[], anchor: Anchor, onHide?: (() => void) | undefined): void {
        this.actionDispatcher.dispatch(SetPopupModelAction.create(EMPTY_ROOT));
        const container = document.getElementById(this.viewerOptions.baseDiv);
        // eslint-disable-next-line prefer-const
        let menuNode: HTMLDivElement;
        const hideMenu = () => {
            container?.removeChild(menuNode);
            if (onHide) {
                onHide();
            }
        };
        menuNode = this.createMenu(items, hideMenu);
        menuNode.style.top = (anchor.y - 5) + 'px';
        menuNode.style.left = (anchor.x - 5) + 'px';


        container?.appendChild(menuNode);
        menuNode.onmouseleave = (e: MouseEvent) => hideMenu();
    }

    protected createMenu(items: MenuItem[], closeCallback: () => void): HTMLDivElement {
        const menuNode = document.createElement('div');
        menuNode.id = 'nmeta-context-menu';
        menuNode.classList.add('nmeta-context-menu');
        this.createMenuItems(items, menuNode, closeCallback);
        return menuNode;
    }

    protected createMenuItems(items: MenuItem[], menuNode: HTMLDivElement, closeCallback: () => void) {
        items.forEach((item, index) => {
            const menuItem = document.createElement('div');
            menuItem.id = 'nmeta-context-menu-item-' + index;
            menuItem.classList.add('nmeta-context-menu-item');
            const itemEnabled = item.isEnabled ? item.isEnabled() : true;
            if (!itemEnabled)
                menuItem.classList.add('disabled-action');
            menuItem.textContent = item.label;
            menuItem.onclick = (e: MouseEvent) => {
                closeCallback();
                if (itemEnabled && item.actions.length > 0) {
                    this.actionDispatcher.dispatchAll(item.actions);
                }
            };
            menuNode.appendChild(menuItem);

            if (item.children) {
                this.createMenuItems(item.children, menuItem, closeCallback);
            }
        });
    }
}