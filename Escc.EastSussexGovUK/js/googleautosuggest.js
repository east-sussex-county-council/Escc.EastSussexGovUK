if (typeof(jQuery) != 'undefined') {
    $(function() {
        // The data source is on the same site as the search page, so request from there. 
        // CORS support is set up for specific domains on the public website.
        var sourceUrl = "/masterpages/AutoSuggestGoogleAnalytics.ashx";

        var searchAction = document.getElementById("search").getAttribute("action");
        var searchHostStart = searchAction.indexOf("://");
        var searchHostEnd = (searchHostStart == -1) ? -1 : searchAction.indexOf("/", searchHostStart + 3);
        if (searchHostEnd != -1) {
            sourceUrl = document.location.protocol + "//" + searchAction.substring(searchHostStart + 3, searchHostEnd) + sourceUrl;
        }

        // Wire up autocomplete
        $(".search").autocomplete({
            source: sourceUrl,
            delay: 100,
            minLength: 2,
            select: function(event, ui) {
                $('#q').val(ui.item.value)[0].form.submit()
            }
        });
    });
}