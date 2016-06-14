// Allow IE8 & IE9 to make a cross-origin request. 
// Required by JQuery autocomplete when using the remote template from a domain other than www.eastsussex.gov.uk.
// Also required to access service alerts from domains other than www.eastsussex.gov.uk

// Should this library not work for a future requirement, try https://github.com/MoonScript/jQuery-ajaxTransport-XDomainRequest 
// Suggestion is it may be better http://stackoverflow.com/questions/10232017/ie9-jquery-ajax-with-cors-returns-access-is-denied

// Source: https://github.com/jaubourg/ajaxHooks/blob/master/src/xdr.js
if (typeof (jQuery) != 'undefined' && window.XDomainRequest) {
	jQuery.ajaxTransport(function( s ) {
		if ( s.crossDomain && s.async ) {
			if ( s.timeout ) {
				s.xdrTimeout = s.timeout;
				delete s.timeout;
			}
			var xdr;
			return {
				send: function( _, complete ) {
					function callback( status, statusText, responses, responseHeaders ) {
						xdr.onload = xdr.onerror = xdr.ontimeout = jQuery.noop;
						xdr = undefined;
						complete( status, statusText, responses, responseHeaders );
					}
					xdr = new XDomainRequest();
					xdr.onload = function() {
						callback( 200, "OK", { text: xdr.responseText }, "Content-Type: " + xdr.contentType );
					};
					xdr.onerror = function() {
						callback( 404, "Not Found" );
					};
					xdr.onprogress = jQuery.noop;
					xdr.ontimeout = function() {
						callback( 0, "timeout" );
					};
					xdr.timeout = s.xdrTimeout || Number.MAX_VALUE;
					xdr.open( s.type, s.url );
					xdr.send( ( s.hasContent && s.data ) || null );
				},
				abort: function() {
					if ( xdr ) {
						xdr.onerror = jQuery.noop;
						xdr.abort();
					}
				}
			};
		}
	});
}