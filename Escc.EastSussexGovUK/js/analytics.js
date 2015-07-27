// Faster Google Analytics code: mathiasbynens.be/notes/async-analytics-snippet
// but restrict it only to external URLs
if (location.host.indexOf('.') > -1 && location.host.indexOf('escc.gov') == -1 && location.host.indexOf('spydus.co.uk') == -1 && location.host.indexOf('azurewebsites.net') == -1 && typeof esccConfig !== 'undefined') {
    (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
        (i[r].q=i[r].q||[]).push(arguments);},i[r].l=1*new Date();a=s.createElement(o),
        m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m);
    })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

    ga('create', esccConfig.GoogleAnalyticsPublicWebsite, 'auto');
    ga('send', 'pageview');

    if (typeof (jQuery) != 'undefined') {
        $(function () { if (typeof (Escc) != 'undefined' && typeof (Escc.Statistics) != 'undefined') Escc.Statistics.TrackWithGoogleAnalytics({ domains: ['eastsussex.gov.uk', 'eastsussexcc.gov.uk'] }); });

        // Track clicks in common areas of template so see how well they're used
        $(".header a").click(function() { ga('send', 'event', 'header', document.URL, $(this).attr("href"), 0); });
        $(".footer a").click(function () { ga('send', 'event', 'footer', document.URL, $(this).attr("href"), 0); });
    }
}
