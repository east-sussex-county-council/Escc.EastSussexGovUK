if (jQuery != 'undefined' && esccGoogleMaps != 'undefined') {
    $(function () {
        esccGoogleMaps.loadGoogleMapsApi({ callback: "esccEmbedLinkedGoogleMap" });
    });
}

function esccEmbedLinkedGoogleMap() {

    // Embed a map of locations using our location template
    $("a.embed")
        .filter(function () { return /\/umbraco\/api\/location\/list/.test(this.href); })
        .each(function () {

            // Create a Google map from the link, or the parent element if that is a paragraph around the link
            var dataUrl = this.href;
            var target = $(this);
            if (target.parent()[0].tagName == "P" && target.parent().children().length == 1) { // this test misses text nodes, but the link should be on its own line
                target = target.parent();
            }
            var mapContainer = $('<div class="google-map"></div>');
            target.replaceWith(mapContainer);

            var map = esccGoogleMaps.createMap(mapContainer[0]);

            var infoWindow = new google.maps.InfoWindow({ maxWidth: 200 });

            // Use code from https://github.com/johnkpaul/jquery-ajax-retry to mitigate against network errors, which are the main
            // cause of no JavaScript according to gov.uk research https://gds.blog.gov.uk/2013/10/21/how-many-people-are-missing-out-on-javascript-enhancement/
            //
            // Add vary={origin} to querystring as setting Vary header from view is getting overridden. Need to vary the response by origin somehow, otherwise it requests the alerts
            // from Origin A, caches the response with a CORS header allowing Origin A, then requests the alerts from Origin B, gets the cached version and fails the CORS test.
            $.ajax({ dataType: "json", url: dataUrl + "&vary=" + document.location.protocol + "//" + document.location.host }).retry({ times: 3 }).then(function (data) {

                var bounds = new google.maps.LatLngBounds();

                for (var i = 0; i < data.length; i++) {

                    // Place a marker for this location on the map
                    var coords = new google.maps.LatLng(data[i].Latitude, data[i].Longitude);

                    data[i].marker = new google.maps.Marker({
                        position: coords,
                        map: map,
                        title: data[i].Name,
                        description: data[i].Description,
                        url: data[i].Url
                    });

                    // Open an InfoWindow with a clickable link when a location is clicked
                    data[i].marker.addListener('click', function () {
                        infoWindow.setContent("<h2><a href=\"" + this.url + "\">" + this.title + "</a></h2>" + this.description);
                        infoWindow.open(map, this);
                    });

                    // Include this marker when we resize the map to show all markers
                    bounds.extend(coords);
                }

                // Resize the map to show all markers
                map.fitBounds(bounds);
            });
        });



    // Embed a map from Google My Maps at https://www.google.co.uk/maps/d/u/0/
    $("a.embed")
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
