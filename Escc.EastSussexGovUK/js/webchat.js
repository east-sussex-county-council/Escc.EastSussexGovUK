// This function needs to be in the global namespace because its a callback used by the basic Click4Assistance window.
function C4AWJSLoaded() {
    oC4AW_Widget = new oC4AW_Widget();
    oC4AW_Widget.setAccGUID("0f062fe2-f1c1-4727-9009-678f13d3a9e8");
    oC4AW_Widget.setWSGUID("f96394e1-d493-4cb7-852d-c725a30de8fd");
    oC4AW_Widget.setWFGUID("38424a83-823d-48b2-b013-63275f6ffe4e");
    oC4AW_Widget.setPopupWindowWFGUID("59976160-4b48-44d7-b135-36207f209a5a");
    oC4AW_Widget.setDockPosition("BOTTOM");
    oC4AW_Widget.setBtnStyle("position:fixed; border:none; bottom:0em; right:1em;");
    oC4AW_Widget.setIdentity("Embedded Chat");
    oC4AW_Widget.Initilize();
}

// This function needs to be in the global namespace because its a callback used by the Click4Assistance tracking/proactive feature
var C4A_TB;
function C4AJSJustLoaded() {
    C4A_TB = C4ATB();
    C4A_TB.setAccountGUID('0f062fe2-f1c1-4727-9009-678f13d3a9e8');
    C4A_TB.setWebsiteGUID('f96394e1-d493-4cb7-852d-c725a30de8fd');
    C4A_TB.setUseCookie(true);
    C4A_TB.Run();
}

$(function () {
    function load(url) {
        var head = document.getElementsByTagName('head')[0];
        var script = document.createElement('script');
        script.charset = 'utf-8';
        script.defer = true;
        script.src = url;
        if (head) {
            head.appendChild(script);
        }
    }

    load('https://prod3si.click4assistance.co.uk/JS/ChatWidget.js');
    load('https://prod3si.click4assistance.co.uk/JS/TrackProactive.js');
});