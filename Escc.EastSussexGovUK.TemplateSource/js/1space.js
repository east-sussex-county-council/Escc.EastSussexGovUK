if (typeof(jQuery) !== 'undefined') {
    $(function () {
        // polyfill placeholder text
        var placeholder = 'What are you looking for?';
        var input = $("#marketplace-search-widget input");
        input.focus(function () {
            if (this.value === placeholder) {
                this.value = '';
            }
        }).blur(function () {
            if (this.value === '') { this.value = placeholder; }
        });

        //
        $("#marketplace-search-widget form").submit(function() {
            var searchTerm = input.val();
            if (!searchTerm || searchTerm === placeholder) {
                input.val('');
                this.action = "https://www.eastsussex1space.co.uk";
            }
        });

        // track views and searches
        if (typeof(ga) !== 'undefined' && location.host.indexOf('.') > -1 && location.host.indexOf('escc.gov') === -1 && location.host.indexOf('spydus.co.uk') === -1 && location.host.indexOf('azurewebsites.net') === -1) {
            ga('send', 'event', '1space-widget', 'viewed', '', 0);

            $("#marketplace-search-widget button").click(function () {
                ga('send', 'event', '1space-widget', 'search', input.val(), 0);
            });
        }

    });
}