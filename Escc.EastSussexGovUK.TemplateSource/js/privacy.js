// When the page loads, show a privacy alert at the top of the page.
// Offer a close button, and don't show it again once it's closed.
//
// Note: using var rather than let only because YUICompressor.MSBuild doesn't understand let
document.addEventListener("DOMContentLoaded", function () {
    "use strict";
    var consent = window.localStorage && window.localStorage.getItem("eguk-consent");

    if (!consent) {
        var privacyAlert = document.createElement("div");
        if (privacyAlert.classList) {
            privacyAlert.classList.add("privacy-alert");
        }
        privacyAlert.innerHTML = '<div class="container"><div class="content text-content"><p><small>By using this site you consent to the cookies and third-party services we use &#8211; see <a href="https://www.eastsussex.gov.uk/privacy/">Privacy and cookies</a>.</small> <form id="close-privacy-alert"><div><button>OK, I understand.</button></div></form></p></div></div>';

        var whereToPlaceIt = document.getElementsByTagName("body");
        if (whereToPlaceIt && whereToPlaceIt.length >= 1 && whereToPlaceIt[0].childNodes.length >= 1) {
            whereToPlaceIt[0].insertBefore(privacyAlert, whereToPlaceIt[0].childNodes[0]);

            var closeAlert = document.getElementById("close-privacy-alert");
            if (closeAlert) {
                closeAlert.addEventListener("submit", function (e) {
                    e.preventDefault();
                    if (window.localStorage) {
                        window.localStorage.setItem("eguk-consent", new Date().toISOString());
                    }
                    if (privacyAlert.classList) {
                        privacyAlert.classList.add("closed");
                    }
                });
            }
        }
    }
});