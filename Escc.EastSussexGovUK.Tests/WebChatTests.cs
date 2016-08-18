using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;
using Escc.EastSussexGovUK.Features;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Tests
{
    [TestFixture]
    public class WebChatTests
    {
        private WebChatSettings _model;

        [SetUp]
        public void Setup()
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

        [Test]
        public void ExactUrlIsMatched()
        {
            _model.PageUrl = new Uri("/roadsandtransport/roads/default.htm", UriKind.Relative);
            
            var feature = new WebChat() {WebChatSettings = _model};

            Assert.IsTrue(feature.IsRequired());
        }

        [Test]
        public void FolderUrlIsMatched()
        {
             _model.PageUrl = new Uri("/contactus/some-section/default.htm", UriKind.Relative);
            
            var feature = new WebChat() {WebChatSettings = _model};

            Assert.IsTrue(feature.IsRequired());
        }

        [Test]
        public void ExcludedExactUrlIsNotMatched()
        {
            _model.PageUrl = new Uri("/contactus/apply/default.htm", UriKind.Relative);

            var feature = new WebChat() { WebChatSettings = _model };

            Assert.IsFalse(feature.IsRequired());
        }

        [Test]
        public void ExcludedFolderUrlIsNotMatched()
        {
            _model.PageUrl = new Uri("/contactus/pay/default.htm", UriKind.Relative);

            var feature = new WebChat() { WebChatSettings = _model };

            Assert.IsFalse(feature.IsRequired());
        }
    }
}
