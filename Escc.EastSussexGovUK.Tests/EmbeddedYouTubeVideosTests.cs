using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Features;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Tests
{
    [TestFixture]
    public class EmbeddedYouTubeVideosTests
    {
        [Test]
        public void YouTubeVideoUrlIsRecognised()
        {
            const string html = "<a href=\"https://www.youtube.com/watch?v=N_dUmDBfp6k\">YouTube video</a>";

            var feature = new EmbeddedYouTubeVideos() {Html = new[] {html}};

            Assert.IsTrue(feature.IsRequired());
        }

        [Test]
        public void YouTubeShareUrlIsRecognised()
        {
            const string html = "<a href=\"http://youtu.be/N_dUmDBfp6k\">YouTube video</a>";

            var feature = new EmbeddedYouTubeVideos() { Html = new[] { html } };

            Assert.IsTrue(feature.IsRequired());
        }
    }
}
