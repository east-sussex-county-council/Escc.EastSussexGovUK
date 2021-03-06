﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;
using Escc.EastSussexGovUK.Features;
using Escc.Net;
using Escc.Net.Configuration;
using Microsoft.Extensions.Options;
using Xunit;

namespace Escc.EastSussexGovUK.Tests
{
    public class WebChatTests
    {
        private WebChatSettings _model;

        public WebChatTests()
        {
            // These were the actual URLs specified in the initial implemenatation of web chat
            var chatInclude = new [] {
                "/contactus/",
                "/roadsandtransport/roads/default.htm",
                "/roadsandtransport/roads/roadworks/",
                "/roadsandtransport/roads/maintenance/", 
                "/roadsandtransport/roads/contactus/",
                "/roadsandtransport/roads/roadadoption/",
                "/roadsandtransport/roads/roadsafety/",
                "/roadsandtransport/bexhillhastingslinkroad/",
                "/roadsandtransport/roads/roadschemes/uckfieldtransport/",
                "/community/emergencyplanningandcommunitysafety/emergencyplanning/advice/flooding/",
                "/leisureandtourism/countryside/rightsofway/",
                "/leisureandtourism/localandfamilyhistory/",
                "/libraries/activities-and-events/library-volunteers/",
                "/libraries/elibrary/howtouse/",
                "/libraries/find/",
                "/libraries/reference/",
                "/environment/rubbishandrecycling/recyclingsites/",
                "/environment/woodlands/dutchelms/",
                "/socialcare/aboutus/contact.htm",
                "/socialcare/aboutus/complaints/default.htm",
                "/socialcare/gettinghelp/default.htm",
                "/socialcare/gettinghelp/apply/",
                "/socialcare/gettinghelp/eligibility/",
                "/socialcare/disability/learning/contacts/socialcaredirect.htm"
            };

            var chatExclude = new[]{
                "/contactus/apply/default.htm",
                "/contactus/pay/",
                "/contactus/languages.htm",
                "/contactus/interpreting.htm",
                "/libraries/find/northiam/default.htm",
                "/libraries/find/rotherfield/default.htm",
                "/libraries/find/sedlescombe/default.htm"
            };

            _model = new WebChatSettings();
            foreach (var url in chatInclude) _model.WebChatUrls.Add(new Uri(url, UriKind.Relative));
            foreach (var url in chatExclude) _model.ExcludedUrls.Add(new Uri(url, UriKind.Relative));
        }

        [Fact]
        public void WebChat_matches_exact_URL()
        {
            _model.PageUrl = new Uri("/roadsandtransport/roads/default.htm", UriKind.Relative);
            
            var feature = new WebChat() {WebChatSettings = _model};

            Assert.True(feature.IsRequired());
        }

        [Fact]
        public void WebChat_matches_folder_URL()
        {
             _model.PageUrl = new Uri("/contactus/some-section/default.htm", UriKind.Relative);
            
            var feature = new WebChat() {WebChatSettings = _model};

            Assert.True(feature.IsRequired());
        }

        [Fact]
        public void WebChat_does_not_match_excluded_exact_URL()
        {
            _model.PageUrl = new Uri("/contactus/apply/default.htm", UriKind.Relative);

            var feature = new WebChat() { WebChatSettings = _model };

            Assert.False(feature.IsRequired());
        }

        [Fact]
        public void WebChat_does_not_match_excluded_folder_URL()
        {
            _model.PageUrl = new Uri("/contactus/pay/default.htm", UriKind.Relative);

            var feature = new WebChat() { WebChatSettings = _model };

            Assert.False(feature.IsRequired());
        }

        //[Fact]
        // This is an integration test - use for testing changes, but it will not always pass because it relies on local changeable data 
        public async Task WebChatApiRequest()
        {
            var apiSettings = Options.Create(new WebChatApiSettings { WebChatSettingsUrl = new Uri("https://www.eastsussex.gov.uk/umbraco/api/webchat/getwebchaturls") });
            var source = new WebChatSettingsFromApi(apiSettings,new HttpClientProvider(new ConfigurationProxyProvider()), null);
            var settings = await source.ReadWebChatSettings();
            settings.PageUrl = new Uri("/some-relative-url");
            Assert.Equal("/yourcouncil/", settings.WebChatUrls[0].ToString());
        }
    }
}
