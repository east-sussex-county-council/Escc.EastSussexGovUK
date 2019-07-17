/* Update the language translation links in the sitewide footer to include the URL of the page where the link was clicked.
 * We expect that to be the page in need of translation, so it is helpful to provide the URL automatically. */
if (typeof (jQuery) != "undefined") {
    $("div.languages a").attr("href", function () { return this.href + "&url=" + encodeURIComponent(document.URL); });
};