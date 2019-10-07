export default class MakeRequest {
    constructor(url, method = "get") {
        this.method = method;
        this.url = url;
    }
    send(body = null) {
        return new Promise((resolve, reject) => {
            const xmlHttpRequest = new XMLHttpRequest();
            xmlHttpRequest.open(this.method, this.url);
            if (body != null) {
                xmlHttpRequest.setRequestHeader("Accept", "*/*");
                xmlHttpRequest.setRequestHeader("Content-Type", "application/json");
            }
            xmlHttpRequest.onload = () => {
                if (xmlHttpRequest.status >= 200 && xmlHttpRequest.status < 300) {
                    resolve(xmlHttpRequest.response);
                }
                else {
                    reject({
                        status: xmlHttpRequest.status,
                        statusText: xmlHttpRequest.statusText
                    });
                }
            };
            xmlHttpRequest.onerror = () => {
                reject({
                    status: xmlHttpRequest.status,
                    statusText: xmlHttpRequest.statusText
                });
            };
            xmlHttpRequest.send(body);
        });
    }
}
//# sourceMappingURL=MakeRequest.js.map