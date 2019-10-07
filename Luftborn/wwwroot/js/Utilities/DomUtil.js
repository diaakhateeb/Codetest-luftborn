export default class DomUtil {
    constructor(element) {
        this.element = element;
    }
    getDataAttr(attr, defaultvalue = undefined) {
        if (!this.element)
            return null;
        const value = this.element.getAttribute(`data-${attr}`);
        return value !== undefined ? value : defaultvalue;
    }
    setDataAttr(attr, value) {
        if (!this.element)
            return null;
        return this.element.setAttribute(`data-${attr}`, value);
    }
    getAttr(attr, defaultvalue = undefined) {
        if (!this.element)
            return null;
        const value = this.element.getAttribute(attr);
        return value !== undefined ? value : defaultvalue;
    }
    setAttr(attr, value) {
        if (!this.element)
            return null;
        this.element.setAttribute(attr, value);
    }
    removeAttr(attr) {
        if (!this.element)
            return null;
        this.element.removeAttribute(attr);
    }
    hasAttr(attr) {
        if (!this.element)
            return null;
        return this.element.hasAttribute(attr);
    }
    shake() {
        this.appendClass("shake");
        setTimeout(() => {
            this.removeClass("shake");
            this.element.readOnly = false;
        }, 300);
    }
    removeClass(classname) {
        this.element.classList.remove(classname);
    }
    appendClass(classname) {
        this.element.classList.add(classname);
    }
}
//# sourceMappingURL=DomUtil.js.map