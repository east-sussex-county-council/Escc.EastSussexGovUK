if (typeof (jQuery) != "undefined") {
    $("div.languages a").attr("href", function () { return this.href + "&url=" + encodeURIComponent(document.URL); });
};