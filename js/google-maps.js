if (jQuery != 'undefined') {
    var esccGoogleMaps = (function () {
        /// <summary>Utility library for ESCC applications using the Google Maps API v3</summary>
        if (typeof esccConfig === 'undefined') return null;

        this.loadGoogleMapsApi = function (options) {
            /// <summary>Load Google maps API asynchronously and then run the callback function</summary>
            var defaultOptions = { libraries: '', callback: '' };
            if (options) $.extend(defaultOptions, options);

            var script = document.createElement("script");
            var sensor = ($(".header .alphabet").length > 0) ? 'false' : 'true'; // look for A-Z to see whether we're using desktop template
            script.src = "https://maps.googleapis.com/maps/api/js?libraries=" + defaultOptions.libraries + "&key=" + esccConfig.GoogleMapsApiKey + "&sensor=" + sensor + "&callback=" + defaultOptions.callback;
            document.body.appendChild(script);
        };

        this.createMap = function () {
            /// <summary>Create a Google map centred on East Sussex</summary>
            var mapOptions = {
                center: eastSussexCentre(),
                zoom: 10,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            return new google.maps.Map(document.getElementById("google-map"), mapOptions);
        };

        this.eastSussexCentre = function () {
            /// <summary>Gets a LatLng representing the centre point of East Sussex</summary>
            return new google.maps.LatLng(50.96674, 0.256443);
        };

        this.addLocationSearch = function (searchBoxId, searchButtonId, map) {
            /// <summary>Wire up the search box to zoom to a typed location using the Google geocoder</summary>
            var input = document.getElementById(searchBoxId);
            var button = $("#" + searchButtonId);

            addLocationSearchAutocomplete(input, button);
            addLocationSearchAction(input, button, map);
        };

        function addLocationSearchAutocomplete(input, button) {
            ///<summary>Add the standard Google autocomplete to the search box, biased towards an area a little wider than East Sussex </summary>
            var options = {
                bounds: new google.maps.LatLngBounds(
                        new google.maps.LatLng(51.198279, -0.252686),
                        new google.maps.LatLng(50.713852, 1.065674)),
                componentRestrictions: { country: 'uk' }
            };
            var autocomplete = new google.maps.places.Autocomplete(input, options);

            google.maps.event.addListener(autocomplete, 'place_changed', function () {
                var place = autocomplete.getPlace();
                if (place.formatted_address) input.value = place.formatted_address;
                button.click();
            });
        }

        function addLocationSearchAction(input, button, map) {
            ///<summary>Go to the place typed by the user, or selected from the Google autocomplete</summary>
            var geocoder = new google.maps.Geocoder();

            button.click(function (e) {
                e.preventDefault();
                geocoder.geocode(
                    { 'address': input.value },
                    function (results, status) {
                        if (status == google.maps.GeocoderStatus.OK) {
                            var loc = results[0].geometry.location;
                            map.setCenter(new google.maps.LatLng(loc.lat(), loc.lng()));
                            map.setZoom(15);
                        }
                    }
                );
            });
        }

        this.htmlEncode = function (html) {
            /// <summary>Encodes an HTML string for display, but not suitable for attributes</summary>
            return document.createElement('a').appendChild(document.createTextNode(html)).parentNode.innerHTML;
        };

        return this;
    })();
}