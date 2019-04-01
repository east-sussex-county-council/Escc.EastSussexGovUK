using System.Web.UI.HtmlControls;
using Escc.EastSussexGovUK.TemplateSource.MasterPages.Controls;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.TemplateSource.Tests
{
    [TestFixture]
    public class SelectedSectionMatcherTests
    {
        [Test]
        public void SelectedSection_no_match_returns_null()
        {
            var text = "Test";
            var control = new HtmlGenericControl()
            {
                InnerText = "Libraries"
            };

            var matcher = new SelectedSectionMatcher();
            var matched = matcher.MatchSection(text, new[] { control });

            Assert.IsNull(matched);
        }

        [Test]
        public void SelectedSection_exact_match_is_found()
        {
            var text = "Libraries";
            var control = new HtmlGenericControl()
            {
                InnerText = "Libraries"
            };

            var matcher = new SelectedSectionMatcher();
            var matched = matcher.MatchSection(text, new [] { control });

            Assert.IsNotNull(matched);
        }

        [Test]
        public void SelectedSection_ampersand_matches_and()
        {
            var text = "Environment and planning";
            var control = new HtmlGenericControl()
            {
                InnerText = "Environment & planning"
            };

            var matcher = new SelectedSectionMatcher();
            var matched = matcher.MatchSection(text, new[] { control });

            Assert.IsNotNull(matched);
        }

        [Test]
        public void SelectedSection_line_break_matches_space()
        {
            var text = "Your Council";
            var control = new HtmlGenericControl()
            {
                InnerText = "Your<br />Council"
            };

            var matcher = new SelectedSectionMatcher();
            var matched = matcher.MatchSection(text, new[] { control });

            Assert.IsNotNull(matched);
        }

        [Test]
        public void SelectedSection_first_word_match_is_found()
        {
            var text = "Business in East Sussex";
            var control = new HtmlGenericControl()
            {
                InnerText = "Business"
            };

            var matcher = new SelectedSectionMatcher();
            var matched = matcher.MatchSection(text, new[] { control });

            Assert.IsNotNull(matched);
        }

        [Test]
        public void SelectedSection_exact_match_beats_first_word()
        {
            var text = "Business";
            var exact = new HtmlGenericControl()
            {
                InnerText = "Business"
            };
            var firstWord = new HtmlGenericControl()
            {
                InnerText = "Business in East Sussex"
            };

            var matcher = new SelectedSectionMatcher();
            var matched = matcher.MatchSection(text, new[] { firstWord, exact });

            Assert.AreEqual(exact, matched);
        }
    }
}
