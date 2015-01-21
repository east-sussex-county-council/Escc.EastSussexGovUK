if (jQuery != 'undefined' && esccGoogleMaps != 'undefined') {
    $(function () {
        esccGoogleMaps.loadGoogleMapsApi({ callback: "esccEmbedLinkedGoogleMap" });
    });
}

function esccEmbedLinkedGoogleMap() {
    $("a")
    .filter(function () { return /maps\.google\.co\.uk\/maps\/ms\?msid=[0-9a-f.]+&msa=0/.test(this.href); })
    .each(function () {
        // Note: frameborder attribute is deprecated, but still needed by IE8
        var mapUrl = this.href + '&ie=UTF8&t=m&output=embed';
        if (!this.href.match(/&z=[0-9]/)) {
            mapUrl += "&z=14";
        }
        var iframe = $('<div class="googlemap"><iframe frameborder="0" width="100%" height="400" seamless="seamless" title="Map" src="' + mapUrl + '"></iframe><p><a href="' + mapUrl + '">View a larger map</a></p></div>')[0];
        replaceWithMap(this, iframe);
    });

    function replaceWithMap(replaceThis, withThis) {
        // Has this link got any siblings other than white space?
        var hasSiblings = false;
        var siblings = replaceThis.parentNode.childNodes;
        var len = siblings.length;
        for (var i = 0; i < len; i++) {
            if (siblings[i] == replaceThis) continue;
            if (/[^\t\n\r ]/.test(siblings[i].data)) hasSiblings = true;
        }

        // Replace the original link, and its container if empty, with the map
        var replace = hasSiblings ? replaceThis : replaceThis.parentNode;
        $(withThis).insertBefore(replace);
        $(replace).remove();
    }
}
