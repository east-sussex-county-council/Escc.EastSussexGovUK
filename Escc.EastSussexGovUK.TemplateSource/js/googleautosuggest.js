if (typeof (jQuery) !== 'undefined' && typeof (esccConfig) !== 'undefined' && esccConfig.SearchAutoCompleteUrl) {
    $(function () {
        "use strict";

        // Wire up autocomplete on search box
        $(".search").autocomplete({
            source: esccConfig.SearchAutoCompleteUrl,
            delay: 100,
            minLength: 2,
            select: function(event, ui) {
                $('#q').val(ui.item.value)[0].form.submit()
            }
        });
    });
}