if (typeof(jQuery) != 'undefined') {
    $(function () {
        // polyfill placeholder text
        $("#marketplace-search-widget input").focus(function () {
            if (this.value == 'What are you looking for?') {
                this.value = '';
            }
        }).blur(function () {
            if (this.value == '') { this.value = 'What are you looking for?'; }
        });

        // track views and searches
        if (typeof (_gaq) != 'undefined' && location.host.indexOf('.') > -1 && location.host.indexOf('escc.gov') == -1) {
            _gaq.push(['_trackEvent', '1space-widget', 'viewed', '', 0]);

            $("#marketplace-search-widget button").click(function () {
                _gaq.push(['_trackEvent', '1space-widget', 'search', $("#marketplace-search-widget input").val(), 0]);
            });
        }

    });
}