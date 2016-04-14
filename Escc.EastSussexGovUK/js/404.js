if (typeof (jQuery) !== "undefined") {
    $(function() {
        // Track in Google Analytics which pages returned a 404
        var track = $("#track-404");
        var requestUrl = track.data("request");
        var referrerUrl = track.data("referrer");
        
        if (typeof (ga) !== "undefined") {
            ga('send', 'event', '404',requestUrl, referrerUrl);
        }
    });
}