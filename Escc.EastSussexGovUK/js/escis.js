if (typeof (jQuery) !== 'undefined') {
    $(function() {
        /* If <div class="escis" /> is in the page, load an iframe inside it. But only load the iframe if we're above the large breakpoint (or if that test is not supported). */
        var escisLoaded = false;
        function loadEscis() {
            if (escisLoaded) return;

            if (!window.matchMedia || window.matchMedia("(min-width: 802px)").matches) {
                $(".escis").append('<iframe src="https://www.escis.org.uk/escis-widget/escis-widget.html" frameborder="0" width="100%" height="459px" />');
                escisLoaded = true;
            }
        }

        $(window).resize(loadEscis);
        loadEscis();
    });
}