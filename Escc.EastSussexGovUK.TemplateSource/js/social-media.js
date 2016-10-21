// Code from http://code.google.com/p/analytics-api-samples/source/browse/trunk/src/tracking/javascript/v5/social/ga_social_tracking.js
// but with unused bits removed

// Copyright 2011 Google Inc. All Rights Reserved.

/**
* @fileoverview A simple script to automatically track Facebook 
* buttons using Google Analytics social tracking feature.
* @author api.nickm@google.com (Nick Mihailovski)
*/


/**
* Namespace.
* @type {Object}.
*/
var _ga = _ga || {};


/**
* Tracks Facebook likes, unlikes and sends by suscribing to the Facebook
* JSAPI event model. Note: This will not track facebook buttons using the
* iFrame method.
* @param {string} opt_pageUrl An optional URL to associate the social
*     tracking with a particular page.
* @param {string} opt_trackerName An optional name for the tracker object.
*/
_ga.trackFacebook = function ()
{
    try
    {
        if (FB && FB.Event && FB.Event.subscribe)
        {
            FB.Event.subscribe('edge.create', function (targetUrl) {
                ga('send', 'social', 'facebook', 'like', targetUrl);
            });
            FB.Event.subscribe('edge.remove', function (targetUrl) {
                ga('send', 'social', 'facebook', 'unlike', targetUrl);
            });
            FB.Event.subscribe('message.send', function (targetUrl) {
                ga('send', 'social', 'facebook', 'send', targetUrl);
            });
        }
    } catch (e) { }
};


/* Wire up the Facebook 'Like' button asynchronously including Google Analytics social tracking 
   Code from http://analytics-api-samples.googlecode.com/svn/trunk/src/tracking/javascript/v5/social/facebook_js_async.html
   but using our Facebook app_id 
*/
(function ()
{
    var fb = document.getElementById('fb-root');
    if (!fb) return;

    var e = document.createElement('script'); e.async = true;
    e.src = document.location.protocol + '//connect.facebook.net/en_US/all.js';
    fb.appendChild(e);
}());

window.fbAsyncInit = function ()
{
    FB.init({ appId: '169406409819518', status: true, cookie: true,
        xfbml: true
    });

    _ga.trackFacebook();
};