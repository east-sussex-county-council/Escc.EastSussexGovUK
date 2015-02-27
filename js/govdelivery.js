$(function() {
    // Add support for subdomains, CORS and retries later
    if (document.location.hostname.indexOf('.') != -1 && document.location.hostname != 'www.eastsussex.gov.uk') return;

    // Add a 'keep me posted' link to the header, which loads HTML and styles when clicked
    $("<a href=\"#\" class=\"govdelivery\">Keep me posted</a>").appendTo(".header .container").click(function (e) {
        e.preventDefault();

        if (!document.getElementById('govdelivery')) {
            $('<link>').appendTo('head').attr({ type: 'text/css', rel: 'stylesheet', 'href': '/css/emailpanel-formssmall.cssx' });
            $.get("/masterpages/controls/govdelivery.html", function (data) {
                $(data).hide().insertBefore(".header").slideDown();
            });
        } else {
            $("#govdelivery").slideToggle();
        }

       
    });

});