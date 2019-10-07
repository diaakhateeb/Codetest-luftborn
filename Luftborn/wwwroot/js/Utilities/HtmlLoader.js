export default class HtmlLoader {
    constructor(queryString) {
        this.targetElement = document.querySelector(queryString);
    }
    load(url, method = "get") {
        if (!this.targetElement) {
            throw new Error("missing target element");
        }
        const xmlHttpRequest = new XMLHttpRequest();
        xmlHttpRequest.onreadystatechange = () => {
            if (xmlHttpRequest.readyState == 4 && xmlHttpRequest.status == 200) {
                this.targetElement.innerHTML = xmlHttpRequest.responseText;
            }
        };
        xmlHttpRequest.open(method, url, true);
        xmlHttpRequest.send();
    }
}
//# sourceMappingURL=HtmlLoader.js.map