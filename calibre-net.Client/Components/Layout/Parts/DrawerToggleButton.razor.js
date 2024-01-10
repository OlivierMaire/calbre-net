export function HandleDrawerToggled(isOpen, drawerName) {
    const layoutElement = document.querySelector(".mud-layout");
    const mudDrawerElement = document.querySelector("."+drawerName);
    if (isOpen) {
        layoutElement.classList.replace("mud-drawer-close-responsive-md-left", "mud-drawer-open-responsive-md-left");
        mudDrawerElement.classList.replace("mud-drawer--closed", "mud-drawer--open");
    } else {
        mudDrawerElement.classList.replace("mud-drawer--open", "mud-drawer--closed");
        layoutElement.classList.replace("mud-drawer-open-responsive-md-left", "mud-drawer-close-responsive-md-left");
    }
}