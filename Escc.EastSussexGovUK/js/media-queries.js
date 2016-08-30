/**
* @projectDescription Polyfill min-width media queries in non-supporting browsers.
* @author Rick Mason
*/


/**
* A media query in a non-supporting browser. Tested with IE 6, IE 7 and IE 8.
* @constructor
* @param {String} linkElementClass Class name applied to a link element which uses an unsupported media query.
* @param {Number} breakpoint Pixel width at which the media query should be applied.
* @param {String} [ieConditionalClass] Optional class name applied to a link element with no media query, which is inside an IE conditional comment.
* @return {MediaQuery}
*/
function MediaQuery(linkElementClass, breakpoint, ieConditionalClass) {

    /**
    * Apply the media query now, and whenever the window is resized.
    * @return {void}
    */
    this.apply = function () {

        /* A duplicate link element without a media query can be placed inside an Internet Explorer
        conditional comment, to ensure users without JavaScript get all stylesheets applied.
        Now that we're using JavaScript, we can delete these elements.
        */
        if (ieConditionalClass) {
            $("link." + ieConditionalClass, "head").each(removeStylesheet);
        }

        /* Make copies of all the stylesheets we want to work with. */
        var stylesheets = $(copyStylesheets(linkElementClass));

        /* Create a function which applies the media query, run it now and hook it up to the resize event. */
        function applyQuery() {
            stylesheets.each(function () {
                this.disabled = document.documentElement.offsetWidth < breakpoint;
            });
        }
        applyQuery();
        $(window).resize(applyQuery);
    };

    /**
    * Copy link tags which use media queries to new ones which don't. Simply removing the media attribute 
    * doesn't work as it doesn't force Internet Explorer to reload the stylesheet. Apply screen media
    * because we probably don't want these stylesheets to print. 
    * @internal
    */
    function copyStylesheets(className) {
        var stylesheets = [];

        $("link[class~='" + className + "']", "head").each(function () {
            var a = document.createElement("link");
            a.rel = this.rel;
            a.type = this.type;
            a.href = this.href;
            a.media = "screen";
            stylesheets.push(a);
            this.parentNode.insertBefore(a, this);
            removeStylesheet(this);
        });

        return stylesheets;
    }

    /**
    * Remove the stylesheet from the DOM. Prevents Internet Explorer loading the stylesheet twice in the F12 developer tools.
    * @internal
    */
    function removeStylesheet(stylesheet) {
        stylesheet = stylesheet || this;
        stylesheet.parentNode.removeChild(stylesheet);
    }
}

if (typeof (jQuery) != 'undefined')
{
    // Polyfill media queries for old browsers with JavaScript enabled
    if (!Modernizr.mq('only screen and (min-width: 474px)')) {
        new MediaQuery("mqMedium", 474, "mqIEMedium").apply();
        new MediaQuery("mqLarge", 802, "mqIELarge").apply();
    }

    function onEsccBreakpointChange(onSmallActivated, onMediumActivated, onLargeActivated, smallMediumBreakpoint, mediumLargeBreakpoint) {
        /// <summary>Hook up functions to respond to responsive design breakpoints</summary>
        var widthBefore = $(window).width();
        smallMediumBreakpoint = (smallMediumBreakpoint || 474);
        mediumLargeBreakpoint = (mediumLargeBreakpoint || 802);

        // React to current size as soon as events are wired up
        if (widthBefore < smallMediumBreakpoint) {
            if (typeof (onSmallActivated) == 'function') {
                onSmallActivated(null, widthBefore);
            }
        }
        else if (widthBefore < mediumLargeBreakpoint) {
            if (typeof (onMediumActivated) == 'function') {
                onMediumActivated(null, widthBefore);
            }
        }
        else if (typeof (onLargeActivated) == 'function') {
            onLargeActivated(null, widthBefore);
        }

        // React to future changes in window size
        $(window).resize(function () {

            var widthAfter = $(window).width();

            if ((widthBefore < smallMediumBreakpoint || widthBefore >= mediumLargeBreakpoint) &&
                (widthAfter >= smallMediumBreakpoint && widthAfter < mediumLargeBreakpoint)) {
                if (typeof (onMediumActivated) == 'function') {
                    onMediumActivated(widthBefore, widthAfter);
                }
            }
            else if (widthBefore < mediumLargeBreakpoint && widthAfter >= mediumLargeBreakpoint) {
                if (typeof (onLargeActivated) == 'function') {
                    onLargeActivated(widthBefore, widthAfter);
                }
            }
            else if (widthBefore >= smallMediumBreakpoint && widthAfter < smallMediumBreakpoint) {
                if (typeof (onSmallActivated) == 'function') {
                    onSmallActivated(widthBefore, widthAfter);
                }
            }

            widthBefore = widthAfter;
        });
    }
}