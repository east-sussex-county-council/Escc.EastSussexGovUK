$(function() {
    $("<a href=\"#\" class=\"govdelivery\">Keep me posted</a>").appendTo(".header .container").click(function(e) {
        e.preventDefault();

        if (!document.getElementById('govdelivery')) {
            $('<link>').appendTo('head').attr({ type: 'text/css', rel: 'stylesheet', 'href': '/css/govdelivery.css?refresh=' + Date.now() });
            $.get("/masterpages/controls/govdelivery.html", function(data) {
                $(data).hide().insertBefore(".header").slideDown();
            });
        } else {
            $("#govdelivery").slideToggle();
        }

       
    });

});