/***
* Methods which alter the behaviour of the website and e-library when 
* accessed from an e-library computer in a public library. The public
* computer is identified using a modified Internet Explorer user agent.
* http://www.winguides.com/registry/display.php/799/
* We know these computers have JavaScript enabled and no screen-reader installed.
*/

// Identify public library computer
if(navigator.userAgent.indexOf("ESCC Libraries") > -1)
{

	// Redirect to e-library home (hides user credentials if signed in)
	function SignOut()
	{
	    var links = document.getElementsByTagName('a');
	    var len = links.length;
	    for (i = 0; i < len; i++)
	    {
	        if (links[i].innerHTML == 'Sign out')
	        {
	            document.location.href = links[i].getAttribute('href');
	            return;
	        }	        
	    }
		
		document.location.href = "http://www.eastsussex.gov.uk/libraries/elibrary/timeout.aspx";
	}
	
	// Set up a 5-minute timeout for redirect, which resets when the mouse or keyboard is used
	var timeOut;
	function ResetTimeOut()
	{
		window.clearTimeout(timeOut);
		timeOut = window.setTimeout('SignOut()', 300000);
	}
	document.onmousemove = ResetTimeOut;
	document.onkeypress = ResetTimeOut;
	ResetTimeOut();
	
	// Alter external links and links to downloads to point to standard message. Public library computers don't have 
	// access to external sites or Acrobat/Word viewers, so without this they get a Websense screen or error.
	function OnCurrentPageLoad_ExternalLinks()
	{
		if (document.getElementsByTagName)
		{
			// If the link ends with one of these file extensions, it needs a viewer
			var viewerExts = new Array(".pdf", ".rtf", ".doc", ".xls", ".wma", ".mp3");
			
			var redirectTo = 'http://www.eastsussex.gov.uk/libraries/elibrary/howtouse/librarycomputers.htm';
			
			var links = document.getElementsByTagName("a");
			if (links)
			{
				var href;
				for (var i = 0; i < links.length; i++)
				{
					href = links[i].getAttribute('href');
					if (href)
					{
						// If the link starts with http and doesn't mention our domain, it's external.
						// Not perfect, but it'll catch almost everything.
						href = href.toLowerCase();
						if (href.indexOf('http') == 0 && href.indexOf('eastsussex.gov.uk') == -1 && href.indexOf('eastsussexcc.gov.uk') == -1)
						{
							links[i].href = redirectTo;
						}
						
						// Check for viewer file exts
						for (var j = 0; j < viewerExts.length; j++)
						{
							if (href.substring(href.length-viewerExts[j].length, href.length) == viewerExts[j]) links[i].href = redirectTo;
						}
					}
				}
			}
		}
	}
	
	$(OnCurrentPageLoad_ExternalLinks);
}

