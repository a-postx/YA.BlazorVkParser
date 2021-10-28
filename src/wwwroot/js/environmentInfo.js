window.environmentInfo = {
    GetNavigatorInfo() {
        var navigatorInfo = {
            appCodeName: navigator.appCodeName,
            appName: navigator.appName,
            appVersion: navigator.appVersion,
            vendor: navigator.vendor,
            vendorSub: navigator.vendorSub,
            product: navigator.product,
            productSub: navigator.productSub,
            platform: navigator.platform,
            osCpu: navigator.oscpu,
            cookieEnabled: navigator.cookieEnabled,
            language: navigator.language,
            userAgent: navigator.userAgent
        };

        return navigatorInfo;
    },
    GetScreenInfo() {
        var screenInfo = {
            height: window.screen.height,
            width: window.screen.width,
            viewportHeight: Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0),
            viewportWidth: Math.max(document.documentElement.clientWidth || 0, window.innerWidth || 0)
        };

        return screenInfo;
    },
    GetUaInfo() {
        var parser = new UAParser();
        var parsingResult = parser.getResult();

        return parsingResult;
    }
}