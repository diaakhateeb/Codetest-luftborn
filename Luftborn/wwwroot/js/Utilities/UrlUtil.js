export default class UrlUtil {
    constructor() {
        this.queryValue = (field, url) => {
            var href = url ? url : window.location.href;
            var reg = new RegExp('[?&]' + field + '=([^&#]*)', 'i');
            var string = reg.exec(href);
            return string ? string[1].toLowerCase() : null;
        };
        this.currentURL = () => window.location.href;
        this.getURLParameters = (url) => (url.match(/([^?=&]+)(=([^&]*))/g) || []).reduce((a, v) => ((a[v.slice(0, v.indexOf("="))] = v.slice(v.indexOf("=") + 1)), a), {});
        this.redirect = (url, asLink = true) => asLink ? (window.location.href = url) : window.location.replace(url);
        this.baseUrl = () => window.location.origin;
        this.host = () => window.location.host;
        this.pathArray = () => window.location.pathname.split('/');
    }
}
//# sourceMappingURL=UrlUtil.js.map