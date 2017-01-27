// esccConfig is from config.js 
// cascadingContentFilter is from cascading-content.js

if (typeof (jQuery) != 'undefined' && typeof (esccConfig) != 'undefined' && typeof (cascadingContentFilter) !== 'undefined') {

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

            // Adapt data format to that expected by cascading-content.js, which is a refactored version of the code originally written here.
            // Note: array.map not used as it's only supported from IE9 and up
            function adaptDataFormat(alert) {
                if (!alert.TargetUrls) alert.TargetUrls = alert.urls;
                if (!alert.Inherit) alert.Inherit = alert.append;
                if (!alert.Cascade) alert.Cascade = alert.cascade;
                return alert;
            }

            var alerts = data;
            for (var i = 0; i < data.length; i++) {
                alerts[i] = adaptDataFormat(alerts[i]);
            }

            // Note: Must filter inheritance before cascade. If an ancestor of the current page blocks inheritance it needs to be
            // considered when deciding how far up the tree to inherit, before it potentially gets removed if it blocks cascade as well.
            // 
            // eg
            // --- [has cascading alert]
            //     --- [has alert with no cascade, no inherit]
            //         --- [allows inherit, but should not inherit top-level alert]
            // 
            var cascade = cascadingContentFilter();
            alerts = cascade.filterByUrl(alerts, window.location.pathname);
            alerts = cascade.filterByInherit(alerts, window.location.pathname);
            alerts = cascade.filterByCascade(alerts, window.location.pathname);
            alerts = cascade.filterIfBlank(alerts, function (item) { return !$.trim(item.alert); });

            if (alerts.length) {
                displayAlerts(alerts);
            }
        });
    }

    function displayAlerts(alertData) {
        /// <summary>Display an alert on the page</summary>
        $("head").append('<link rel="stylesheet" href="' + esccConfig.CssHandlerPath.replace('{0}', 'alert') + '" />');

        var container = $("#main");
        var fullScreenContainer = $(".topbar");

        var alertHtml = '';
        $.each(alertData, function (key, val) {
            alertHtml += val.alert;
        });

        var alertNode = $('<div class="alert" role="alert"><div class="container"><strong><span class="icon"></span><article>' + alertHtml + '</article></strong></div></div>');

        if (fullScreenContainer.length) {
            alertNode.insertAfter(fullScreenContainer);
        } else if (container.length) {
            alertNode.insertBefore(container);
        }
    }
}