if (typeof (jQuery) != "undefined") {
    $("a.switch").attr("href", function () { return this.href + "&return=" + encodeURIComponent(document.URL); });
};