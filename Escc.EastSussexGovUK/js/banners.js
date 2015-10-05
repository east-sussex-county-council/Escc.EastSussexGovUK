// esccConfig is from config.js 
// cascadingContentFilter is from cascading-content.js

if (typeof (jQuery) !== 'undefined' && typeof (esccConfig) !== 'undefined' && typeof (cascadingContentFilter) !== 'undefined') {
    $(function () {
        /** 
        * Run the XHR request and load the banners only if the window is large enough to display them
        * */
        var loaded = false;
        function loadContent() {
            if (loaded) return;

            if (!window.matchMedia || window.matchMedia("(min-width: 802px)").matches) {
                loadBanners();
                loaded = true;
            }
        }

        $(window).resize(loadContent);
        loadContent();

        /**
         * Load banner data using XHR, filter out those that don't apply to this page, and display
         */
        function loadBanners() {
            var bannersContainer = $(".escc-banners");
            if (bannersContainer.length) {

                // Use code from https://github.com/johnkpaul/jquery-ajax-retry to mitigate against network errors, which are the main
                // cause of no JavaScript according to gov.uk research https://gds.blog.gov.uk/2013/10/21/how-many-people-are-missing-out-on-javascript-enhancement/
                //
                // For IE8 and IE9 this also relies on https://github.com/jaubourg/ajaxHooks/blob/master/src/xdr.js to enable cross-domain requests.
                // 
                // Add vary={origin} to querystring as setting Vary header from view is getting overridden. Need to vary the response by origin somehow, otherwise it requests the alerts
                // from Origin A, caches the response with a CORS header allowing Origin A, then requests the alerts from Origin B, gets the cached version and fails the CORS test.
                $.ajax({ dataType: "json", url: esccConfig.BannersUrl + "?vary=" + document.location.protocol + "//" + document.location.host }).retry({ times: 3 }).then(function (data) {


                    var banners = data;
                    var cascade = cascadingContentFilter();
                    banners = cascade.filterByUrl(banners, window.location.pathname);
                    banners = cascade.filterByInherit(banners, window.location.pathname);
                    banners = cascade.filterByCascade(banners, window.location.pathname);
                    banners = cascade.filterIfBlank(banners, function (item) { return !item.BannerImage; });

                    displayBanners(banners, bannersContainer);
                });
            }
        }

        function displayBanners(banners, container) {
            /// <summary>Display banners on the page</summary>
            if (banners.length) {

                // If page had no right column, add one.
                $(".full-page").removeClass(".full-page").addClass("article");

                // Build and insert HTML for banners.
                var html = '';
                $.each(banners, function(key, banner) {
                    var img = '<img src="' + htmlEncode(banner.BannerImage.ImageUrl) + '" alt="' + htmlEncode(banner.BannerImage.AlternativeText) + '" width="' + htmlEncode(banner.BannerImage.Width) + '" height="' + htmlEncode(banner.BannerImage.Height) + '" />';
                    if (banner.BannerLink) {
                        img = '<a href="' + htmlEncode(banner.BannerLink) + '">' + img + "</a>";
                    }
                    html += '<div class="supporting large advert">' + img + '</div>';
                });

                container.replaceWith(html);
            } else {
                container.remove();
            }
        }

        function htmlEncode(value) {
            //create a in-memory div, set it's inner text(which jQuery automatically encodes)
            //then grab the encoded contents back out.  The div never exists on the page.
            return $('<div/>').text(value).html();
        }
    });
}