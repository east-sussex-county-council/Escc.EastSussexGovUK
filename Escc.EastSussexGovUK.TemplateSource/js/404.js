if (typeof (jQuery) !== "undefined") {
    $(function () {
        // Track in Google Analytics which pages returned a 404
        var requestUrl = $("script[data-request]").data("request");
        var referrerUrl = $("script[data-referrer]").data("referrer");

        if (typeof (ga) !== "undefined") {
            ga('send', 'event', '404', requestUrl, referrerUrl);
        }
    });
}