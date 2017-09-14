if (typeof (jQuery) !== 'undefined' && typeof (jQuery.fn.bt) !== 'undefined') {
        // Add pop-up help to a form control which appears on focus and disappears on blur, and includes a WCAG 2.1-compatible close function.
        //
        // Example HTML:
        //
        // <label for="example">Example</label>
        // <input type="text" id="example" aria-describedby="example-help" class="describedby-tip" />
        // <p id="example-help">We'll use this data for xxx. We won't use it for anything else or share it. You can ask us to delete it.</p>

        // Attach to elements which have help text associated using aria-describedby, so that the help is also read aloud by screenreaders.
        // The pop-up help is placed at the bottom of the DOM where they wouldn't otherwise see it. Look for .describedby-tip too so we don't
        // accidentally attach to other elements using aria-describedby.
    (function($) {
        $.fn.describedByTip = function (options) {

            // Apply tip to each matched element
            return this.each(function() {

                var input = $(this);
                if (input.hasClass("has-describedby-tip")) return;

                // Add user options to defaults for all matched elements.
                // Do so inside .each so that we can go on to set options from data-* attributes within the scope of just this element.
                var optionsForThisElement = $.extend({
                    trigger: ['focus', 'blur'],
                    fill: '#ffc',
                    strokeStyle: '#c0c060',
                    showTip: function (box) {
                        $(box).fadeIn(500);
                    },
                    hideTip: function (box, callback) {
                        $(box).animate({ opacity: 0 }, 500, callback);
                    },
                    shrinkToFit: true
                }, options);

                // Allow preferred positions for an individual element to be specified by a data-tip-positions attribute, 
                // which overrides the global options passed in for all elements
                if (input.data("tip-positions")) {
                    optionsForThisElement.positions = input.data("tip-positions").split(" ");
                }

                // WCAG 2.1: The pop-up tip may obscure other essential content, so provide a way to close it using a pointing device.
                //           This link does nothing until it's wired up later.
                var tip = $("#" + input.attr("aria-describedby"));
                var tipHtml = tip.html() + ' [<a href="#" data-closes-tip="' + input.attr("id") + '">close</a>]';

                input.bt(tipHtml, optionsForThisElement)
                    // WCAG 2.1: The pop-up tip may obscure other essential content, so provide a way to close it using a keyboard shortcut.
                    .keyup(function(e) {
                        // Look for Ctrl+Alt+x pressed when the form element is focussed. This actually toggles the pop-up help rather than just closing it.
                        if (e.ctrlKey && e.altKey && e.keyCode === 88) {
                            $(this).btOff().focus();
                        }
                    })
                    .addClass("has-describedby-tip");

                // Move the original help element off-screen where it can still be accessed by screenreaders via aria-describedby, 
                // and add text telling those users about the keyboard shortcut we've set up.
                tip.addClass("aural").append(' Press Ctrl + Alt + X to close this help bubble.');

                // WCAG 2.1: Wire up the close link we inserted after the pop-up help content. Because it's not in the DOM until the pop-up appears,
                //           we can't attach to the link itself. Instead attach to the pop-up's parent, which is the html element. Use an HTML class
                //           to check whether we've already done this, so that we don't use excess memory by attaching multiple event handlers.
                var html = $("html");
                if (!html.hasClass("has-describedby-tip")) {
                    html.addClass("has-describedby-tip");
                    html.click(function(e) {
                        var clicked = $(e.target);
                        if (!clicked.data("closes-tip")) return;
                        $("#" + clicked.data("closes-tip")).btOff().focus();
                        e.preventDefault();
                    });
                }
            });
        }
    }(jQuery));

    $(function () {
        $("[aria-describedby].describedby-tip").describedByTip();
    });
}