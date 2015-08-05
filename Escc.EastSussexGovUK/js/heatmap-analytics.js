if (typeof (jQuery) != 'undefined' && typeof (esccConfig) != 'undefined') {
    // Use code from https://github.com/johnkpaul/jquery-ajax-retry to mitigate against network errors, which are the main
    // cause of no JavaScript according to gov.uk research https://gds.blog.gov.uk/2013/10/21/how-many-people-are-missing-out-on-javascript-enhancement/
    //
    // For IE8 and IE9 this also relies on https://github.com/jaubourg/ajaxHooks/blob/master/src/xdr.js to enable cross-domain requests.
    // 
    // Add vary={origin} to querystring as setting Vary header from view is getting overridden. Need to vary the response by origin somehow, otherwise it requests the alerts
    // from Origin A, caches the response with a CORS header allowing Origin A, then requests the alerts from Origin B, gets the cached version and fails the CORS test.
    $.ajax({ dataType: "json", url: esccConfig.HeatmapAnalyticsUrl + "?vary=" + document.location.protocol + "//" + document.location.host }).retry({ times: 3 }).then(function(data) {

        if (!data.HeatmapAnalyticsUrls) return;

        function isUrlMatch(urls, path, query) {

            // Normalise case of paths to compare, and trim end / even if there's a querystring after it
            var trimSlash = function (str) {
                var slash = str.lastIndexOf("/"), question = str.indexOf('?');
                str = (slash === str.length - 1) ? str.substr(0, slash) : str;
                str = (question > -1 && slash === question-1) ? str.substr(0, slash) + str.substr(question) : str;
                return str;
            }

            urls = urls.map(function (str) { return trimSlash(str.toUpperCase()); });
            path = trimSlash(path.toUpperCase());
            query = query.toUpperCase();

            // Cycle through enabled URLs looking for a match
            var len = urls.length;
            for (var i = 0; i < len; i++) {
             
                // If the configured URL has a querystring, make sure it matches but allow other parameters afterwards, eg a cache-busting parameter shouldn't break it.
                // If the configured URL has no querystring, don't check for one. eg for a search page, you want it enabled for any querystring.
                if (urls[i].indexOf('?') > -1) {
                    if ((path + query).indexOf(urls[i]) === 0) {
                        return true;
                    }
                } else {
                    if (path == urls[i]) {
                        return true;
                    }
                }
            }

            return false;
        }

        var enableAnalytics = isUrlMatch(data.HeatmapAnalyticsUrls, document.location.pathname, document.location.search);
        if (enableAnalytics) {
            /* Crazy Egg code snippet. */
            setTimeout(function () {
                var a = document.createElement("script");
                var b = document.getElementsByTagName("script")[0];
                a.src = document.location.protocol + "//script.crazyegg.com/pages/scripts/0035/1997.js?" + Math.floor(new Date().getTime() / 3600000);
                a.async = true; a.type = "text/javascript";
                b.parentNode.insertBefore(a, b);
            }, 1);
        };
    });
}