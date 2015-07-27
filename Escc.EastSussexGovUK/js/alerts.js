if (typeof (jQuery) != 'undefined' && typeof (esccConfig) != 'undefined') {

    // If the old alert is still active, don't add another.
    if (!$(".alert").length) {

        // Use code from https://github.com/johnkpaul/jquery-ajax-retry to mitigate against network errors, which are the main
        // cause of no JavaScript according to gov.uk research https://gds.blog.gov.uk/2013/10/21/how-many-people-are-missing-out-on-javascript-enhancement/
        //
        // For IE8 and IE9 this also relies on https://github.com/jaubourg/ajaxHooks/blob/master/src/xdr.js to enable cross-domain requests.
        // 
        // Add vary={origin} to querystring as setting Vary header from view is getting overridden. Need to vary the response by origin somehow, otherwise it requests the alerts
        // from Origin A, caches the response with a CORS header allowing Origin A, then requests the alerts from Origin B, gets the cached version and fails the CORS test.
        $.ajax({ dataType: "json", url: esccConfig.AlertsUrl + "?vary=" + document.location.protocol + "//" + document.location.host }).retry({ times: 3 }).then(function (data) {

            // Note: Must filter inheritance before cascade. If an ancestor of the current page blocks inheritance it needs to be
            // considered when deciding how far up the tree to inherit, before it potentially gets removed if it blocks cascade as well.
            // 
            // eg
            // --- [has cascading alert]
            //     --- [has alert with no cascade, no inherit]
            //         --- [allows inherit, but should not inherit top-level alert]
            // 
            var alerts = data;
            alerts = filterByUrl(alerts);
            alerts = filterByInherit(alerts);
            alerts = filterByCascade(alerts);
            alerts = filterBlankAlerts(alerts);

            if (alerts.length) {
                displayAlerts(alerts);
            }
        });
    }

    function displayAlerts(alertData) {
        /// <summary>Display an alert on the page</summary>
        $("head").append('<link rel="stylesheet" href="' + esccConfig.CssHandlerPath.replace('{0}', 'alert') + '" />');

        var container = $("#main > .container");
        var fullScreenContainer = $(".topbar");
        var breadcrumb = $(".breadcrumb, .breadcrumb-mobile", container);

        var alertHtml = '';
        $.each(alertData, function (key, val) {
            alertHtml += val.alert;
        });

        var alertNode = $('<strong class="alert" role="alert"><span class="icon"></span><article>' + alertHtml + '</article></strong>');

        if (breadcrumb.length) {
            alertNode.insertAfter(breadcrumb[breadcrumb.length - 1]);
        } else if (container.length) {
            alertNode.prependTo(container);
        } else if (fullScreenContainer.length) {
            alertNode.insertAfter(fullScreenContainer);
        }
    }

    function filterBlankAlerts(alertsData) {
        /// <summary>Filters alerts based on whether they have any text</summary>
        var alerts = [];

        $.each(alertsData, function (key, singleAlert) {
            if ($.trim(singleAlert.alert)) {
                alerts.push(singleAlert);
            }
        });

        return alerts;
    }

    function filterByInherit(alertsData) {
        /// <summary>Filters alerts based on their inheritance settings</summary>

        // If inheritance is blocked, that's a new base URL. Anything matching that URL or longer is valid.
        var minimumUrl = '';

        // Start by getting the deepest alert URL which blocks inheritance.
        $.each(alertsData, function (key, singleAlert) {
            if (!singleAlert.append) {
                $.each(singleAlert.urls, function(key2, url) {
                    if (doesUrlMatchCurrentPage(url)) {
                        var minimumUrlCandidate = stripTrailingSlash(url);
                        if (minimumUrlCandidate.length > minimumUrl.length) minimumUrl = minimumUrlCandidate;
                    }
                });
            }
        });

        // If nothing blocks inheritance, return unchanged data
        if (!minimumUrl.length) return alertsData;

        // Otherwise go through all the alerts, and select only those matching the new base URL or longer.
        var alerts = [];
        $.each(alertsData, function (key, singleAlert) {
            $.each(singleAlert.urls, function(key2, url) {
                var alertUrl = stripTrailingSlash(url);
                if (alertUrl.indexOf(minimumUrl) === 0) {
                    alerts.push(singleAlert);
                    return false;
                }
                return true;
            });
        });

        return alerts;
    }

    function filterByCascade(alertsData) {
        /// <summary>Filters alerts based on their cascade settings</summary>
        var alerts = [];

        $.each(alertsData, function(key, singleAlert) {
            if (isExactUrlMatch(singleAlert) || singleAlert.cascade) {
                alerts.push(singleAlert);
            }
        });

        return alerts;
    }

    function filterByUrl(alertsData) {
        /// <summary>Filters alerts based on whether they start from the current URL or an ancestor</summary>
        var alerts = [];

        $.each(alertsData, function(key, singleAlert) {
            if (alertHasUrlMatch(singleAlert)) {
                alerts.push(singleAlert);
            }
        });

        return alerts;
    }

    function alertHasUrlMatch(singleAlert) {
        /// <summary>Checks whether an alert is displayed starting from the current URL or an ancestor</summary>
        var match = false;
        $.each(singleAlert.urls, function (key, url) {
            match = doesUrlMatchCurrentPage(url);
            return !match;
        });
        return match;
    }

    function doesUrlMatchCurrentPage(url) {
        /// <summary>Checks whether a URL matches the current URL or an child page</summary>
        var alertUrl = stripTrailingSlash(url);
        if (window.location.pathname.indexOf(alertUrl) === 0) {
            return true;
        }
        return false;
    }

    function isExactUrlMatch(singleAlert) {
        /// <summary>Checks whether an alert is displayed starting from the current URL</summary>
        var pageUrl = stripTrailingSlash(window.location.pathname);
        var match = false;
        $.each(singleAlert.urls, function (key, url) {
            var alertUrl = stripTrailingSlash(url);
            if (pageUrl === alertUrl) {
                match = true;
                return false;
            }
            return true;
        });
        return match;
    }

    function stripTrailingSlash(str) {
        /// <summary>Trim a trailing / from a string</summary>
        if (str.substr(str.length - 1) == '/') {
            return str.substr(0, str.length - 1);
        }
        return str;
    }
}