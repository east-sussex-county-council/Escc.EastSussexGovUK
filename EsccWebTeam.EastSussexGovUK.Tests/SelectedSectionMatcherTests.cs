using System.Web.UI.HtmlControls;
using EsccWebTeam.EastSussexGovUK.MasterPages.Controls;
using NUnit.Framework;

namespace EsccWebTeam.EastSussexGovUK.Tests
{
    [TestFixture]
    public class SelectedSectionMatcherTests
    {
        [Test]
        public void NoMatchReturnsNull()
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
        public void ExactMatchIsFound()
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
        public void AmpersandMatchesAnd()
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
        public void FirstWordMatchIsFound()
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
        public void ExactMatchBeatsFirstWord()
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
