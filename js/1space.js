if (typeof(jQuery) !== 'undefined') {
    $(function () {
        // polyfill placeholder text
        $("#marketplace-search-widget input").focus(function () {
            if (this.value === 'What are you looking for?') {
                this.value = '';
            }
        }).blur(function () {
            if (this.value === '') { this.value = 'What are you looking for?'; }
        });

        // track views and searches
        if (typeof(ga) !== 'undefined' && location.host.indexOf('.') > -1 && location.host.indexOf('escc.gov') === -1 && location.host.indexOf('spydus.co.uk') === -1 && location.host.indexOf('azurewebsites.net') === -1) {
            ga('send', 'event', '1space-widget', 'viewed', '', 0);

            $("#marketplace-search-widget button").click(function () {
                ga('send', 'event', '1space-widget', 'search', $("#marketplace-search-widget input").val(), 0);
            });
        }

    });
}