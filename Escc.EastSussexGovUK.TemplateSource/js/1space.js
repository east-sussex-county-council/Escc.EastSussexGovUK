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

        // if the user clicked the search button without entering a term, go to the East Sussex 1Space home page
        $("#marketplace-search-widget form").submit(function() {
            var searchTerm = input.val();
            if (!searchTerm || searchTerm === placeholder) {
                input.val('');
                this.action = "https://www.eastsussex1space.co.uk";
            }
        });

        // track views and searches
        if (typeof(ga) !== 'undefined') {
            ga('send', 'event', '1space-widget', 'viewed', '', 0);

            $("#marketplace-search-widget button").click(function () {
                ga('send', 'event', '1space-widget', 'search', input.val(), 0);
            });
        }

    });
}