// The icon used for our web chat header is by Freepik (www.freepik.com) from www.flaticon.com, and licensed under CC BY 3.0.

// Use a self-executing function, but run it now. If we wait for an event like DOM ready the document will be closed,
// and any attempt to use document.write will wipe out all the content. LogoNet code uses document.write so we're forced to.

// Similarly the data is requested from a remote source but, because of document.write, we can't do that with XHR because it's
// async and document.write isn't. Instead the data is periodically downloaded by a scheduled task, concatenated with this script,
// and made available as a global variable esccWebChatData.

// This version of the script is used on www.eastsussex.gov.uk, and will ultimately be replaced by webchat.js
(function () {
    if (typeof (jQuery) != 'undefined' && typeof (esccWebChatData) != 'undefined') {

        var chatInclude = esccWebChatData.include;
        var chatExclude = esccWebChatData.exclude;

        var found = function(paths) {
            var len = paths.length;
            for (var i = 0; i < len; i++) {
                if (location.pathname.indexOf(paths[i]) === 0) {
                    return true;
                }
            }
            return false;
        }

        var includeScript = function() {
            // LogoNet code for including web chat
            var script = "https://eastsussex.logo-net.co.uk/Delivery/DBURLssl.php";
            var page = parent.document.URL;
            page = page.replace(/&/g, "^");
            page = page.toLowerCase();
            page = page.replace(/</g, "-1");
            page = page.replace(/>/g, "-2");
            page = page.replace(/%3c/g, "-1");
            page = page.replace(/%3e/g, "-2");
            document.write("<scr" + "ipt src='" + script);
            document.write("?SDTID=192");
            document.write('&PURL=' + page);
            document.write('&CMS=' + new Date().getTime());
            document.write("'></scr" + "ipt>");
        }

        var insertDiv = function() {
            // Aim for top of the right column. Look first for a single .article and insert after it.
            // If that fails look for any right-col content, and insert before it.
            var elements = $(".article");
            var chat = $('<div class="supporting large" id="webchat" />')[0];
            if (elements.length === 1) {
                elements[0].parentNode.insertBefore(chat, elements[0].nextSibling);
                return true;
            } else {
                elements = $(".supporting,.supporting-text");
                if (elements.length) {
                    elements[0].parentNode.insertBefore(chat, elements[0]);
                    return true;
                }
            }
            return false;
        }

        // Decide whether to load resources from LogoNet
        if (found(chatInclude) && !found(chatExclude)) {
            if (insertDiv()) {
                includeScript();
            }
        };
    }
})();