// This function needs to be in the global namespace because its a callback used by the basic Click4Assistance window.
function InitialiseC4A() {
    var Tool1 = new C4A.Tools(1);
    C4A.Run('48a533c9-94ba-418a-9e5b-01b42c45d0d6');
}

if (document.addEventListener) {
    document.addEventListener('DOMContentLoaded', function () {
        var head = document.getElementsByTagName('head')[0];
        var script = document.createElement('script');
        script.charset = 'utf-8';
        script.defer = true;
        script.src = 'https://v4in1-si.click4assistance.co.uk/SI.js';
        if (head) {
            head.appendChild(script);
        }
    });
}