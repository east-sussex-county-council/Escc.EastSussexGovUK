/**
 * Embed YouTube videos responsively
 * 
 * Options (specified in data-* attributes on the page):
 *      data-video-width="int"     - sets a fixed width for videos on the page
 *      data-video-height="int"    - sets a fixed height for videos on the page
 *      data-video-max-width="int" - updates the maximum width for videos (default 600px)
 *      data-video-resize="bool"   - if set to false, disables responsive resizing of video
 */

if (typeof (jQuery) !== 'undefined') {

    // http://www.paulirish.com/2009/throttled-smartresize-jquery-event-handler/
    (function ($, sr) {

        // debouncing function from John Hann
        // http://unscriptable.com/index.php/2009/03/20/debouncing-javascript-methods/
        var debounce = function (func, threshold, execAsap) {
            var timeout;

            return function debounced() {
                var obj = this, args = arguments;
                function delayed() {
                    if (!execAsap)
                        func.apply(obj, args);
                    timeout = null;
                };

                if (timeout)
                    clearTimeout(timeout);
                else if (execAsap)
                    func.apply(obj, args);

                timeout = setTimeout(delayed, threshold || 100);
            };
        }
        // smartresize 
        jQuery.fn[sr] = function (fn) { return fn ? this.bind('resize', debounce(fn)) : this.trigger(sr); };

    })(jQuery, 'smartresize');



    // By Chris Coyier & tweaked by Mathias Bynens
    // https://css-tricks.com/fluid-width-youtube-videos/

    $(function () {

        function youTubeDimension(attr, defaultSize) {
            var customDimension = parseInt($("[data-" + attr + "]").data(attr));
            if (customDimension) {
                return customDimension;
            }
            return defaultSize;
        }

        // Embed YouTube videos
        var youTube = new RegExp(/^https?:\/\/(youtu.be\/|www.youtube.com\/watch\?v=)([A-Za-z0-9_-]+)(&feature=youtu.be)?$/);
        var youTubeWidth = youTubeDimension("video-width", 450), 
            youTubeHeight = youTubeDimension("video-height", 253), 
            youTubeMaxWidth = youTubeDimension("video-max-width", 600),
            youTubeFixedSize = $("[data-video-resize=false]").length > 0;

        $("a.embed").filter(function (index) {
            return youTube.test(this.href);
        }).each(function () {

            // Swop YouTube link for embedded video
                var match = youTube.exec(this.href);
                $(this).replaceWith('<iframe width="' + youTubeWidth + '" height="' + youTubeHeight + '" src="https://www.youtube-nocookie.com/embed/' + match[2] + '?enablejsapi=1&origin=' + encodeURIComponent(document.location.origin) + '" frameborder="0" allowfullscreen="allowfullscreen" class="video"></iframe>');
        });

        if (!youTubeFixedSize) {
            // Find embedded YouTube videos
            // Figure out and save aspect ratio for each video
            var $allVideos = $("iframe[src^='https://www.youtube-nocookie.com']");
            $allVideos.each(function() {

                $(this)
                    .data('aspect-ratio', this.height / this.width)

                    // and remove the hard coded width/height
                    .removeAttr('height')
                    .removeAttr('width');
            });

            // When the window is resized
            $(window).smartresize(function() {

                // Resize all videos according to their own aspect ratio
                $allVideos.each(function() {

                    var $el = $(this);

                    // The parent element is expected to be fluid width
                    var newWidth = $el.parent().width();
                    if (newWidth > youTubeMaxWidth) {
                        newWidth = youTubeMaxWidth;
                    }

                    $el.width(newWidth)
                        .height(newWidth * $el.data('aspect-ratio'));

                });

                // Kick off one resize to fix all videos on page load
            }).resize();
        }
    });
}