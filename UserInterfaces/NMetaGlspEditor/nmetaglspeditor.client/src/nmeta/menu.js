"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.ContextMenuService = void 0;
const client_1 = require("@eclipse-glsp/client");
const inversify_1 = require("inversify");
let ContextMenuService = class ContextMenuService {
    show(items, anchor, onHide) {
        this.actionDispatcher.dispatch(client_1.SetPopupModelAction.create(client_1.EMPTY_ROOT));
        const container = document.getElementById(this.viewerOptions.baseDiv);
        // eslint-disable-next-line prefer-const
        let menuNode;
        const hideMenu = () => {
            container === null || container === void 0 ? void 0 : container.removeChild(menuNode);
            if (onHide) {
                onHide();
            }
        };
        menuNode = this.createMenu(items, hideMenu);
        menuNode.style.top = (anchor.y - 5) + 'px';
        menuNode.style.left = (anchor.x - 5) + 'px';
        container === null || container === void 0 ? void 0 : container.appendChild(menuNode);
        menuNode.onmouseleave = (e) => hideMenu();
    }
    createMenu(items, closeCallback) {
        const menuNode = document.createElement('div');
        menuNode.id = 'nmeta-context-menu';
        menuNode.classList.add('nmeta-context-menu');
        this.createMenuItems(items, menuNode, closeCallback);
        return menuNode;
    }
    createMenuItems(items, menuNode, closeCallback) {
        items.forEach((item, index) => {
            const menuItem = document.createElement('div');
            menuItem.id = 'nmeta-context-menu-item-' + index;
            menuItem.classList.add('nmeta-context-menu-item');
            const itemEnabled = item.isEnabled ? item.isEnabled() : true;
            if (!itemEnabled)
                menuItem.classList.add('disabled-action');
            menuItem.textContent = item.label;
            menuItem.onclick = (e) => {
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
};
exports.ContextMenuService = ContextMenuService;
__decorate([
    (0, inversify_1.inject)(client_1.TYPES.IActionDispatcher),
    __metadata("design:type", Object)
], ContextMenuService.prototype, "actionDispatcher", void 0);
__decorate([
    (0, inversify_1.inject)(client_1.TYPES.ViewerOptions),
    __metadata("design:type", Object)
], ContextMenuService.prototype, "viewerOptions", void 0);
exports.ContextMenuService = ContextMenuService = __decorate([
    (0, inversify_1.injectable)()
], ContextMenuService);
//# sourceMappingURL=menu.js.map