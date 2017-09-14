if (typeof (jQuery) != 'undefined' && typeof (esccConfig) != 'undefined') {
    $(function () {
        // Enable on https only as requesting the user's email address
        if (document.location.protocol !== "https:") return;

        // Enable on internal hostnames, *.azurewebsites.net, *.spydus.co.uk or specific subdomains of .eastsussex.gov.uk
        if (!(/^([a-z0-9]+|[a-z0-9-]+\.azurewebsites\.net|[a-z0-9-]+\.spydus\.co\.uk|(www|apps|payments|e-library)\.eastsussex\.gov\.uk)$/.test(document.location.hostname))) return;

        // Add a 'keep me posted' link to the header, which loads HTML and styles when clicked
        $("<a href=\"#\" class=\"govdelivery screen\">Keep me posted</a>").appendTo(".header .container, .header-v2 .container").click(function(e) {
            e.preventDefault();

            if (!document.getElementById('govdelivery')) {
                // Load extra CSS. Check first before loading forms-small.css because it may already be loaded. As well as saving bytes, if we load it a second time
                // after the forms-medium.css and forms-large.css it changes the cascade and causes layout problems.
                var formsCssLoaded = ($("link[rel=stylesheet]").filter(function() { return this.href.indexOf("formssmall") > -1 }).length > 0);
                $('<link>').appendTo('head').attr({ type: 'text/css', rel: 'stylesheet', 'href': esccConfig.CssHandlerPath.replace('{0}', formsCssLoaded ? 'emailpanel' : 'emailpanel-formssmall') });

                // Load extra JS for GDPR data protection message. Check first to see if it's already loaded.
                if (typeof (jQuery.fn.bt) === 'undefined') {
                    $('<script>').appendTo('body').attr({ async: true, 'src': esccConfig.JsHandlerPath.replace('{0}', 'describedbytips-tips') });
                }

                // Use code from https://github.com/johnkpaul/jquery-ajax-retry to mitigate against network errors, which are the main
                // cause of no JavaScript according to gov.uk research https://gds.blog.gov.uk/2013/10/21/how-many-people-are-missing-out-on-javascript-enhancement/
                //
                // For IE8 and IE9 this also relies on https://github.com/jaubourg/ajaxHooks/blob/master/src/xdr.js to enable cross-domain requests.
                // 
                // Add vary={origin} to querystring as setting Vary header from .NET gets overridden. Need to vary the response by origin somehow for resources protected by CORS, 
                // otherwise a client requests the URL from Origin A, caches the response with a CORS header allowing Origin A, then requests the alerts from Origin B, gets the 
                // cached version and fails the CORS test.
                $.ajax({ dataType: "html", url: esccConfig.MasterPagesBaseUrl + "/controls/govdelivery.html?vary=" + document.location.protocol + "//" + document.location.host }).retry({ times: 3 }).then(function (data) {
                    $(data).hide().insertBefore(".header, .header-v2").slideDown();
                    if (typeof (jQuery.fn.describedByTip) !== 'undefined') {
                        $("#govdelivery-email").describedByTip();
                    }
                });

            } else {
                $("#govdelivery").slideToggle();
            }

        });

    });
}