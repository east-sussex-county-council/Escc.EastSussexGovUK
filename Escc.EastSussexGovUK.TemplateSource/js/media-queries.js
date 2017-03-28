if (typeof (jQuery) != 'undefined')
{
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