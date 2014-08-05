// Faster Google Analytics code: mathiasbynens.be/notes/async-analytics-snippet
// but restrict it only to external URLs
if (location.host.indexOf('.') > -1 && location.host.indexOf('escc.gov') == -1 && location.host.indexOf('spydus.co.uk') == -1 && location.host.indexOf('azurewebsites.net') == -1 && typeof esccConfig !== 'undefined') {
    var _gaq = [['_setAccount', esccConfig.GoogleAnalyticsPublicWebsite], ['_trackPageview'], ['_setDomainName', '.eastsussex.gov.uk'], ['_trackPageLoadTime']];
    (function (d, t) {
        var g = d.createElement(t), s = d.getElementsByTagName(t)[0]; g.async = 1;
        g.src = ('https:' == location.protocol ? '//ssl' : '//www') + '.google-analytics.com/ga.js';
        s.parentNode.insertBefore(g, s)
    }(document, 'script'));

    if (typeof (jQuery) != 'undefined') {
        $(function () { if (typeof (Escc) != 'undefined' && typeof (Escc.Statistics) != 'undefined') Escc.Statistics.TrackWithGoogleAnalytics({ domains: ['eastsussex.gov.uk', 'eastsussexcc.gov.uk'] }); });

        // Track clicks in common areas of template so see how well they're used
        $(".header a").click(function () { _gaq.push(['_trackEvent', 'header', document.URL, $(this).attr("href"), 0]) })
        $(".rap a").click(function () { _gaq.push(['_trackEvent', 'report, apply, pay', document.URL, $(this).attr("href"), 0]) })
        $(".footer a").click(function () { _gaq.push(['_trackEvent', 'footer', document.URL, $(this).attr("href"), 0]) })
        $(".footer form").submit(function () { _gaq.push(['_trackEvent', 'footer', 'newsletter sign up', '', 0]) })
    }
}
