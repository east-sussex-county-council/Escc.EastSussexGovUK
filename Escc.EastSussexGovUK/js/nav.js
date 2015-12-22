if (typeof (jQuery) != 'undefined') {
    $(function() {
        // Show/hide the navigation within the guide. CSS ensures this only takes effect at mobile sizes.
        var menu = $(".guide-nav").hide();
        var toggle = $('<div class="menu-toggle menu-toggle-show small">Show all pages in this section</div>');

        toggle.insertBefore(menu).click(function () {
            if (menu.is(":visible")) {
                menu.slideUp();
                toggle.removeClass("menu-toggle-hide").addClass("menu-toggle-show").text("Show all pages in this section");
            } else {
                menu.slideDown();
                toggle.removeClass("menu-toggle-show").addClass("menu-toggle-hide").text("Hide all pages in this section");
            }
        });

    });
}