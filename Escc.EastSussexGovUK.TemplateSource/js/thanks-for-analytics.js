// When asynchronous actions are taken after a form submission in ASP.NET WebForms, we cannot use a server-side redirect from a POSTed WebForm to a separate thank you page. 
// This ends the request and prevents the asynchronous method from executing. Instead the original web page hides its form controls and displays some 
// 'thank you' text.
//
// With this approach the URL doesn't change when the form is submitted, but we need it to change to track the conversion rate for the form in Google Analytics. 
// The workaround is, when we display the 'thank you' controls, to load this JavaScript which redirects on the client-side to `?thankyou`. 
// Google Analytics can track that change of URL, and a server-side check for that querystring can redisplay the 'thank you' controls instead of the form.

if (!document.location.search || !document.location.search.match(/(\?|&)thankyou/)) {
    var append = (document.location.search) ? "&thankyou" : "?thankyou";
    document.location.href = document.location.href + append;
}